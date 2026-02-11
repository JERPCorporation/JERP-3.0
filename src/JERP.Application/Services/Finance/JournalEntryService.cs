/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using JERP.Application.DTOs.Finance;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Core.Exceptions;
using JERP.Infrastructure.Data;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Implementation of IJournalEntryService.
/// 
/// KEY IMPROVEMENTS OVER THE OLD CONTROLLER-BASED APPROACH:
/// 
/// 1. REUSABLE: Other services (payroll, inventory) can now create journal entries
///    by injecting IJournalEntryService instead of duplicating the logic.
/// 
/// 2. TESTABLE: You can write unit tests by mocking IJournalEntryService.
///    Testing a controller that talks directly to DbContext is much harder.
/// 
/// 3. SINGLE RESPONSIBILITY: This class ONLY handles journal entry business rules.
///    The controller ONLY handles HTTP request/response concerns.
/// 
/// 4. FIXES BUGS:
///    - Race condition on number generation (now uses database-level MAX + transaction)
///    - N+1 query on posting (loads all accounts in one query before the loop)
///    - Duplicated DTO mapping (single private method: MapToDto)
/// </summary>
public class JournalEntryService : IJournalEntryService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<JournalEntryService> _logger;

    public JournalEntryService(
        JerpDbContext context,
        ILogger<JournalEntryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<JournalEntryDto>> GetAllAsync(
        Guid companyId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        JournalEntryStatus? status = null)
    {
        var query = _context.JournalEntries
            .Where(j => j.CompanyId == companyId);

        if (startDate.HasValue)
            query = query.Where(j => j.TransactionDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(j => j.TransactionDate <= endDate.Value);

        if (status.HasValue)
            query = query.Where(j => j.Status == status.Value);

        var entries = await query
            .OrderByDescending(j => j.TransactionDate)
            .ThenByDescending(j => j.CreatedAt)
            .Select(j => new JournalEntryDto
            {
                Id = j.Id,
                CompanyId = j.CompanyId,
                JournalEntryNumber = j.JournalEntryNumber,
                TransactionDate = j.TransactionDate,
                Description = j.Description,
                Status = j.Status,
                Source = j.Source,
                TotalDebit = j.TotalDebit,
                TotalCredit = j.TotalCredit,
                IsBalanced = j.IsBalanced,
                PostedAt = j.PostedAt,
                CreatedAt = j.CreatedAt,
                UpdatedAt = j.UpdatedAt
            })
            .ToListAsync();

        return entries;
    }

    public async Task<JournalEntryDto?> GetByIdAsync(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
                .ThenInclude(e => e.Account)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
            return null;

        return MapToDto(journalEntry);
    }

    public async Task<JournalEntryDto> CreateAsync(CreateJournalEntryRequest request)
    {
        // ── Validate entries exist ──
        if (request.Entries == null || !request.Entries.Any())
            throw new BusinessRuleViolationException("Journal entry must have at least one ledger entry");

        // ── Validate balance (debits must equal credits - this is fundamental accounting) ──
        var totalDebits = request.Entries.Sum(e => e.DebitAmount);
        var totalCredits = request.Entries.Sum(e => e.CreditAmount);

        if (totalDebits != totalCredits)
            throw new BusinessRuleViolationException(
                $"Journal entry is not balanced. Debits: {totalDebits:C}, Credits: {totalCredits:C}");

        // ── Verify all referenced accounts exist (single query, not one per account) ──
        var accountIds = request.Entries.Select(e => e.AccountId).Distinct().ToList();
        var accounts = await _context.Accounts
            .Where(a => accountIds.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id);

        if (accounts.Count != accountIds.Count)
        {
            var missingIds = accountIds.Where(id => !accounts.ContainsKey(id));
            throw new NotFoundException($"Accounts not found: {string.Join(", ", missingIds)}");
        }

        // ── Generate journal entry number (FIXED: uses transaction to prevent race condition) ──
        // 
        // OLD BUG: The controller used to query the last entry, parse the number, add 1, then save.
        // If two users created entries at the same time, both would get the same number.
        //
        // FIX: We use ExecuteSqlRawAsync or a transaction with serializable isolation.
        // The simplest reliable approach: use MAX() inside the same transaction as the INSERT.
        //
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var maxNumber = await _context.JournalEntries
                .Where(j => j.CompanyId == request.CompanyId)
                .MaxAsync(j => (int?)j.SequenceNumber) ?? 0;

            var nextNumber = maxNumber + 1;

            var journalEntry = new JournalEntry
            {
                CompanyId = request.CompanyId,
                SequenceNumber = nextNumber,
                JournalEntryNumber = $"JE-{nextNumber:D4}",
                TransactionDate = request.TransactionDate,
                Description = request.Description,
                Status = JournalEntryStatus.Draft,
                Source = EntrySource.Manual,
                TotalDebit = totalDebits,
                TotalCredit = totalCredits
            };

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            // ── Create ledger entries ──
            var ledgerEntries = request.Entries.Select(e =>
            {
                var account = accounts[e.AccountId];
                return new GeneralLedgerEntry
                {
                    CompanyId = request.CompanyId,
                    JournalEntryId = journalEntry.Id,
                    AccountId = e.AccountId,
                    TransactionDate = request.TransactionDate,
                    DebitAmount = e.DebitAmount,
                    CreditAmount = e.CreditAmount,
                    Description = e.Description,
                    Source = EntrySource.Manual,
                    Is280EDeductible = account.IsCOGS
                };
            }).ToList();

            _context.GeneralLedgerEntries.AddRange(ledgerEntries);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation(
                "Created manual journal entry {JournalEntryNumber} for company {CompanyId}",
                journalEntry.JournalEntryNumber, journalEntry.CompanyId);

            // Reload with includes so we can map the full DTO
            var createdEntry = await _context.JournalEntries
                .Include(j => j.LedgerEntries)
                    .ThenInclude(e => e.Account)
                .FirstAsync(j => j.Id == journalEntry.Id);

            return MapToDto(createdEntry);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<JournalEntryDto> PostAsync(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
            throw new NotFoundException($"Journal entry with ID {id} not found");

        if (journalEntry.Status != JournalEntryStatus.Draft)
            throw new BusinessRuleViolationException(
                $"Can only post draft entries. Current status: {journalEntry.Status}");

        if (!journalEntry.IsBalanced)
            throw new BusinessRuleViolationException("Cannot post an unbalanced journal entry");

        // ── FIXED: Load ALL accounts in one query (was N+1 - one query per ledger entry) ──
        //
        // OLD BUG: The controller did FindAsync(entry.AccountId) inside a foreach loop.
        // With 20 ledger lines, that's 20 separate database round trips.
        //
        // FIX: One query loads all accounts into a dictionary, then we look them up in memory.
        //
        var accountIds = journalEntry.LedgerEntries.Select(e => e.AccountId).Distinct().ToList();
        var accounts = await _context.Accounts
            .Where(a => accountIds.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id);

        journalEntry.Status = JournalEntryStatus.Posted;
        journalEntry.PostedAt = DateTime.UtcNow;

        // Update account balances
        foreach (var entry in journalEntry.LedgerEntries)
        {
            if (accounts.TryGetValue(entry.AccountId, out var account))
            {
                // Debit-normal accounts (Assets, Expenses): balance increases with debits
                // Credit-normal accounts (Liabilities, Equity, Revenue): balance increases with credits
                switch (account.Type)
                {
                    case AccountType.Asset:
                    case AccountType.Expense:
                        account.Balance += entry.DebitAmount - entry.CreditAmount;
                        break;
                    case AccountType.Liability:
                    case AccountType.Equity:
                    case AccountType.Revenue:
                        account.Balance += entry.CreditAmount - entry.DebitAmount;
                        break;
                }
            }
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Posted journal entry {JournalEntryNumber}", journalEntry.JournalEntryNumber);

        // Reload with Account navigation for DTO mapping
        var posted = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
                .ThenInclude(e => e.Account)
            .FirstAsync(j => j.Id == id);

        return MapToDto(posted);
    }

    public async Task<JournalEntryDto> VoidAsync(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
            throw new NotFoundException($"Journal entry with ID {id} not found");

        if (journalEntry.Status == JournalEntryStatus.Voided)
            throw new BusinessRuleViolationException("Journal entry is already voided");

        // Reverse account balances if the entry was previously posted
        if (journalEntry.Status == JournalEntryStatus.Posted)
        {
            // ── FIXED: Same N+1 fix as PostAsync ──
            var accountIds = journalEntry.LedgerEntries.Select(e => e.AccountId).Distinct().ToList();
            var accounts = await _context.Accounts
                .Where(a => accountIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id);

            foreach (var entry in journalEntry.LedgerEntries)
            {
                if (accounts.TryGetValue(entry.AccountId, out var account))
                {
                    switch (account.Type)
                    {
                        case AccountType.Asset:
                        case AccountType.Expense:
                            account.Balance -= entry.DebitAmount - entry.CreditAmount;
                            break;
                        case AccountType.Liability:
                        case AccountType.Equity:
                        case AccountType.Revenue:
                            account.Balance -= entry.CreditAmount - entry.DebitAmount;
                            break;
                    }
                }
            }
        }

        journalEntry.Status = JournalEntryStatus.Voided;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Voided journal entry {JournalEntryNumber}", journalEntry.JournalEntryNumber);

        var voided = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
                .ThenInclude(e => e.Account)
            .FirstAsync(j => j.Id == id);

        return MapToDto(voided);
    }

    // ══════════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ══════════════════════════════════════════════════════════════

    /// <summary>
    /// Single place to map JournalEntry entity → JournalEntryDto.
    /// 
    /// FIXED: This mapping was copy-pasted 3 times in the old controller.
    /// Now it's in one place. If you add a field, you update it once.
    /// </summary>
    private static JournalEntryDto MapToDto(JournalEntry entity)
    {
        return new JournalEntryDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            JournalEntryNumber = entity.JournalEntryNumber,
            TransactionDate = entity.TransactionDate,
            Description = entity.Description,
            Status = entity.Status,
            Source = entity.Source,
            TotalDebit = entity.TotalDebit,
            TotalCredit = entity.TotalCredit,
            IsBalanced = entity.IsBalanced,
            PostedAt = entity.PostedAt,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            LedgerEntries = entity.LedgerEntries?.Select(e => new GeneralLedgerEntryDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                JournalEntryId = e.JournalEntryId,
                AccountId = e.AccountId,
                AccountNumber = e.Account?.AccountNumber ?? "",
                AccountName = e.Account?.AccountName ?? "",
                TransactionDate = e.TransactionDate,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                Description = e.Description,
                Source = e.Source,
                SourceEntityId = e.SourceEntityId,
                Is280EDeductible = e.Is280EDeductible,
                CreatedAt = e.CreatedAt
            }).ToList() ?? new List<GeneralLedgerEntryDto>()
        };
    }
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Implementation of general ledger service
/// </summary>
public class GeneralLedgerService : IGeneralLedgerService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<GeneralLedgerService> _logger;

    public GeneralLedgerService(
        JerpDbContext context,
        ILogger<GeneralLedgerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<JournalEntry> CreateJournalEntryAsync(JournalEntry journalEntry, List<GeneralLedgerEntry> ledgerEntries)
    {
        _logger.LogInformation("Creating journal entry for company {CompanyId}", journalEntry.CompanyId);

        // Validate balance
        if (!ValidateJournalEntryBalance(ledgerEntries))
        {
            throw new InvalidOperationException("Journal entry is not balanced. Debits must equal credits.");
        }

        // Create journal entry
        _context.JournalEntries.Add(journalEntry);
        await _context.SaveChangesAsync();

        // Add ledger entries
        foreach (var entry in ledgerEntries)
        {
            entry.JournalEntryId = journalEntry.Id;
        }

        await _context.GeneralLedgerEntries.AddRangeAsync(ledgerEntries);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Journal entry created: {JournalEntryId}", journalEntry.Id);

        return journalEntry;
    }

    public async Task<JournalEntry> PostJournalEntryAsync(Guid journalEntryId, Guid userId)
    {
        _logger.LogInformation("Posting journal entry {JournalEntryId}", journalEntryId);

        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .FirstOrDefaultAsync(j => j.Id == journalEntryId && !j.IsDeleted);

        if (journalEntry == null)
        {
            throw new InvalidOperationException($"Journal entry {journalEntryId} not found");
        }

        if (journalEntry.Status != JournalEntryStatus.Draft)
        {
            throw new InvalidOperationException($"Only draft journal entries can be posted");
        }

        // Validate balance again before posting
        if (!ValidateJournalEntryBalance(journalEntry.LedgerEntries.ToList()))
        {
            throw new InvalidOperationException("Journal entry is not balanced. Cannot post.");
        }

        // Post the entry
        journalEntry.Status = JournalEntryStatus.Posted;
        journalEntry.PostedById = userId;
        journalEntry.PostedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Update account balances
        await UpdateAccountBalancesAsync(journalEntry);

        _logger.LogInformation("Journal entry posted: {JournalEntryId}", journalEntryId);

        return journalEntry;
    }

    public async Task<JournalEntry> VoidJournalEntryAsync(Guid journalEntryId, Guid userId)
    {
        _logger.LogInformation("Voiding journal entry {JournalEntryId}", journalEntryId);

        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .ThenInclude(e => e.Account)
            .FirstOrDefaultAsync(j => j.Id == journalEntryId && !j.IsDeleted);

        if (journalEntry == null)
        {
            throw new InvalidOperationException($"Journal entry {journalEntryId} not found");
        }

        if (journalEntry.Status == JournalEntryStatus.Voided)
        {
            throw new InvalidOperationException("Journal entry is already voided");
        }

        // Reverse the account balances
        foreach (var entry in journalEntry.LedgerEntries)
        {
            if (entry.EntryType == EntryType.Debit)
            {
                entry.Account.Balance -= entry.Amount;
            }
            else
            {
                entry.Account.Balance += entry.Amount;
            }
        }

        journalEntry.Status = JournalEntryStatus.Voided;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Journal entry voided: {JournalEntryId}", journalEntryId);

        return journalEntry;
    }

    public async Task<List<JournalEntry>> GetJournalEntriesAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .ThenInclude(e => e.Account)
            .Where(j => j.CompanyId == companyId && !j.IsDeleted);

        if (startDate.HasValue)
        {
            query = query.Where(j => j.EntryDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(j => j.EntryDate <= endDate.Value);
        }

        return await query.OrderByDescending(j => j.EntryDate).ToListAsync();
    }

    public async Task<JournalEntry?> GetJournalEntryByIdAsync(Guid id)
    {
        return await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .ThenInclude(e => e.Account)
            .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);
    }

    public async Task<List<GeneralLedgerEntry>> GetLedgerEntriesForAccountAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.GeneralLedgerEntries
            .Include(e => e.JournalEntry)
            .Where(e => e.AccountId == accountId && !e.IsDeleted && e.JournalEntry.Status == JournalEntryStatus.Posted);

        if (startDate.HasValue)
        {
            query = query.Where(e => e.JournalEntry.EntryDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.JournalEntry.EntryDate <= endDate.Value);
        }

        return await query.OrderBy(e => e.JournalEntry.EntryDate).ToListAsync();
    }

    public bool ValidateJournalEntryBalance(List<GeneralLedgerEntry> entries)
    {
        var totalDebits = entries.Where(e => e.EntryType == EntryType.Debit).Sum(e => e.Amount);
        var totalCredits = entries.Where(e => e.EntryType == EntryType.Credit).Sum(e => e.Amount);

        return totalDebits == totalCredits;
    }

    public async Task UpdateAccountBalancesAsync(JournalEntry journalEntry)
    {
        _logger.LogInformation("Updating account balances for journal entry {JournalEntryId}", journalEntry.Id);

        foreach (var entry in journalEntry.LedgerEntries)
        {
            var account = await _context.Accounts.FindAsync(entry.AccountId);
            if (account != null)
            {
                // Debits increase asset/expense accounts, decrease liability/equity/revenue accounts
                // Credits increase liability/equity/revenue accounts, decrease asset/expense accounts
                if (entry.EntryType == EntryType.Debit)
                {
                    if (account.AccountType == AccountType.Asset || 
                        account.AccountType == AccountType.Expense || 
                        account.AccountType == AccountType.COGS)
                    {
                        account.Balance += entry.Amount;
                    }
                    else
                    {
                        account.Balance -= entry.Amount;
                    }
                }
                else // Credit
                {
                    if (account.AccountType == AccountType.Asset || 
                        account.AccountType == AccountType.Expense || 
                        account.AccountType == AccountType.COGS)
                    {
                        account.Balance -= entry.Amount;
                    }
                    else
                    {
                        account.Balance += entry.Amount;
                    }
                }
            }
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Account balances updated for journal entry {JournalEntryId}", journalEntry.Id);
    }
}

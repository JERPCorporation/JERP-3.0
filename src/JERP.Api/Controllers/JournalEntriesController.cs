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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Finance;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;

namespace JERP.Api.Controllers;

/// <summary>
/// Journal entry management endpoints
/// </summary>
[Route("api/v1/finance/journal-entries")]
[Authorize]
public class JournalEntriesController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<JournalEntriesController> _logger;

    public JournalEntriesController(
        JerpDbContext context,
        ILogger<JournalEntriesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// List journal entries
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetJournalEntries(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] JournalEntryStatus? status = null)
    {
        var query = _context.JournalEntries
            .Where(j => j.CompanyId == companyId);

        if (startDate.HasValue)
        {
            query = query.Where(j => j.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(j => j.TransactionDate <= endDate.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(j => j.Status == status.Value);
        }

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

        return Ok(entries);
    }

    /// <summary>
    /// Get journal entry details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJournalEntry(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
                .ThenInclude(e => e.Account)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
        {
            return NotFound($"Journal entry with ID {id} not found");
        }

        var dto = new JournalEntryDto
        {
            Id = journalEntry.Id,
            CompanyId = journalEntry.CompanyId,
            JournalEntryNumber = journalEntry.JournalEntryNumber,
            TransactionDate = journalEntry.TransactionDate,
            Description = journalEntry.Description,
            Status = journalEntry.Status,
            Source = journalEntry.Source,
            TotalDebit = journalEntry.TotalDebit,
            TotalCredit = journalEntry.TotalCredit,
            IsBalanced = journalEntry.IsBalanced,
            PostedAt = journalEntry.PostedAt,
            CreatedAt = journalEntry.CreatedAt,
            UpdatedAt = journalEntry.UpdatedAt,
            LedgerEntries = journalEntry.LedgerEntries.Select(e => new GeneralLedgerEntryDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                JournalEntryId = e.JournalEntryId,
                AccountId = e.AccountId,
                AccountNumber = e.Account.AccountNumber,
                AccountName = e.Account.AccountName,
                TransactionDate = e.TransactionDate,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                Description = e.Description,
                Source = e.Source,
                SourceEntityId = e.SourceEntityId,
                Is280EDeductible = e.Is280EDeductible,
                CreatedAt = e.CreatedAt
            }).ToList()
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create a manual journal entry
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateJournalEntry([FromBody] CreateJournalEntryRequest request)
    {
        if (request.Entries == null || !request.Entries.Any())
        {
            return BadRequest("Journal entry must have at least one ledger entry");
        }

        // Validate balance
        var totalDebits = request.Entries.Sum(e => e.DebitAmount);
        var totalCredits = request.Entries.Sum(e => e.CreditAmount);

        if (totalDebits != totalCredits)
        {
            return BadRequest($"Journal entry is not balanced. Debits: {totalDebits}, Credits: {totalCredits}");
        }

        // Verify all accounts exist
        var accountIds = request.Entries.Select(e => e.AccountId).Distinct().ToList();
        var accounts = await _context.Accounts
            .Where(a => accountIds.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id);

        if (accounts.Count != accountIds.Count)
        {
            return BadRequest("One or more accounts not found");
        }

        // Generate journal entry number
        var lastEntry = await _context.JournalEntries
            .Where(j => j.CompanyId == request.CompanyId)
            .OrderByDescending(j => j.CreatedAt)
            .FirstOrDefaultAsync();

        var nextNumber = 1;
        if (lastEntry != null)
        {
            nextNumber = int.Parse(lastEntry.JournalEntryNumber.Split('-')[1]) + 1;
        }

        var journalEntry = new JournalEntry
        {
            CompanyId = request.CompanyId,
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

        // Create ledger entries
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

        _logger.LogInformation("Created manual journal entry {JournalEntryNumber} for company {CompanyId}",
            journalEntry.JournalEntryNumber, journalEntry.CompanyId);

        // Return the created entry
        var createdEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
                .ThenInclude(e => e.Account)
            .FirstAsync(j => j.Id == journalEntry.Id);

        var dto = new JournalEntryDto
        {
            Id = createdEntry.Id,
            CompanyId = createdEntry.CompanyId,
            JournalEntryNumber = createdEntry.JournalEntryNumber,
            TransactionDate = createdEntry.TransactionDate,
            Description = createdEntry.Description,
            Status = createdEntry.Status,
            Source = createdEntry.Source,
            TotalDebit = createdEntry.TotalDebit,
            TotalCredit = createdEntry.TotalCredit,
            IsBalanced = createdEntry.IsBalanced,
            PostedAt = createdEntry.PostedAt,
            CreatedAt = createdEntry.CreatedAt,
            UpdatedAt = createdEntry.UpdatedAt,
            LedgerEntries = createdEntry.LedgerEntries.Select(e => new GeneralLedgerEntryDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                JournalEntryId = e.JournalEntryId,
                AccountId = e.AccountId,
                AccountNumber = e.Account.AccountNumber,
                AccountName = e.Account.AccountName,
                TransactionDate = e.TransactionDate,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                Description = e.Description,
                Source = e.Source,
                SourceEntityId = e.SourceEntityId,
                Is280EDeductible = e.Is280EDeductible,
                CreatedAt = e.CreatedAt
            }).ToList()
        };

        return Created(dto);
    }

    /// <summary>
    /// Post a draft journal entry
    /// </summary>
    [HttpPost("{id}/post")]
    public async Task<IActionResult> PostJournalEntry(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
        {
            return NotFound($"Journal entry with ID {id} not found");
        }

        if (journalEntry.Status != JournalEntryStatus.Draft)
        {
            return BadRequest($"Can only post draft entries. Current status: {journalEntry.Status}");
        }

        if (!journalEntry.IsBalanced)
        {
            return BadRequest("Cannot post an unbalanced journal entry");
        }

        journalEntry.Status = JournalEntryStatus.Posted;
        journalEntry.PostedAt = DateTime.UtcNow;

        // Update account balances
        foreach (var entry in journalEntry.LedgerEntries)
        {
            var account = await _context.Accounts.FindAsync(entry.AccountId);
            if (account != null)
            {
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

        return Ok(new { message = "Journal entry posted successfully" });
    }

    /// <summary>
    /// Void a journal entry
    /// </summary>
    [HttpPost("{id}/void")]
    public async Task<IActionResult> VoidJournalEntry(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.LedgerEntries)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
        {
            return NotFound($"Journal entry with ID {id} not found");
        }

        if (journalEntry.Status == JournalEntryStatus.Voided)
        {
            return BadRequest("Journal entry is already voided");
        }

        // Reverse account balances if the entry was posted
        if (journalEntry.Status == JournalEntryStatus.Posted)
        {
            foreach (var entry in journalEntry.LedgerEntries)
            {
                var account = await _context.Accounts.FindAsync(entry.AccountId);
                if (account != null)
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

        return Ok(new { message = "Journal entry voided successfully" });
    }
}

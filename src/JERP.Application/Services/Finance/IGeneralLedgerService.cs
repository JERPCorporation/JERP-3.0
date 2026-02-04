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

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for managing journal entries and general ledger
/// </summary>
public interface IGeneralLedgerService
{
    /// <summary>
    /// Create a journal entry with ledger entries
    /// </summary>
    Task<JournalEntry> CreateJournalEntryAsync(JournalEntry journalEntry, List<GeneralLedgerEntry> ledgerEntries);

    /// <summary>
    /// Post a journal entry to the general ledger
    /// </summary>
    Task<JournalEntry> PostJournalEntryAsync(Guid journalEntryId, Guid userId);

    /// <summary>
    /// Void a journal entry
    /// </summary>
    Task<JournalEntry> VoidJournalEntryAsync(Guid journalEntryId, Guid userId);

    /// <summary>
    /// Get journal entries for a company
    /// </summary>
    Task<List<JournalEntry>> GetJournalEntriesAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Get journal entry by ID with ledger entries
    /// </summary>
    Task<JournalEntry?> GetJournalEntryByIdAsync(Guid id);

    /// <summary>
    /// Get general ledger entries for an account
    /// </summary>
    Task<List<GeneralLedgerEntry>> GetLedgerEntriesForAccountAsync(Guid accountId, DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Validate that a journal entry is balanced (debits = credits)
    /// </summary>
    bool ValidateJournalEntryBalance(List<GeneralLedgerEntry> entries);

    /// <summary>
    /// Update account balances based on posted journal entry
    /// </summary>
    Task UpdateAccountBalancesAsync(JournalEntry journalEntry);
}

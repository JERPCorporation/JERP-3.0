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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Journal entry data transfer object
/// </summary>
public class JournalEntryDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public required string JournalEntryNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public required string Description { get; set; }
    public JournalEntryStatus Status { get; set; }
    public EntrySource Source { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsBalanced { get; set; }
    public DateTime? PostedAt { get; set; }
    public List<GeneralLedgerEntryDto> LedgerEntries { get; set; } = new();
    public List<JournalEntryLineDto> Lines { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Request to create a manual journal entry
/// </summary>
public class CreateJournalEntryRequest
{
    public Guid CompanyId { get; set; }
    public DateTime TransactionDate { get; set; }
    public required string Description { get; set; }
    public List<CreateLedgerEntryRequest> Entries { get; set; } = new();
}

/// <summary>
/// Request to create a ledger entry line
/// </summary>
public class CreateLedgerEntryRequest
{
    public Guid AccountId { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public required string Description { get; set; }
}

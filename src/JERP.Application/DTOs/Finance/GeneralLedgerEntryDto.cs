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
/// General ledger entry data transfer object
/// </summary>
public class GeneralLedgerEntryDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid JournalEntryId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public required string Description { get; set; }
    public EntrySource Source { get; set; }
    public Guid? SourceEntityId { get; set; }
    public bool Is280EDeductible { get; set; }
    public DateTime CreatedAt { get; set; }
}

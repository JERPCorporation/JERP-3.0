/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a single entry in the general ledger
/// </summary>
public class GeneralLedgerEntry : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the journal entry
    /// </summary>
    [Required]
    public Guid JournalEntryId { get; set; }

    /// <summary>
    /// Foreign key to the account
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Date of the transaction
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Debit amount (0 if credit entry)
    /// </summary>
    public decimal DebitAmount { get; set; } = 0;

    /// <summary>
    /// Credit amount (0 if debit entry)
    /// </summary>
    public decimal CreditAmount { get; set; } = 0;

    /// <summary>
    /// Description of this specific entry
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Source system that created this entry
    /// </summary>
    [Required]
    public EntrySource Source { get; set; } = EntrySource.Manual;

    /// <summary>
    /// Optional link to source entity (e.g., PayrollRecordId)
    /// </summary>
    public Guid? SourceEntityId { get; set; }

    /// <summary>
    /// Whether this entry is deductible under IRS 280E (cannabis tax compliance)
    /// </summary>
    public bool Is280EDeductible { get; set; } = false;

    // Navigation properties
    public Company Company { get; set; } = null!;
    public JournalEntry JournalEntry { get; set; } = null!;
    public Account Account { get; set; } = null!;
}

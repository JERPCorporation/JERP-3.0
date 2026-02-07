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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a journal entry containing one or more general ledger entries
/// </summary>
public class JournalEntry : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Journal entry number (e.g., "JE-0001")
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string JournalEntryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date of the transaction
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Description of the journal entry
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the journal entry
    /// </summary>
    [Required]
    public JournalEntryStatus Status { get; set; } = JournalEntryStatus.Draft;

    /// <summary>
    /// Source system that created this entry
    /// </summary>
    [Required]
    public EntrySource Source { get; set; } = EntrySource.Manual;

    /// <summary>
    /// Total debit amount
    /// </summary>
    public decimal TotalDebit { get; set; } = 0;

    /// <summary>
    /// Total credit amount
    /// </summary>
    public decimal TotalCredit { get; set; } = 0;

    /// <summary>
    /// Computed property to check if the entry is balanced
    /// </summary>
    public bool IsBalanced => TotalDebit == TotalCredit;

    /// <summary>
    /// Timestamp when the entry was posted
    /// </summary>
    public DateTime? PostedAt { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public ICollection<GeneralLedgerEntry> LedgerEntries { get; set; } = new List<GeneralLedgerEntry>();
}

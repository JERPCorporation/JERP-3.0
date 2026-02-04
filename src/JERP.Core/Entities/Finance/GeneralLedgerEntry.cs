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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a single line item in the general ledger (debit or credit)
/// </summary>
public class GeneralLedgerEntry : BaseEntity
{
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
    /// Entry type (Debit or Credit)
    /// </summary>
    [Required]
    public EntryType EntryType { get; set; }

    /// <summary>
    /// Amount of the entry
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// Description for this specific line item
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public JournalEntry JournalEntry { get; set; } = null!;
    public Account Account { get; set; } = null!;
}

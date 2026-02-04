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

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a cash reconciliation (cannabis-specific feature)
/// </summary>
public class CashReconciliation : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Location/vault identifier
    /// </summary>
    [MaxLength(100)]
    public string? Location { get; set; }

    /// <summary>
    /// Date of reconciliation
    /// </summary>
    [Required]
    public DateTime ReconciliationDate { get; set; }

    /// <summary>
    /// Expected cash balance from system
    /// </summary>
    [Required]
    public decimal ExpectedBalance { get; set; }

    /// <summary>
    /// Actual counted cash
    /// </summary>
    [Required]
    public decimal ActualBalance { get; set; }

    /// <summary>
    /// Variance (over/short)
    /// </summary>
    public decimal Variance { get; set; }

    // Bill denominations
    public int Count100s { get; set; } = 0;
    public int Count50s { get; set; } = 0;
    public int Count20s { get; set; } = 0;
    public int Count10s { get; set; } = 0;
    public int Count5s { get; set; } = 0;
    public int Count1s { get; set; } = 0;

    // Coin denominations (in cents)
    public int CountQuarters { get; set; } = 0;
    public int CountDimes { get; set; } = 0;
    public int CountNickels { get; set; } = 0;
    public int CountPennies { get; set; } = 0;

    /// <summary>
    /// User who performed the reconciliation
    /// </summary>
    public Guid? ReconciliatedById { get; set; }

    /// <summary>
    /// Notes/comments
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Journal entry created for variance (if any)
    /// </summary>
    public Guid? JournalEntryId { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public User? ReconciliatedBy { get; set; }
    public JournalEntry? JournalEntry { get; set; }
}

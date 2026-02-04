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
/// Represents an account in the chart of accounts
/// </summary>
public class Account : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Account number (e.g., "5000", "1000")
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account name (e.g., "Payroll Expense", "Cash - Operating")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Primary account type
    /// </summary>
    [Required]
    public AccountType Type { get; set; }

    /// <summary>
    /// Account sub-type for more detailed classification
    /// </summary>
    [Required]
    public AccountSubType SubType { get; set; }

    /// <summary>
    /// Current balance of the account
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Whether the account is active and available for use
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether this is a system account that cannot be deleted
    /// </summary>
    public bool IsSystemAccount { get; set; } = false;

    /// <summary>
    /// Whether this account is part of Cost of Goods Sold (cannabis 280E deductible)
    /// </summary>
    public bool IsCOGS { get; set; } = false;

    /// <summary>
    /// Whether expenses in this account are non-deductible under 280E
    /// </summary>
    public bool IsNonDeductible { get; set; } = false;

    /// <summary>
    /// Tax category for reporting purposes
    /// </summary>
    [MaxLength(100)]
    public string? TaxCategory { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public ICollection<GeneralLedgerEntry> LedgerEntries { get; set; } = new List<GeneralLedgerEntry>();
}

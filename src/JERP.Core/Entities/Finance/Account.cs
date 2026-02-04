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
/// Represents an account in the Chart of Accounts
/// </summary>
public class Account : BaseEntity
{
    /// <summary>
    /// Account code/number (e.g., "1000", "4000")
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>
    /// Account name/description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Type of account (Asset, Liability, Equity, Revenue, Expense, COGS)
    /// </summary>
    [Required]
    public AccountType AccountType { get; set; }

    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Optional parent account for hierarchical chart of accounts
    /// </summary>
    public Guid? ParentAccountId { get; set; }

    /// <summary>
    /// Current balance of the account
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Whether this account is for Cost of Goods Sold (cannabis 280E)
    /// </summary>
    public bool IsCOGS { get; set; } = false;

    /// <summary>
    /// Whether expenses in this account are 280E tax deductible (cannabis-specific)
    /// </summary>
    public bool Is280EDeductible { get; set; } = false;

    /// <summary>
    /// Whether this account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Description/notes about the account
    /// </summary>
    public string? Description { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public Account? ParentAccount { get; set; }
    public ICollection<Account> SubAccounts { get; set; } = new List<Account>();
    public ICollection<GeneralLedgerEntry> LedgerEntries { get; set; } = new List<GeneralLedgerEntry>();
}

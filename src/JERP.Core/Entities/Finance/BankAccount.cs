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
/// Represents a bank account
/// </summary>
public class BankAccount : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the GL account
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Bank name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Account name/description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Account number (last 4 digits only for security)
    /// </summary>
    [MaxLength(4)]
    public string? AccountNumberLast4 { get; set; }

    /// <summary>
    /// Routing number
    /// </summary>
    [MaxLength(9)]
    public string? RoutingNumber { get; set; }

    /// <summary>
    /// Account type (Checking, Savings, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? AccountType { get; set; }

    /// <summary>
    /// Current balance
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Whether the account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Notes
    /// </summary>
    public string? Notes { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

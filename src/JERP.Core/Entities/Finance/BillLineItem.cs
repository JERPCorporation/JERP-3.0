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
/// Represents a line item on a bill
/// </summary>
public class BillLineItem : BaseEntity
{
    /// <summary>
    /// Foreign key to the bill
    /// </summary>
    [Required]
    public Guid BillId { get; set; }

    /// <summary>
    /// Foreign key to the expense account
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Description of the line item
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// Unit price
    /// </summary>
    [Required]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total amount (Quantity * UnitPrice)
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    // Navigation properties
    public Bill Bill { get; set; } = null!;
    public Account Account { get; set; } = null!;
}

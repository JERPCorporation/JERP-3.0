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
/// Represents a bill from a vendor (Accounts Payable)
/// </summary>
public class Bill : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the vendor
    /// </summary>
    [Required]
    public Guid VendorId { get; set; }

    /// <summary>
    /// Bill number/reference
    /// </summary>
    [MaxLength(50)]
    public string? BillNumber { get; set; }

    /// <summary>
    /// Date of the bill
    /// </summary>
    [Required]
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Due date for payment
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Total amount of the bill
    /// </summary>
    [Required]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Amount already paid
    /// </summary>
    public decimal PaidAmount { get; set; } = 0;

    /// <summary>
    /// Status of the bill
    /// </summary>
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Description/memo
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Optional journal entry created for this bill
    /// </summary>
    public Guid? JournalEntryId { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public Vendor Vendor { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<BillLineItem> LineItems { get; set; } = new List<BillLineItem>();
    public ICollection<BillPayment> Payments { get; set; } = new List<BillPayment>();
}

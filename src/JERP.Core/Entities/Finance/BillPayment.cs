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
using System.ComponentModel.DataAnnotations.Schema;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a payment made to a vendor bill
/// </summary>
public class BillPayment : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to the bill
    /// </summary>
    [Required]
    public Guid BillId { get; set; }
    
    /// <summary>
    /// Payment number (auto-generated)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PaymentNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime PaymentDate { get; set; }
    
    /// <summary>
    /// Payment amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Payment method (e.g., Check, ACH, Wire, Cash)
    /// </summary>
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// Check number or transaction reference
    /// </summary>
    [MaxLength(50)]
    public string? ReferenceNumber { get; set; }
    
    /// <summary>
    /// Associated journal entry
    /// </summary>
    public Guid? JournalEntryId { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public VendorBill Bill { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
}

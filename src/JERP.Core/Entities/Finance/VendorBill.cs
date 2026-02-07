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
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a vendor bill/invoice in accounts payable
/// </summary>
public class VendorBill : BaseEntity
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
    /// Bill number (auto-generated)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string BillNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Vendor's invoice number
    /// </summary>
    [MaxLength(50)]
    public string? VendorInvoiceNumber { get; set; }
    
    /// <summary>
    /// Bill date
    /// </summary>
    public DateTime BillDate { get; set; }
    
    /// <summary>
    /// Due date
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Subtotal amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
    
    /// <summary>
    /// Tax amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Total amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Amount paid
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    /// Amount remaining
    /// </summary>
    [NotMapped]
    public decimal AmountRemaining => TotalAmount - AmountPaid;
    
    /// <summary>
    /// Bill status
    /// </summary>
    public BillStatus Status { get; set; } = BillStatus.Draft;
    
    /// <summary>
    /// Whether bill is paid
    /// </summary>
    public bool IsPaid { get; set; }
    
    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime? PaymentDate { get; set; }
    
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
    public Vendor Vendor { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<BillLineItem> LineItems { get; set; } = new List<BillLineItem>();
    public ICollection<BillPayment> Payments { get; set; } = new List<BillPayment>();
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a customer invoice in accounts receivable
/// </summary>
public class CustomerInvoice : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to the customer
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Invoice number (auto-generated)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Invoice date
    /// </summary>
    public DateTime InvoiceDate { get; set; }
    
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
    /// Invoice status
    /// </summary>
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    
    /// <summary>
    /// Whether invoice is paid
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
    public Customer Customer { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();
    public ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();
}

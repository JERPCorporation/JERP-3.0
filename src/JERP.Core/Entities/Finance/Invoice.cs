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
/// Represents an invoice to a customer (Accounts Receivable)
/// </summary>
public class Invoice : BaseEntity
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
    /// Invoice number/reference
    /// </summary>
    [MaxLength(50)]
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Date of the invoice
    /// </summary>
    [Required]
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Due date for payment
    /// </summary>
    [Required]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Total amount of the invoice
    /// </summary>
    [Required]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Amount already paid
    /// </summary>
    public decimal PaidAmount { get; set; } = 0;

    /// <summary>
    /// Status of the invoice
    /// </summary>
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Description/memo
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Optional journal entry created for this invoice
    /// </summary>
    public Guid? JournalEntryId { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();
    public ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();
}

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
/// Represents a payment (cash, check, ACH, etc.)
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the bank account (if applicable)
    /// </summary>
    public Guid? BankAccountId { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    [Required]
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment method (Cash, Check, ACH, Credit Card, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// Reference/check number
    /// </summary>
    [MaxLength(50)]
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Memo/description
    /// </summary>
    [MaxLength(500)]
    public string? Memo { get; set; }

    /// <summary>
    /// Status of the payment
    /// </summary>
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Journal entry created for this payment
    /// </summary>
    public Guid? JournalEntryId { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public BankAccount? BankAccount { get; set; }
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();
    public ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();
}

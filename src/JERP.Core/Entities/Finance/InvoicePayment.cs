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
/// Represents a payment received on an invoice
/// </summary>
public class InvoicePayment : BaseEntity
{
    /// <summary>
    /// Foreign key to the invoice
    /// </summary>
    [Required]
    public Guid InvoiceId { get; set; }

    /// <summary>
    /// Foreign key to the payment
    /// </summary>
    [Required]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Amount of this payment applied to the invoice
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
}

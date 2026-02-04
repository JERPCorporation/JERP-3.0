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

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Invoice line item DTO
/// </summary>
public class InvoiceLineItemDto
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public required string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>
/// Invoice payment DTO
/// </summary>
public class InvoicePaymentDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid InvoiceId { get; set; }
    public required string ReceiptNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Full invoice details DTO
/// </summary>
public class InvoiceDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerNumber { get; set; }
    public required string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue => TotalAmount - AmountPaid;
    public InvoiceStatus Status { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaymentDate { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? Notes { get; set; }
    public List<InvoiceLineItemDto> LineItems { get; set; } = new();
    public List<InvoicePaymentDto> Payments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Invoice list/summary DTO
/// </summary>
public class InvoiceListDto
{
    public Guid Id { get; set; }
    public required string InvoiceNumber { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue => TotalAmount - AmountPaid;
    public InvoiceStatus Status { get; set; }
    public int DaysOverdue
    {
        get
        {
            if (Status == InvoiceStatus.Paid || Status == InvoiceStatus.Void)
                return 0;
            var days = (DateTime.Now - DueDate).Days;
            return days > 0 ? days : 0;
        }
    }
}

/// <summary>
/// Create invoice line item DTO
/// </summary>
public class CreateInvoiceLineItemDto
{
    [Required]
    public Guid AccountId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
    
    [Range(0.0001, double.MaxValue)]
    public decimal Quantity { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}

/// <summary>
/// Create invoice DTO
/// </summary>
public class CreateInvoiceDto
{
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public DateTime InvoiceDate { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateInvoiceLineItemDto> LineItems { get; set; } = new();
}

/// <summary>
/// Update invoice DTO
/// </summary>
public class UpdateInvoiceDto
{
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public DateTime InvoiceDate { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateInvoiceLineItemDto> LineItems { get; set; } = new();
}

/// <summary>
/// Create invoice payment DTO
/// </summary>
public class CreateInvoicePaymentDto
{
    [Required]
    public Guid InvoiceId { get; set; }
    
    [Required]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }
    
    [MaxLength(50)]
    public string? ReferenceNumber { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

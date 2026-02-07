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
using JERP.Core.Enums;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Bill line item DTO
/// </summary>
public class BillLineItemDto
{
    public Guid Id { get; set; }
    public Guid BillId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public required string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public bool IsCOGS { get; set; }
}

/// <summary>
/// Bill payment DTO
/// </summary>
public class BillPaymentDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BillId { get; set; }
    public required string PaymentNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Full bill details DTO
/// </summary>
public class BillDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public Guid VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? VendorNumber { get; set; }
    public string? VendorInvoiceNumber { get; set; }
    public DateTime BillDate { get; set; }
    public DateTime DueDate { get; set; }
    public BillStatus StatusEnum { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string? Description { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? Notes { get; set; }
    public List<BillLineItemDto> LineItems { get; set; } = new(); // Original for services
    public List<BillLineDto> Lines { get; set; } = new(); // New for Desktop
    public List<BillPaymentDto> Payments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Bill list/summary DTO
/// </summary>
public class BillListDto
{
    public Guid Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public string? VendorInvoiceNumber { get; set; }
    public Guid VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public DateTime BillDate { get; set; }
    public DateTime DueDate { get; set; }
    public BillStatus StatusEnum { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }
}

/// <summary>
/// Create bill line item DTO
/// </summary>
public class CreateBillLineItemDto
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
    
    public bool IsCOGS { get; set; }
}

/// <summary>
/// Create bill DTO
/// </summary>
public class CreateBillDto
{
    [Required]
    public Guid VendorId { get; set; }
    
    [MaxLength(50)]
    public string? VendorInvoiceNumber { get; set; }
    
    [Required]
    public DateTime BillDate { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateBillLineItemDto> LineItems { get; set; } = new();
}

/// <summary>
/// Update bill DTO
/// </summary>
public class UpdateBillDto
{
    [Required]
    public Guid VendorId { get; set; }
    
    [MaxLength(50)]
    public string? VendorInvoiceNumber { get; set; }
    
    [Required]
    public DateTime BillDate { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateBillLineItemDto> LineItems { get; set; } = new();
}

/// <summary>
/// Create bill payment DTO
/// </summary>
public class CreateBillPaymentDto
{
    [Required]
    public Guid BillId { get; set; }
    
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

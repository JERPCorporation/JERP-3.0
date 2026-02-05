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

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Vendor bill DTO with 3-way matching support
/// </summary>
public class VendorBillDto
{
    public Guid Id { get; set; }
    
    public required string BillNumber { get; set; }
    
    public Guid VendorId { get; set; }
    
    public required string VendorName { get; set; }
    
    public Guid? PurchaseOrderId { get; set; }
    
    public string? PONumber { get; set; }
    
    public required string VendorInvoiceNumber { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public required string Status { get; set; }
    
    public List<VendorBillLineDto> LineItems { get; set; } = new();
    
    public decimal SubTotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public decimal AmountPaid { get; set; }
    
    public decimal AmountDue { get; set; }
    
    public bool IsPaid { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public Guid? JournalEntryId { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

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

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Create vendor bill request DTO
/// </summary>
public class CreateVendorBillRequest
{
    [Required]
    public Guid VendorId { get; set; }
    
    public Guid? PurchaseOrderId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string VendorInvoiceNumber { get; set; }
    
    [Required]
    public DateTime InvoiceDate { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateVendorBillLineRequest> LineItems { get; set; } = new();
    
    public decimal TaxAmount { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

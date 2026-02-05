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
/// Full purchase order details DTO
/// </summary>
public class PurchaseOrderDto
{
    public Guid Id { get; set; }
    
    public required string PONumber { get; set; }
    
    public Guid VendorId { get; set; }
    
    public required string VendorName { get; set; }
    
    public required string Status { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public DateTime ExpectedDeliveryDate { get; set; }
    
    public Guid WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public string? ShippingMethod { get; set; }
    
    public string? ShippingTerms { get; set; }
    
    public string? PaymentTerms { get; set; }
    
    public List<PurchaseOrderLineDto> LineItems { get; set; } = new();
    
    public decimal SubTotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal ShippingAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public decimal TotalReceived { get; set; }
    
    public bool IsFullyReceived { get; set; }
    
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    public string? VendorPONumber { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}

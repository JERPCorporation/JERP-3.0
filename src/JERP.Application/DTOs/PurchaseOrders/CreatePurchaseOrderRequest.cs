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
/// Create purchase order request DTO
/// </summary>
public class CreatePurchaseOrderRequest
{
    [Required]
    public Guid VendorId { get; set; }
    
    [Required]
    public DateTime OrderDate { get; set; }
    
    [Required]
    public DateTime ExpectedDeliveryDate { get; set; }
    
    [Required]
    public Guid WarehouseId { get; set; }
    
    [MaxLength(100)]
    public string? ShippingMethod { get; set; }
    
    [MaxLength(50)]
    public string? ShippingTerms { get; set; }
    
    [MaxLength(50)]
    public string? PaymentTerms { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreatePurchaseOrderLineRequest> LineItems { get; set; } = new();
    
    public decimal ShippingAmount { get; set; }
    
    [MaxLength(50)]
    public string? VendorPONumber { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}

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
/// Purchase order line item DTO
/// </summary>
public class PurchaseOrderLineDto
{
    public Guid Id { get; set; }
    
    public int LineNumber { get; set; }
    
    public Guid ProductId { get; set; }
    
    public required string ItemCode { get; set; }
    
    public required string ItemName { get; set; }
    
    public string? Description { get; set; }
    
    public decimal Quantity { get; set; }
    
    public required string UnitOfMeasure { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal LineTotal { get; set; }
    
    public decimal QuantityReceived { get; set; }
    
    public decimal QuantityRemaining { get; set; }
    
    public bool IsFullyReceived { get; set; }
    
    public string? Notes { get; set; }
}

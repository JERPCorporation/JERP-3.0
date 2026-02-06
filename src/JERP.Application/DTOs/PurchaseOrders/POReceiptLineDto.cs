/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Purchase order receipt line item DTO
/// </summary>
public class POReceiptLineDto
{
    public Guid Id { get; set; }
    
    public Guid PurchaseOrderLineId { get; set; }
    
    public Guid ProductId { get; set; }
    
    public required string ItemCode { get; set; }
    
    public required string ItemName { get; set; }
    
    public decimal QuantityOrdered { get; set; }
    
    public decimal QuantityReceived { get; set; }
    
    public decimal QuantityPreviouslyReceived { get; set; }
    
    public required string UnitOfMeasure { get; set; }
    
    public decimal UnitCost { get; set; }
    
    public Guid? BatchId { get; set; }
    
    public string? BatchNumber { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public string? Notes { get; set; }
}

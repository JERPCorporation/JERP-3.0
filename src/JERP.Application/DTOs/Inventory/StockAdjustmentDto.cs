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

using System;

namespace JERP.Application.DTOs.Inventory;

public class StockAdjustmentDto
{
    public Guid Id { get; set; }
    
    public string AdjustmentNumber { get; set; } = string.Empty;
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemName { get; set; } = string.Empty;
    
    public Guid? BatchLotId { get; set; }
    
    public string? BatchNumber { get; set; }
    
    public decimal QuantityBefore { get; set; }
    
    public decimal AdjustmentQuantity { get; set; }
    
    public decimal QuantityAfter { get; set; }
    
    public string AdjustmentType { get; set; } = string.Empty; // Increase, Decrease, Recount
    
    public string Reason { get; set; } = string.Empty; // Damaged, Expired, Lost, Found, Recount
    
    public DateTime AdjustmentDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public string? BinLocation { get; set; }
    
    public string? Notes { get; set; }
    
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    public string Status { get; set; } = string.Empty; // Draft, Approved, Posted
    
    public string? CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

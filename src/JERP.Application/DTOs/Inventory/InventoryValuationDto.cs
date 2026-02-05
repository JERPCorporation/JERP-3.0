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

public class InventoryValuationDto
{
    public Guid InventoryItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public decimal QuantityOnHand { get; set; }
    
    public decimal AverageCost { get; set; }
    
    public decimal TotalValue { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public Guid? WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public DateTime ValuationDate { get; set; }
}

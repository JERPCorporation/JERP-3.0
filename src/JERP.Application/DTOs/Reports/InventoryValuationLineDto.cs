/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class InventoryValuationLineDto
{
    public Guid ItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public string ItemType { get; set; } = string.Empty;
    
    public string WarehouseName { get; set; } = string.Empty;
    
    public decimal QuantityOnHand { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public decimal AverageCost { get; set; }
    
    public decimal TotalValue { get; set; }
    
    public DateTime? LastMovementDate { get; set; }
    
    public int DaysSinceLastMovement { get; set; }
}

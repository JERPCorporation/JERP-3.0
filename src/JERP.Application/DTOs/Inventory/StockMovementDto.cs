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

using System;

namespace JERP.Application.DTOs.Inventory;

public class StockMovementDto
{
    public Guid Id { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemName { get; set; } = string.Empty;
    
    public string MovementType { get; set; } = string.Empty; // Receipt, Issue, Transfer, Adjustment, Return
    
    public decimal Quantity { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public decimal? UnitCost { get; set; }
    
    public Guid? BatchLotId { get; set; }
    
    public string? BatchNumber { get; set; }
    
    public Guid? FromWarehouseId { get; set; }
    
    public string? FromWarehouseName { get; set; }
    
    public string? FromBinLocation { get; set; }
    
    public Guid? ToWarehouseId { get; set; }
    
    public string? ToWarehouseName { get; set; }
    
    public string? ToBinLocation { get; set; }
    
    public string? ReferenceType { get; set; } // PurchaseOrder, SalesOrder, WorkOrder, Adjustment
    
    public Guid? ReferenceId { get; set; }
    
    public string? ReferenceNumber { get; set; }
    
    public DateTime TransactionDate { get; set; }
    
    public string? Reason { get; set; }
    
    public string? Notes { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

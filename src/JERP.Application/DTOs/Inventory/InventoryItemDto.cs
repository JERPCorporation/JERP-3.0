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
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.Inventory;

/// <summary>
/// Inventory item data transfer object
/// </summary>
public class InventoryItemDto
{
    public Guid Id { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string ItemType { get; set; } = string.Empty; // Product, RawMaterial, FinishedGood, Packaging
    
    public string? SKU { get; set; }
    
    public string? Barcode { get; set; }
    
    public string? Category { get; set; }
    
    public decimal UnitCost { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal QuantityOnHand { get; set; }
    
    public decimal QuantityAvailable { get; set; }
    
    public decimal QuantityReserved { get; set; }
    
    public decimal ReorderLevel { get; set; }
    
    public decimal ReorderQuantity { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty; // Each, Pound, Gram, Kilogram, Ounce
    
    public bool IsActive { get; set; }
    
    public bool IsSerialized { get; set; }
    
    public bool IsBatchTracked { get; set; }
    
    // Cannabis-specific fields
    public bool IsCannabisProduct { get; set; }
    
    public decimal? THCPercentage { get; set; }
    
    public decimal? CBDPercentage { get; set; }
    
    public string? StrainName { get; set; }
    
    public string? ProductForm { get; set; } // Flower, Concentrate, Edible, Topical, Pre-Roll
    
    // Warehouse/Location
    public Guid? WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public string? DefaultBinLocation { get; set; }
    
    // Accounting
    public Guid? InventoryAccountId { get; set; }
    
    public Guid? COGSAccountId { get; set; }
    
    public Guid? SalesAccountId { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public string? Notes { get; set; }
}

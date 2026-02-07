/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.Inventory;

public class CreateInventoryItemRequest
{
    [Required]
    [MaxLength(50)]
    public string ItemCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public string ItemType { get; set; } = string.Empty; // Product, RawMaterial, FinishedGood, Packaging
    
    [MaxLength(100)]
    public string? SKU { get; set; }
    
    [MaxLength(100)]
    public string? Barcode { get; set; }
    
    [MaxLength(100)]
    public string? Category { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal UnitCost { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal ReorderLevel { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal ReorderQuantity { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public bool IsSerialized { get; set; }
    
    public bool IsBatchTracked { get; set; }
    
    // Cannabis-specific
    public bool IsCannabisProduct { get; set; }
    
    public decimal? THCPercentage { get; set; }
    
    public decimal? CBDPercentage { get; set; }
    
    [MaxLength(100)]
    public string? StrainName { get; set; }
    
    [MaxLength(50)]
    public string? ProductForm { get; set; }
    
    // Location
    public Guid? WarehouseId { get; set; }
    
    [MaxLength(50)]
    public string? DefaultBinLocation { get; set; }
    
    // Accounting
    public Guid? InventoryAccountId { get; set; }
    
    public Guid? COGSAccountId { get; set; }
    
    public Guid? SalesAccountId { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
}

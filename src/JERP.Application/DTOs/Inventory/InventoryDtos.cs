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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Inventory;

/// <summary>
/// DTO for Product entity
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Brand { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    
    // Cannabis attributes
    public bool IsCannabisProduct { get; set; }
    public decimal? THCPercentage { get; set; }
    public decimal? CBDPercentage { get; set; }
    public string? StrainType { get; set; }
    public bool RequiresBatchTracking { get; set; }
    public bool RequiresTestingCertificate { get; set; }
    
    // Pricing
    public decimal StandardCost { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public decimal GrossMargin { get; set; }
    
    // Inventory settings
    public int ReorderPoint { get; set; }
    public int ReorderQuantity { get; set; }
    public int SafetyStock { get; set; }
    
    // Valuation
    public InventoryValuationMethod ValuationMethod { get; set; }
    
    // Status
    public bool IsActive { get; set; }
    public bool IsDiscontinued { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO for creating a product
/// </summary>
public class CreateProductDto
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? Brand { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    
    public bool IsCannabisProduct { get; set; }
    public decimal? THCPercentage { get; set; }
    public decimal? CBDPercentage { get; set; }
    public string? StrainType { get; set; }
    public bool RequiresBatchTracking { get; set; }
    public bool RequiresTestingCertificate { get; set; }
    
    public decimal StandardCost { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    
    public int ReorderPoint { get; set; }
    public int ReorderQuantity { get; set; }
    public int SafetyStock { get; set; }
    
    public Guid InventoryAssetAccountId { get; set; }
    public Guid COGSAccountId { get; set; }
    public Guid RevenueAccountId { get; set; }
    
    public InventoryValuationMethod ValuationMethod { get; set; }
    public Guid? DefaultWarehouseId { get; set; }
}

/// <summary>
/// DTO for warehouse entity
/// </summary>
public class WarehouseDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? WarehouseType { get; set; }
    public bool IsActive { get; set; }
    public bool IsSecureVault { get; set; }
    public bool IsClimateControlled { get; set; }
    public string? CannabisLicense { get; set; }
}

/// <summary>
/// DTO for inventory level
/// </summary>
public class InventoryLevelDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityOnOrder { get; set; }
    public decimal TotalValue { get; set; }
    public decimal AverageCost { get; set; }
    public DateTime LastStockDate { get; set; }
    public DateTime LastCountDate { get; set; }
}

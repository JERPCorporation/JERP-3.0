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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a product in the inventory system
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Product SKU (auto-generated: PRD-0001)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;
    
    /// <summary>
    /// Product name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Product description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Foreign key to product category
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Brand name
    /// </summary>
    [MaxLength(100)]
    public string? Brand { get; set; }
    
    // Unit of Measure
    /// <summary>
    /// Unit of measure (1oz, 1g, each, etc.)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    /// <summary>
    /// Barcode
    /// </summary>
    [MaxLength(50)]
    public string? Barcode { get; set; }
    
    // Cannabis-Specific Attributes
    /// <summary>
    /// Whether this is a cannabis product
    /// </summary>
    public bool IsCannabisProduct { get; set; }
    
    /// <summary>
    /// THC percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? THCPercentage { get; set; }
    
    /// <summary>
    /// CBD percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? CBDPercentage { get; set; }
    
    /// <summary>
    /// Strain type (Sativa, Indica, Hybrid)
    /// </summary>
    [MaxLength(50)]
    public string? StrainType { get; set; }
    
    /// <summary>
    /// Whether batch tracking is required
    /// </summary>
    public bool RequiresBatchTracking { get; set; }
    
    /// <summary>
    /// Whether testing certificate is required
    /// </summary>
    public bool RequiresTestingCertificate { get; set; }
    
    /// <summary>
    /// Whether cannabis license is required
    /// </summary>
    public bool RequiresLicense { get; set; }
    
    /// <summary>
    /// Cannabis license number
    /// </summary>
    [MaxLength(50)]
    public string? CannabisLicense { get; set; }
    
    // Expiration Tracking
    /// <summary>
    /// Whether expiration is tracked
    /// </summary>
    public bool TracksExpiration { get; set; }
    
    /// <summary>
    /// Shelf life in days
    /// </summary>
    public int? ShelfLifeDays { get; set; }
    
    // Pricing & Costing
    /// <summary>
    /// Standard cost (used for valuation)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal StandardCost { get; set; }
    
    /// <summary>
    /// Retail price
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal RetailPrice { get; set; }
    
    /// <summary>
    /// Wholesale price
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? WholesalePrice { get; set; }
    
    /// <summary>
    /// Gross margin percentage
    /// </summary>
    [NotMapped]
    public decimal GrossMargin => RetailPrice > 0 ? ((RetailPrice - StandardCost) / RetailPrice) * 100 : 0;
    
    // Inventory Settings
    /// <summary>
    /// Reorder point
    /// </summary>
    public int ReorderPoint { get; set; }
    
    /// <summary>
    /// Reorder quantity
    /// </summary>
    public int ReorderQuantity { get; set; }
    
    /// <summary>
    /// Lead time in days
    /// </summary>
    public int LeadTimeDays { get; set; }
    
    /// <summary>
    /// Safety stock level
    /// </summary>
    public int SafetyStock { get; set; }
    
    /// <summary>
    /// Minimum order quantity
    /// </summary>
    public int MinOrderQuantity { get; set; }
    
    // Accounting Integration
    /// <summary>
    /// Inventory asset account (1300)
    /// </summary>
    [Required]
    public Guid InventoryAssetAccountId { get; set; }
    
    /// <summary>
    /// COGS account (5000)
    /// </summary>
    [Required]
    public Guid COGSAccountId { get; set; }
    
    /// <summary>
    /// Revenue account (4000)
    /// </summary>
    [Required]
    public Guid RevenueAccountId { get; set; }
    
    /// <summary>
    /// Whether COGS is 280E deductible
    /// </summary>
    public bool Is280EDeductible { get; set; }
    
    // Valuation Method
    /// <summary>
    /// Inventory valuation method
    /// </summary>
    public InventoryValuationMethod ValuationMethod { get; set; }
    
    // Storage
    /// <summary>
    /// Default warehouse
    /// </summary>
    public Guid? DefaultWarehouseId { get; set; }
    
    /// <summary>
    /// Storage conditions (Climate Controlled, Refrigerated, etc.)
    /// </summary>
    [MaxLength(100)]
    public string? StorageConditions { get; set; }
    
    // Images
    /// <summary>
    /// Product image URL
    /// </summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    
    // Status
    /// <summary>
    /// Whether product is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Whether product is discontinued
    /// </summary>
    public bool IsDiscontinued { get; set; }
    
    // Navigation Properties
    public Company Company { get; set; } = null!;
    public ProductCategory Category { get; set; } = null!;
    public Account InventoryAssetAccount { get; set; } = null!;
    public Account COGSAccount { get; set; } = null!;
    public Account RevenueAccount { get; set; } = null!;
    public Warehouse? DefaultWarehouse { get; set; }
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
    public ICollection<ProductBatch> Batches { get; set; } = new List<ProductBatch>();
}

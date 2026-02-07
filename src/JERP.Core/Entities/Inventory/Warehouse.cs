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

using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a warehouse/storage location
/// </summary>
public class Warehouse : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Warehouse code (WH-MAIN, WH-VAULT, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Warehouse name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    
    // Address
    /// <summary>
    /// Street address
    /// </summary>
    [MaxLength(200)]
    public string? Street { get; set; }
    
    /// <summary>
    /// City
    /// </summary>
    [MaxLength(100)]
    public string? City { get; set; }
    
    /// <summary>
    /// State (2-letter code)
    /// </summary>
    [MaxLength(2)]
    public string? State { get; set; }
    
    /// <summary>
    /// ZIP code
    /// </summary>
    [MaxLength(10)]
    public string? Zip { get; set; }
    
    // Warehouse Type
    /// <summary>
    /// Warehouse type (Vault, Retail Display, Storage, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? WarehouseType { get; set; }
    
    // Capacity
    /// <summary>
    /// Storage capacity
    /// </summary>
    public int Capacity { get; set; }
    
    /// <summary>
    /// Capacity unit (units, sq ft, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? CapacityUnit { get; set; }
    
    // Security & Compliance
    /// <summary>
    /// Whether this is a secure vault
    /// </summary>
    public bool IsSecureVault { get; set; }
    
    /// <summary>
    /// Whether warehouse is climate controlled
    /// </summary>
    public bool IsClimateControlled { get; set; }
    
    /// <summary>
    /// Whether warehouse has 24-hour security
    /// </summary>
    public bool Has24HourSecurity { get; set; }
    
    /// <summary>
    /// Whether warehouse has access control
    /// </summary>
    public bool HasAccessControl { get; set; }
    
    /// <summary>
    /// Cannabis license number
    /// </summary>
    [MaxLength(50)]
    public string? CannabisLicense { get; set; }
    
    // Status
    /// <summary>
    /// Whether warehouse is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    // Navigation
    public Company Company { get; set; } = null!;
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
}

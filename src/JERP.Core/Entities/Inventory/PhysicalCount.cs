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
using System.ComponentModel.DataAnnotations.Schema;
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a physical inventory count
/// </summary>
public class PhysicalCount : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse
    /// </summary>
    [Required]
    public Guid WarehouseId { get; set; }
    
    /// <summary>
    /// Count number (auto-generated: CNT-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CountNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Count date
    /// </summary>
    public DateTime CountDate { get; set; }
    
    /// <summary>
    /// Count type (Full/Cycle/Spot)
    /// </summary>
    public CountType Type { get; set; }
    
    /// <summary>
    /// Counted by user ID
    /// </summary>
    [Required]
    public Guid CountedByUserId { get; set; }
    
    /// <summary>
    /// Verified by user ID
    /// </summary>
    public Guid? VerifiedByUserId { get; set; }
    
    /// <summary>
    /// Count status
    /// </summary>
    public PhysicalCountStatus Status { get; set; }
    
    /// <summary>
    /// Total items counted
    /// </summary>
    public int TotalItemsCounted { get; set; }
    
    /// <summary>
    /// Items with variance
    /// </summary>
    public int ItemsWithVariance { get; set; }
    
    /// <summary>
    /// Total variance value
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalVarianceValue { get; set; }
    
    /// <summary>
    /// Accuracy rate percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal AccuracyRate { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Associated adjustment (if variances posted)
    /// </summary>
    public Guid? AdjustmentId { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public InventoryAdjustment? Adjustment { get; set; }
    public ICollection<PhysicalCountLine> Lines { get; set; } = new List<PhysicalCountLine>();
}

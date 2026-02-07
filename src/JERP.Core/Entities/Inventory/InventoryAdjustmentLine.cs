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

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a line item in an inventory adjustment
/// </summary>
public class InventoryAdjustmentLine : BaseEntity
{
    /// <summary>
    /// Foreign key to adjustment
    /// </summary>
    [Required]
    public Guid AdjustmentId { get; set; }
    
    /// <summary>
    /// Foreign key to product
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Foreign key to batch (optional)
    /// </summary>
    public Guid? BatchId { get; set; }
    
    /// <summary>
    /// Quantity before adjustment
    /// </summary>
    public int QuantityBefore { get; set; }
    
    /// <summary>
    /// Quantity adjustment (can be negative)
    /// </summary>
    public int QuantityAdjustment { get; set; }
    
    /// <summary>
    /// Quantity after adjustment
    /// </summary>
    [NotMapped]
    public int QuantityAfter => QuantityBefore + QuantityAdjustment;
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Adjustment value
    /// </summary>
    [NotMapped]
    public decimal AdjustmentValue => QuantityAdjustment * UnitCost;
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation
    public InventoryAdjustment Adjustment { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductBatch? Batch { get; set; }
}

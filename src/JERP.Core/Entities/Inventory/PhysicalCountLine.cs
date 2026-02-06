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
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a line item in a physical count
/// </summary>
public class PhysicalCountLine : BaseEntity
{
    /// <summary>
    /// Foreign key to physical count
    /// </summary>
    [Required]
    public Guid CountId { get; set; }
    
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
    /// System quantity
    /// </summary>
    public int SystemQuantity { get; set; }
    
    /// <summary>
    /// Counted quantity
    /// </summary>
    public int CountedQuantity { get; set; }
    
    /// <summary>
    /// Variance
    /// </summary>
    [NotMapped]
    public int Variance => CountedQuantity - SystemQuantity;
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Variance value
    /// </summary>
    [NotMapped]
    public decimal VarianceValue => Variance * UnitCost;
    
    /// <summary>
    /// Variance reason
    /// </summary>
    public VarianceReason? Reason { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation
    public PhysicalCount Count { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductBatch? Batch { get; set; }
}

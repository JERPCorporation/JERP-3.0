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

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents inventory level for a product at a warehouse
/// </summary>
public class InventoryLevel : BaseEntity
{
    /// <summary>
    /// Foreign key to product
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse
    /// </summary>
    [Required]
    public Guid WarehouseId { get; set; }
    
    /// <summary>
    /// Quantity on hand
    /// </summary>
    public int QuantityOnHand { get; set; }
    
    /// <summary>
    /// Quantity reserved (for orders)
    /// </summary>
    public int QuantityReserved { get; set; }
    
    /// <summary>
    /// Quantity available
    /// </summary>
    [NotMapped]
    public int QuantityAvailable => QuantityOnHand - QuantityReserved;
    
    /// <summary>
    /// Quantity on order (from POs)
    /// </summary>
    public int QuantityOnOrder { get; set; }
    
    /// <summary>
    /// Total value of inventory
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalValue { get; set; }
    
    /// <summary>
    /// Average cost per unit
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageCost { get; set; }
    
    /// <summary>
    /// Last stock date
    /// </summary>
    public DateTime LastStockDate { get; set; }
    
    /// <summary>
    /// Last physical count date
    /// </summary>
    public DateTime LastCountDate { get; set; }
    
    // Navigation
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}

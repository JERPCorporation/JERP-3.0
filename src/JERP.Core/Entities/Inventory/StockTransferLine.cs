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
/// Represents a line item in a stock transfer
/// </summary>
public class StockTransferLine : BaseEntity
{
    /// <summary>
    /// Foreign key to transfer
    /// </summary>
    [Required]
    public Guid TransferId { get; set; }
    
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
    /// Quantity to transfer
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Line value
    /// </summary>
    [NotMapped]
    public decimal LineValue => Quantity * UnitCost;
    
    // Navigation
    public StockTransfer Transfer { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductBatch? Batch { get; set; }
}

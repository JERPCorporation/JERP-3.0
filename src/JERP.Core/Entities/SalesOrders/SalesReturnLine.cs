/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JERP.Core.Entities.Inventory;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a line item in a sales return
/// </summary>
public class SalesReturnLine : BaseEntity
{
    /// <summary>
    /// Foreign key to sales return
    /// </summary>
    [Required]
    public Guid SalesReturnId { get; set; }
    
    /// <summary>
    /// Foreign key to sales order line (if applicable)
    /// </summary>
    public Guid? SalesOrderLineId { get; set; }
    
    /// <summary>
    /// Foreign key to inventory item
    /// </summary>
    [Required]
    public Guid InventoryItemId { get; set; }
    
    /// <summary>
    /// Item description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Quantity returned
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Unit price
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Tax amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Line total
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; }
    
    /// <summary>
    /// Restocking fee
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal RestockingFee { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public SalesReturn SalesReturn { get; set; } = null!;
    public SalesOrderLine? SalesOrderLine { get; set; }
    public Product InventoryItem { get; set; } = null!;
}

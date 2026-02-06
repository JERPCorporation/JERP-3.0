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
using JERP.Core.Entities.Inventory;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a line item in a sales order
/// </summary>
public class SalesOrderLine : BaseEntity
{
    /// <summary>
    /// Foreign key to sales order
    /// </summary>
    [Required]
    public Guid SalesOrderId { get; set; }
    
    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; set; }
    
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
    /// Quantity ordered
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Unit price
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Discount percent
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercent { get; set; }
    
    /// <summary>
    /// Discount amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }
    
    /// <summary>
    /// Tax percent
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxPercent { get; set; }
    
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
    /// Quantity shipped
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityShipped { get; set; }
    
    /// <summary>
    /// Quantity invoiced
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityInvoiced { get; set; }
    
    /// <summary>
    /// Revenue account for this line
    /// </summary>
    public Guid? RevenueAccountId { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public SalesOrder SalesOrder { get; set; } = null!;
    public Product InventoryItem { get; set; } = null!;
    public Account? RevenueAccount { get; set; }
}

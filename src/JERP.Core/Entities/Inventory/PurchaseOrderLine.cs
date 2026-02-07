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
/// Represents a line item in a purchase order
/// </summary>
public class PurchaseOrderLine : BaseEntity
{
    /// <summary>
    /// Foreign key to purchase order
    /// </summary>
    [Required]
    public Guid PurchaseOrderId { get; set; }
    
    /// <summary>
    /// Foreign key to product
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; set; }
    
    /// <summary>
    /// Quantity ordered
    /// </summary>
    public int QuantityOrdered { get; set; }
    
    /// <summary>
    /// Quantity received
    /// </summary>
    public int QuantityReceived { get; set; }
    
    /// <summary>
    /// Quantity remaining to receive
    /// </summary>
    [NotMapped]
    public int QuantityRemaining => QuantityOrdered - QuantityReceived;
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Line total
    /// </summary>
    [NotMapped]
    public decimal LineTotal => QuantityOrdered * UnitCost;
    
    /// <summary>
    /// Expected expiration date
    /// </summary>
    public DateTime? ExpectedExpirationDate { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

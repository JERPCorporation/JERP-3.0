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
using JERP.Core.Entities.Inventory;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a line item in a shipment
/// </summary>
public class SOShipmentLine : BaseEntity
{
    /// <summary>
    /// Foreign key to shipment
    /// </summary>
    [Required]
    public Guid ShipmentId { get; set; }
    
    /// <summary>
    /// Foreign key to sales order line
    /// </summary>
    [Required]
    public Guid SalesOrderLineId { get; set; }
    
    /// <summary>
    /// Foreign key to inventory item
    /// </summary>
    [Required]
    public Guid InventoryItemId { get; set; }
    
    /// <summary>
    /// Quantity shipped in this shipment
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityShipped { get; set; }
    
    /// <summary>
    /// Foreign key to batch/lot
    /// </summary>
    public Guid? BatchLotId { get; set; }
    
    /// <summary>
    /// Serial number
    /// </summary>
    [MaxLength(50)]
    public string? SerialNumber { get; set; }
    
    /// <summary>
    /// Bin location
    /// </summary>
    [MaxLength(50)]
    public string? BinLocation { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public SOShipment Shipment { get; set; } = null!;
    public SalesOrderLine SalesOrderLine { get; set; } = null!;
    public Product InventoryItem { get; set; } = null!;
    public ProductBatch? BatchLot { get; set; }
}

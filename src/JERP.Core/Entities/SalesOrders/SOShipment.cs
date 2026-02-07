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
using JERP.Core.Entities.Finance;
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a shipment for a sales order
/// </summary>
public class SOShipment : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Shipment number (auto-generated: SHIP-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ShipmentNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Foreign key to sales order
    /// </summary>
    [Required]
    public Guid SalesOrderId { get; set; }
    
    /// <summary>
    /// Foreign key to customer
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Ship date
    /// </summary>
    public DateTime ShipDate { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse
    /// </summary>
    public Guid? WarehouseId { get; set; }
    
    /// <summary>
    /// Shipping method
    /// </summary>
    [MaxLength(100)]
    public string? ShippingMethod { get; set; }
    
    /// <summary>
    /// Tracking number
    /// </summary>
    [MaxLength(100)]
    public string? TrackingNumber { get; set; }
    
    /// <summary>
    /// Carrier
    /// </summary>
    [MaxLength(100)]
    public string? Carrier { get; set; }
    
    /// <summary>
    /// Shipping cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ShippingCost { get; set; }
    
    /// <summary>
    /// Shipment status
    /// </summary>
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Draft;
    
    // Cannabis Tracking
    [MaxLength(100)]
    public string? MetrcManifestNumber { get; set; }
    
    public DateTime? MetrcManifestDate { get; set; }
    
    [MaxLength(100)]
    public string? PackedBy { get; set; }
    
    [MaxLength(100)]
    public string? ShippedBy { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public DateTime? ShippedAt { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public SalesOrder SalesOrder { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Warehouse? Warehouse { get; set; }
    public ICollection<SOShipmentLine> LineItems { get; set; } = new List<SOShipmentLine>();
}

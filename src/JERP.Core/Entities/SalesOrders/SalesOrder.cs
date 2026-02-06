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
using JERP.Core.Enums;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a sales order
/// </summary>
public class SalesOrder : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to the customer
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// SO number (auto-generated: SO-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string SONumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Order status
    /// </summary>
    public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Draft;
    
    /// <summary>
    /// Order date
    /// </summary>
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// Requested ship date
    /// </summary>
    public DateTime? RequestedShipDate { get; set; }
    
    /// <summary>
    /// Promised ship date
    /// </summary>
    public DateTime? PromisedShipDate { get; set; }
    
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
    /// Shipping terms
    /// </summary>
    [MaxLength(50)]
    public string? ShippingTerms { get; set; }
    
    /// <summary>
    /// Payment terms
    /// </summary>
    [MaxLength(50)]
    public string? PaymentTerms { get; set; }
    
    // Shipping Address
    [MaxLength(200)]
    public string? ShipToAddressLine1 { get; set; }
    
    [MaxLength(200)]
    public string? ShipToAddressLine2 { get; set; }
    
    [MaxLength(100)]
    public string? ShipToCity { get; set; }
    
    [MaxLength(50)]
    public string? ShipToState { get; set; }
    
    [MaxLength(20)]
    public string? ShipToPostalCode { get; set; }
    
    [MaxLength(100)]
    public string? ShipToCountry { get; set; }
    
    // Amounts
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    // Fulfillment Tracking
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalShipped { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalInvoiced { get; set; }
    
    public bool IsFullyShipped { get; set; }
    
    public bool IsFullyInvoiced { get; set; }
    
    // Approval
    [MaxLength(100)]
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    // Sales Rep
    public Guid? SalesRepId { get; set; }
    
    // References
    public Guid? SalesQuoteId { get; set; }
    
    [MaxLength(50)]
    public string? SalesQuoteNumber { get; set; }
    
    [MaxLength(50)]
    public string? CustomerPONumber { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
    
    [MaxLength(2000)]
    public string? InternalNotes { get; set; }
    
    // Cannabis Tracking
    public bool RequiresMetrcTracking { get; set; }
    
    [MaxLength(100)]
    public string? MetrcManifestNumber { get; set; }
    
    [MaxLength(100)]
    public string? CreatedBy { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Warehouse? Warehouse { get; set; }
    public ICollection<SalesOrderLine> LineItems { get; set; } = new List<SalesOrderLine>();
    public ICollection<SOShipment> Shipments { get; set; } = new List<SOShipment>();
}

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
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a purchase order
/// </summary>
public class PurchaseOrder : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to vendor
    /// </summary>
    [Required]
    public Guid VendorId { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse (ship to)
    /// </summary>
    [Required]
    public Guid WarehouseId { get; set; }
    
    /// <summary>
    /// PO number (auto-generated: PO-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PONumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Order date
    /// </summary>
    public DateTime OrderDate { get; set; }
    
    /// <summary>
    /// Expected delivery date
    /// </summary>
    public DateTime ExpectedDate { get; set; }
    
    /// <summary>
    /// Received date
    /// </summary>
    public DateTime? ReceivedDate { get; set; }
    
    /// <summary>
    /// Subtotal amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
    
    /// <summary>
    /// Tax amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Shipping amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }
    
    /// <summary>
    /// Total amount
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Purchase order status
    /// </summary>
    public PurchaseOrderStatus Status { get; set; }
    
    /// <summary>
    /// Approved date
    /// </summary>
    public DateTime? ApprovedDate { get; set; }
    
    /// <summary>
    /// Approved by user ID
    /// </summary>
    public Guid? ApprovedByUserId { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Vendor's PO number
    /// </summary>
    [MaxLength(100)]
    public string? VendorPONumber { get; set; }
    
    /// <summary>
    /// Associated vendor bill (created when fully received)
    /// </summary>
    public Guid? VendorBillId { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public Vendor Vendor { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public VendorBill? VendorBill { get; set; }
    public ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    public ICollection<StockReceipt> Receipts { get; set; } = new List<StockReceipt>();
}

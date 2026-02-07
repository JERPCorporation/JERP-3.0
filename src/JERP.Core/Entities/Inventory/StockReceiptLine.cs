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
/// Represents a line item in a stock receipt
/// </summary>
public class StockReceiptLine : BaseEntity
{
    /// <summary>
    /// Foreign key to stock receipt
    /// </summary>
    [Required]
    public Guid ReceiptId { get; set; }
    
    /// <summary>
    /// Foreign key to purchase order line
    /// </summary>
    [Required]
    public Guid POLineId { get; set; }
    
    /// <summary>
    /// Foreign key to product
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Quantity received
    /// </summary>
    public int QuantityReceived { get; set; }
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    // Batch Information
    /// <summary>
    /// Batch number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Expiration date
    /// </summary>
    public DateTime? ExpirationDate { get; set; }
    
    // Cannabis Testing
    /// <summary>
    /// Actual THC percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? ActualTHC { get; set; }
    
    /// <summary>
    /// Actual CBD percentage
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? ActualCBD { get; set; }
    
    /// <summary>
    /// Whether testing passed
    /// </summary>
    public bool? TestingPassed { get; set; }
    
    /// <summary>
    /// Test certificate URL
    /// </summary>
    [MaxLength(500)]
    public string? TestCertificateUrl { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    // Navigation
    public StockReceipt Receipt { get; set; } = null!;
    public PurchaseOrderLine POLine { get; set; } = null!;
    public Product Product { get; set; } = null!;
}

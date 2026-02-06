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
/// Represents a batch/lot of product with cannabis tracking
/// </summary>
public class ProductBatch : BaseEntity
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
    /// Batch number (BTH-2025-001)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Date received
    /// </summary>
    public DateTime ReceivedDate { get; set; }
    
    /// <summary>
    /// Expiration date
    /// </summary>
    public DateTime? ExpirationDate { get; set; }
    
    /// <summary>
    /// Testing date
    /// </summary>
    public DateTime? TestingDate { get; set; }
    
    /// <summary>
    /// Original quantity
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Remaining quantity
    /// </summary>
    public int RemainingQuantity { get; set; }
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
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
    public bool TestingPassed { get; set; }
    
    /// <summary>
    /// Testing lab name
    /// </summary>
    [MaxLength(100)]
    public string? TestingLab { get; set; }
    
    /// <summary>
    /// Test certificate URL
    /// </summary>
    [MaxLength(500)]
    public string? TestCertificateUrl { get; set; }
    
    // Metrc/Track & Trace
    /// <summary>
    /// Metrc UID for cannabis tracking
    /// </summary>
    [MaxLength(50)]
    public string? MetrcUID { get; set; }
    
    /// <summary>
    /// Source license
    /// </summary>
    [MaxLength(100)]
    public string? SourceLicense { get; set; }
    
    // Status
    /// <summary>
    /// Whether batch is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Whether batch is quarantined
    /// </summary>
    public bool IsQuarantined { get; set; }
    
    /// <summary>
    /// Quarantine reason
    /// </summary>
    [MaxLength(500)]
    public string? QuarantineReason { get; set; }
    
    // Navigation
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}

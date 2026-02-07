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
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents an inventory transaction for audit trail
/// </summary>
public class InventoryTransaction : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
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
    /// Foreign key to batch (optional)
    /// </summary>
    public Guid? BatchId { get; set; }
    
    /// <summary>
    /// Transaction date
    /// </summary>
    public DateTime TransactionDate { get; set; }
    
    /// <summary>
    /// Transaction type
    /// </summary>
    public InventoryTransactionType Type { get; set; }
    
    /// <summary>
    /// Quantity change (can be negative)
    /// </summary>
    public int QuantityChange { get; set; }
    
    /// <summary>
    /// Unit cost
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Total value
    /// </summary>
    [NotMapped]
    public decimal TotalValue => QuantityChange * UnitCost;
    
    /// <summary>
    /// Quantity after transaction
    /// </summary>
    public int QuantityAfter { get; set; }
    
    // Source Document References
    /// <summary>
    /// Source type (PO, Adjustment, Transfer, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? SourceType { get; set; }
    
    /// <summary>
    /// Source document ID
    /// </summary>
    public Guid? SourceId { get; set; }
    
    /// <summary>
    /// Source document number
    /// </summary>
    [MaxLength(50)]
    public string? SourceNumber { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Transacted by user ID
    /// </summary>
    [Required]
    public Guid TransactedByUserId { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public ProductBatch? Batch { get; set; }
}

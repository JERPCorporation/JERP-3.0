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
using JERP.Core.Enums;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents an inventory adjustment
/// </summary>
public class InventoryAdjustment : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse
    /// </summary>
    [Required]
    public Guid WarehouseId { get; set; }
    
    /// <summary>
    /// Adjustment number (auto-generated: ADJ-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AdjustmentNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Adjustment date
    /// </summary>
    public DateTime AdjustmentDate { get; set; }
    
    /// <summary>
    /// Adjusted by user ID
    /// </summary>
    [Required]
    public Guid AdjustedByUserId { get; set; }
    
    /// <summary>
    /// Adjustment type (Increase/Decrease)
    /// </summary>
    public AdjustmentType Type { get; set; }
    
    /// <summary>
    /// Adjustment reason
    /// </summary>
    public AdjustmentReason Reason { get; set; }
    
    /// <summary>
    /// Total value of adjustment
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalValue { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Adjustment status
    /// </summary>
    public AdjustmentStatus Status { get; set; }
    
    /// <summary>
    /// Approved date
    /// </summary>
    public DateTime? ApprovedDate { get; set; }
    
    /// <summary>
    /// Approved by user ID
    /// </summary>
    public Guid? ApprovedByUserId { get; set; }
    
    /// <summary>
    /// Associated journal entry
    /// </summary>
    public Guid? JournalEntryId { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<InventoryAdjustmentLine> Lines { get; set; } = new List<InventoryAdjustmentLine>();
}

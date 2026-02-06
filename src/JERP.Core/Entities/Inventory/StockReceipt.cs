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
using JERP.Core.Entities.Finance;

namespace JERP.Core.Entities.Inventory;

/// <summary>
/// Represents a stock receipt from a purchase order
/// </summary>
public class StockReceipt : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Foreign key to purchase order
    /// </summary>
    [Required]
    public Guid PurchaseOrderId { get; set; }
    
    /// <summary>
    /// Foreign key to warehouse
    /// </summary>
    [Required]
    public Guid WarehouseId { get; set; }
    
    /// <summary>
    /// Receipt number (auto-generated: RCV-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ReceiptNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Receipt date
    /// </summary>
    public DateTime ReceiptDate { get; set; }
    
    /// <summary>
    /// Received by user ID
    /// </summary>
    [Required]
    public Guid ReceivedByUserId { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Whether PO is fully received
    /// </summary>
    public bool IsComplete { get; set; }
    
    // Quality Control
    /// <summary>
    /// Whether QC passed
    /// </summary>
    public bool QCPassed { get; set; }
    
    /// <summary>
    /// QC notes
    /// </summary>
    [MaxLength(500)]
    public string? QCNotes { get; set; }
    
    /// <summary>
    /// Associated journal entry
    /// </summary>
    public Guid? JournalEntryId { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
    public JournalEntry? JournalEntry { get; set; }
    public ICollection<StockReceiptLine> Lines { get; set; } = new List<StockReceiptLine>();
}

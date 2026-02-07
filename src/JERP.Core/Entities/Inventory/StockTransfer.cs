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
/// Represents a stock transfer between warehouses
/// </summary>
public class StockTransfer : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Transfer number (auto-generated: TRF-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TransferNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Transfer date
    /// </summary>
    public DateTime TransferDate { get; set; }
    
    /// <summary>
    /// From warehouse
    /// </summary>
    [Required]
    public Guid FromWarehouseId { get; set; }
    
    /// <summary>
    /// To warehouse
    /// </summary>
    [Required]
    public Guid ToWarehouseId { get; set; }
    
    /// <summary>
    /// Transferred by user ID
    /// </summary>
    [Required]
    public Guid TransferredByUserId { get; set; }
    
    /// <summary>
    /// Received by user ID
    /// </summary>
    public Guid? ReceivedByUserId { get; set; }
    
    /// <summary>
    /// Transfer status
    /// </summary>
    public TransferStatus Status { get; set; }
    
    /// <summary>
    /// Total value
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalValue { get; set; }
    
    /// <summary>
    /// Reason for transfer
    /// </summary>
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation
    public Company Company { get; set; } = null!;
    public Warehouse FromWarehouse { get; set; } = null!;
    public Warehouse ToWarehouse { get; set; } = null!;
    public ICollection<StockTransferLine> Lines { get; set; } = new List<StockTransferLine>();
}

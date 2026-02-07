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

using System;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.Inventory;

public class CreateStockMovementRequest
{
    [Required]
    public Guid InventoryItemId { get; set; }
    
    [Required]
    public string MovementType { get; set; } = string.Empty; // Receipt, Issue, Transfer, Adjustment, Return
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }
    
    public decimal? UnitCost { get; set; }
    
    public Guid? BatchLotId { get; set; }
    
    public Guid? FromWarehouseId { get; set; }
    
    [MaxLength(50)]
    public string? FromBinLocation { get; set; }
    
    public Guid? ToWarehouseId { get; set; }
    
    [MaxLength(50)]
    public string? ToBinLocation { get; set; }
    
    [MaxLength(50)]
    public string? ReferenceType { get; set; }
    
    public Guid? ReferenceId { get; set; }
    
    [MaxLength(50)]
    public string? ReferenceNumber { get; set; }
    
    public DateTime? TransactionDate { get; set; }
    
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

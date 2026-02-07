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

public class CreateStockAdjustmentRequest
{
    [Required]
    public Guid InventoryItemId { get; set; }
    
    public Guid? BatchLotId { get; set; }
    
    [Required]
    public decimal AdjustmentQuantity { get; set; } // Can be positive or negative
    
    [Required]
    public string AdjustmentType { get; set; } = string.Empty; // Increase, Decrease, Recount
    
    [Required]
    [MaxLength(100)]
    public string Reason { get; set; } = string.Empty;
    
    public DateTime? AdjustmentDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    [MaxLength(50)]
    public string? BinLocation { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

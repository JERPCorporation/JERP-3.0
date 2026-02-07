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

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Create purchase order receipt line request DTO
/// </summary>
public class CreatePOReceiptLineRequest
{
    [Required]
    public Guid PurchaseOrderLineId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal QuantityReceived { get; set; }
    
    [MaxLength(50)]
    public string? BatchNumber { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}

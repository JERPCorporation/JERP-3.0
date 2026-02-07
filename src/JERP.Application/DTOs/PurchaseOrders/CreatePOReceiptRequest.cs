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
/// Create purchase order receipt request DTO
/// </summary>
public class CreatePOReceiptRequest
{
    [Required]
    public Guid PurchaseOrderId { get; set; }
    
    [Required]
    public DateTime ReceiptDate { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreatePOReceiptLineRequest> LineItems { get; set; } = new();
    
    public bool QCPassed { get; set; }
    
    [MaxLength(500)]
    public string? QCNotes { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

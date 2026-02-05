/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Purchase order receipt DTO
/// </summary>
public class POReceiptDto
{
    public Guid Id { get; set; }
    
    public required string ReceiptNumber { get; set; }
    
    public Guid PurchaseOrderId { get; set; }
    
    public required string PONumber { get; set; }
    
    public Guid VendorId { get; set; }
    
    public required string VendorName { get; set; }
    
    public DateTime ReceiptDate { get; set; }
    
    public Guid WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public bool IsComplete { get; set; }
    
    public bool QCPassed { get; set; }
    
    public string? QCNotes { get; set; }
    
    public List<POReceiptLineDto> LineItems { get; set; } = new();
    
    public string? ReceivedBy { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

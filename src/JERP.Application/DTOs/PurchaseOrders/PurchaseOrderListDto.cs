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
/// Purchase order list/summary DTO
/// </summary>
public class PurchaseOrderListDto
{
    public Guid Id { get; set; }
    
    public required string PONumber { get; set; }
    
    public Guid VendorId { get; set; }
    
    public required string VendorName { get; set; }
    
    public required string Status { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public DateTime ExpectedDeliveryDate { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public bool IsFullyReceived { get; set; }
}

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

using System;

namespace JERP.Application.DTOs.Inventory;

public class BatchLotDto
{
    public Guid Id { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemName { get; set; } = string.Empty;
    
    public string BatchNumber { get; set; } = string.Empty;
    
    public string? LotNumber { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal QuantityAvailable { get; set; }
    
    public DateTime? ManufactureDate { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public Guid? VendorId { get; set; }
    
    public string? VendorName { get; set; }
    
    public string? VendorBatchNumber { get; set; }
    
    public bool IsActive { get; set; }
    
    // Cannabis-specific compliance
    public string? MetrcTag { get; set; }
    
    public string? LabTestCertificate { get; set; }
    
    public DateTime? LabTestDate { get; set; }
    
    public decimal? TestedTHC { get; set; }
    
    public decimal? TestedCBD { get; set; }
    
    public bool IsQuarantined { get; set; }
    
    public string? QuarantineReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? Notes { get; set; }
}

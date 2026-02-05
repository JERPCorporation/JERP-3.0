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
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.Inventory;

public class CreateBatchLotRequest
{
    [Required]
    public Guid InventoryItemId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? LotNumber { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }
    
    public DateTime? ManufactureDate { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public Guid? VendorId { get; set; }
    
    [MaxLength(50)]
    public string? VendorBatchNumber { get; set; }
    
    // Cannabis-specific
    [MaxLength(100)]
    public string? MetrcTag { get; set; }
    
    [MaxLength(200)]
    public string? LabTestCertificate { get; set; }
    
    public DateTime? LabTestDate { get; set; }
    
    public decimal? TestedTHC { get; set; }
    
    public decimal? TestedCBD { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

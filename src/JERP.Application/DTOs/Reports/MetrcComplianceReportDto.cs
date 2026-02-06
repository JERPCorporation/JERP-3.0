/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class MetrcComplianceReportDto
{
    public Guid CompanyId { get; set; }
    
    public string LicenseNumber { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    // Inventory Tracking
    public List<MetrcInventoryLineDto> InventoryTransactions { get; set; } = new();
    
    public int TotalReceiptsRecorded { get; set; }
    
    public int TotalSalesRecorded { get; set; }
    
    public int TotalTransfersRecorded { get; set; }
    
    // Sales Tracking
    public decimal TotalSalesAmount { get; set; }
    
    public decimal TotalWeightSold { get; set; }
    
    // Compliance Issues
    public List<ComplianceIssueDto> Issues { get; set; } = new();
    
    public int TotalIssues { get; set; }
    
    public bool IsCompliant { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}

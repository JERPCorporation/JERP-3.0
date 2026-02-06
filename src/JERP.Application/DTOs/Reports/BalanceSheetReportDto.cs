/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class BalanceSheetReportDto
{
    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; } = string.Empty;
    
    public DateTime AsOfDate { get; set; }
    
    // Assets
    public List<ReportLineItemDto> CurrentAssets { get; set; } = new();
    
    public decimal TotalCurrentAssets { get; set; }
    
    public List<ReportLineItemDto> FixedAssets { get; set; } = new();
    
    public decimal TotalFixedAssets { get; set; }
    
    public List<ReportLineItemDto> OtherAssets { get; set; } = new();
    
    public decimal TotalOtherAssets { get; set; }
    
    public decimal TotalAssets { get; set; }
    
    // Liabilities
    public List<ReportLineItemDto> CurrentLiabilities { get; set; } = new();
    
    public decimal TotalCurrentLiabilities { get; set; }
    
    public List<ReportLineItemDto> LongTermLiabilities { get; set; } = new();
    
    public decimal TotalLongTermLiabilities { get; set; }
    
    public decimal TotalLiabilities { get; set; }
    
    // Equity
    public List<ReportLineItemDto> EquityItems { get; set; } = new();
    
    public decimal TotalEquity { get; set; }
    
    public decimal TotalLiabilitiesAndEquity { get; set; }
    
    public bool IsBalanced { get; set; }
    
    // Ratios
    public decimal CurrentRatio { get; set; }
    
    public decimal DebtToEquityRatio { get; set; }
    
    public decimal WorkingCapital { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}

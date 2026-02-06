/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class ReportRequestDto
{
    public string ReportType { get; set; } = string.Empty;
    
    public Guid CompanyId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string OutputFormat { get; set; } = "PDF"; // PDF, Excel, CSV, JSON
    
    public Dictionary<string, object>? Filters { get; set; }
    
    public string? GroupBy { get; set; }
    
    public string? SortBy { get; set; }
    
    public bool IncludeCharts { get; set; }
    
    public bool IncludeDetails { get; set; }
    
    public string? ComparisonPeriod { get; set; } // PriorYear, PriorQuarter, Budget
}

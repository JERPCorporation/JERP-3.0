/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class SalesReportLineDto
{
    public string GroupKey { get; set; } = string.Empty;
    
    public string GroupName { get; set; } = string.Empty;
    
    public decimal SalesAmount { get; set; }
    
    public decimal COGSAmount { get; set; }
    
    public decimal GrossProfit { get; set; }
    
    public decimal GrossProfitMargin { get; set; }
    
    public int OrderCount { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal AveragePrice { get; set; }
}

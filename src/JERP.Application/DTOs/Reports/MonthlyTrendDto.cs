/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class MonthlyTrendDto
{
    public int Year { get; set; }
    
    public int Month { get; set; }
    
    public string MonthName { get; set; } = string.Empty;
    
    public decimal Value { get; set; }
    
    public decimal? PriorYearValue { get; set; }
    
    public decimal? GrowthPercent { get; set; }
}

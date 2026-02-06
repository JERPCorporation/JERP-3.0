/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class SalesReportDto
{
    public Guid CompanyId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string GroupBy { get; set; } = string.Empty; // Customer, Product, SalesRep, Date
    
    public List<SalesReportLineDto> Lines { get; set; } = new();
    
    public decimal TotalSales { get; set; }
    
    public decimal TotalCOGS { get; set; }
    
    public decimal GrossProfit { get; set; }
    
    public decimal GrossProfitMargin { get; set; }
    
    public int TotalOrders { get; set; }
    
    public decimal AverageOrderValue { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}

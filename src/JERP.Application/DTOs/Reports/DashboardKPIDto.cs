/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class DashboardKPIDto
{
    public Guid CompanyId { get; set; }
    
    public DateTime AsOfDate { get; set; }
    
    // Financial KPIs
    public decimal TotalRevenueMTD { get; set; }
    
    public decimal TotalRevenueYTD { get; set; }
    
    public decimal GrossProfitMarginMTD { get; set; }
    
    public decimal NetProfitMarginMTD { get; set; }
    
    public decimal CashBalance { get; set; }
    
    public decimal AccountsReceivableBalance { get; set; }
    
    public decimal AccountsPayableBalance { get; set; }
    
    public decimal WorkingCapital { get; set; }
    
    // Inventory KPIs
    public decimal InventoryValue { get; set; }
    
    public int LowStockItemsCount { get; set; }
    
    public decimal InventoryTurnoverRatio { get; set; }
    
    public int DaysInventoryOnHand { get; set; }
    
    // Sales KPIs
    public decimal SalesOrdersPendingValue { get; set; }
    
    public int SalesOrdersPendingCount { get; set; }
    
    public decimal BackordersValue { get; set; }
    
    public int CustomersCount { get; set; }
    
    // Purchase KPIs
    public decimal PurchaseOrdersPendingValue { get; set; }
    
    public int PurchaseOrdersPendingCount { get; set; }
    
    public int VendorsCount { get; set; }
    
    // Payroll KPIs
    public decimal TotalPayrollCostMTD { get; set; }
    
    public int EmployeeCount { get; set; }
    
    // Trends (last 12 months)
    public List<MonthlyTrendDto> RevenueTrend { get; set; } = new();
    
    public List<MonthlyTrendDto> ProfitTrend { get; set; } = new();
    
    public DateTime GeneratedAt { get; set; }
}

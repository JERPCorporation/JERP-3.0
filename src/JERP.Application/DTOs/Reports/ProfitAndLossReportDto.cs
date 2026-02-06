/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class ProfitAndLossReportDto
{
    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string PeriodDescription { get; set; } = string.Empty;
    
    // Revenue Section
    public List<ReportLineItemDto> RevenueItems { get; set; } = new();
    
    public decimal TotalRevenue { get; set; }
    
    // COGS Section
    public List<ReportLineItemDto> COGSItems { get; set; } = new();
    
    public decimal TotalCOGS { get; set; }
    
    public decimal GrossProfit { get; set; }
    
    public decimal GrossProfitMargin { get; set; }
    
    // Operating Expenses
    public List<ReportLineItemDto> OperatingExpenseItems { get; set; } = new();
    
    public decimal TotalOperatingExpenses { get; set; }
    
    public decimal OperatingIncome { get; set; }
    
    // Other Income/Expense
    public List<ReportLineItemDto> OtherIncomeItems { get; set; } = new();
    
    public List<ReportLineItemDto> OtherExpenseItems { get; set; } = new();
    
    public decimal TotalOtherIncome { get; set; }
    
    public decimal TotalOtherExpense { get; set; }
    
    // Net Income
    public decimal NetIncomeBeforeTaxes { get; set; }
    
    public decimal TaxExpense { get; set; }
    
    public decimal NetIncome { get; set; }
    
    public decimal NetProfitMargin { get; set; }
    
    // Cannabis 280E
    public decimal NonDeductibleExpenses { get; set; }
    
    public decimal TaxableIncome280E { get; set; }
    
    // Comparison (optional)
    public ProfitAndLossReportDto? ComparisonReport { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}

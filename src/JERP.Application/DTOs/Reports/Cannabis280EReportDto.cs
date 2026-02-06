/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class Cannabis280EReportDto
{
    public Guid CompanyId { get; set; }
    
    public string TaxYear { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    // Revenue
    public decimal TotalRevenue { get; set; }
    
    public decimal CannabisSalesRevenue { get; set; }
    
    public decimal NonCannabisRevenue { get; set; }
    
    // COGS (Deductible)
    public List<ReportLineItemDto> COGSItems { get; set; } = new();
    
    public decimal TotalCOGS { get; set; }
    
    public decimal GrossProfit { get; set; }
    
    // Operating Expenses (NON-DEDUCTIBLE for cannabis)
    public List<ReportLineItemDto> NonDeductibleExpenses { get; set; } = new();
    
    public decimal TotalNonDeductibleExpenses { get; set; }
    
    // Deductible Expenses (if any non-cannabis operations)
    public List<ReportLineItemDto> DeductibleExpenses { get; set; } = new();
    
    public decimal TotalDeductibleExpenses { get; set; }
    
    // Tax Calculation
    public decimal TaxableIncomeBeforeAdjustment { get; set; }
    
    public decimal Section280EAdjustment { get; set; }
    
    public decimal TaxableIncome280E { get; set; }
    
    public decimal EstimatedFederalTax { get; set; }
    
    public decimal EffectiveTaxRate { get; set; }
    
    // Documentation
    public string? COGSMethodology { get; set; }
    
    public string? AllocationMethod { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}

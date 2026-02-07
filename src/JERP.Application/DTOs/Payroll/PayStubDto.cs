/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Pay stub data for PDF generation
/// </summary>
public class PayStubDto
{
    public Guid PayrollRecordId { get; set; }
    
    // Company Information
    public required string CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    
    // Employee Information
    public required string EmployeeName { get; set; }
    public required string EmployeeNumber { get; set; }
    public string? MaskedSSN { get; set; }
    public string? Department { get; set; }
    
    // Pay Period Information
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public DateTime PayDate { get; set; }
    
    // Earnings
    public List<LineItem> Earnings { get; set; } = new();
    
    // Taxes
    public List<LineItem> Taxes { get; set; } = new();
    
    // Deductions
    public List<LineItem> Deductions { get; set; } = new();
    
    // Summary
    public decimal GrossPay { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    
    // Year-to-Date
    public decimal YTDGrossPay { get; set; }
    public decimal YTDFederalTax { get; set; }
    public decimal YTDStateTax { get; set; }
    public decimal YTDSocialSecurity { get; set; }
    public decimal YTDMedicare { get; set; }
    public decimal YTDNetPay { get; set; }
}

/// <summary>
/// Line item for earnings, taxes, or deductions
/// </summary>
public class LineItem
{
    public required string Description { get; set; }
    public decimal? Hours { get; set; }
    public decimal? Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal? YTDAmount { get; set; }
}

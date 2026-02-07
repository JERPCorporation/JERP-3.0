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

namespace JERP.Application.Services.Payroll.Pdf.Models;

/// <summary>
/// Document model for pay stub PDF generation
/// </summary>
public class PayStubDocument
{
    public CompanyInfo Company { get; set; } = null!;
    public EmployeeInfo Employee { get; set; } = null!;
    public PayPeriodInfo PayPeriod { get; set; } = new();
    public List<EarningLine> Earnings { get; set; } = new();
    public List<TaxLine> Taxes { get; set; } = new();
    public List<DeductionLine> Deductions { get; set; } = new();
    public PaySummary Summary { get; set; } = new();
    public YTDSummary YTD { get; set; } = new();
}

/// <summary>
/// Company information
/// </summary>
public class CompanyInfo
{
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
}

/// <summary>
/// Employee information
/// </summary>
public class EmployeeInfo
{
    public required string Name { get; set; }
    public required string EmployeeNumber { get; set; }
    public string? MaskedSSN { get; set; }
    public string? Department { get; set; }
}

/// <summary>
/// Pay period information
/// </summary>
public class PayPeriodInfo
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayDate { get; set; }
}

/// <summary>
/// Earning line item
/// </summary>
public class EarningLine
{
    public required string Description { get; set; }
    public decimal? Hours { get; set; }
    public decimal? Rate { get; set; }
    public decimal Amount { get; set; }
}

/// <summary>
/// Tax line item
/// </summary>
public class TaxLine
{
    public required string Description { get; set; }
    public decimal Current { get; set; }
    public decimal YTD { get; set; }
}

/// <summary>
/// Deduction line item
/// </summary>
public class DeductionLine
{
    public required string Description { get; set; }
    public decimal Current { get; set; }
    public decimal YTD { get; set; }
}

/// <summary>
/// Pay summary
/// </summary>
public class PaySummary
{
    public decimal GrossPay { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
}

/// <summary>
/// Year-to-date summary
/// </summary>
public class YTDSummary
{
    public decimal GrossPay { get; set; }
    public decimal FederalTax { get; set; }
    public decimal StateTax { get; set; }
    public decimal SocialSecurity { get; set; }
    public decimal Medicare { get; set; }
    public decimal NetPay { get; set; }
}

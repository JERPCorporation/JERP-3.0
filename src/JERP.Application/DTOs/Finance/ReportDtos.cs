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

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Aging summary by buckets
/// </summary>
public class AgingSummaryDto
{
    public decimal Current { get; set; }
    public decimal Days1To30 { get; set; }
    public decimal Days31To60 { get; set; }
    public decimal Days61To90 { get; set; }
    public decimal Days90Plus { get; set; }
    public decimal TotalAmount => Current + Days1To30 + Days31To60 + Days61To90 + Days90Plus;
    
    public decimal CurrentPercent { get; set; }
    public decimal Days1To30Percent { get; set; }
    public decimal Days31To60Percent { get; set; }
    public decimal Days61To90Percent { get; set; }
    public decimal Days90PlusPercent { get; set; }
}

/// <summary>
/// Vendor aging details
/// </summary>
public class VendorAgingDto
{
    public Guid VendorId { get; set; }
    public required string VendorNumber { get; set; }
    public required string VendorName { get; set; }
    public decimal Current { get; set; }
    public decimal Days1To30 { get; set; }
    public decimal Days31To60 { get; set; }
    public decimal Days61To90 { get; set; }
    public decimal Days90Plus { get; set; }
    public decimal TotalAmount => Current + Days1To30 + Days31To60 + Days61To90 + Days90Plus;
}

/// <summary>
/// Customer aging details
/// </summary>
public class CustomerAgingDto
{
    public Guid CustomerId { get; set; }
    public required string CustomerNumber { get; set; }
    public required string CustomerName { get; set; }
    public decimal Current { get; set; }
    public decimal Days1To30 { get; set; }
    public decimal Days31To60 { get; set; }
    public decimal Days61To90 { get; set; }
    public decimal Days90Plus { get; set; }
    public decimal TotalAmount => Current + Days1To30 + Days31To60 + Days61To90 + Days90Plus;
}

/// <summary>
/// AP aging report DTO
/// </summary>
public class APAgingReportDto
{
    public DateTime AsOfDate { get; set; }
    public List<VendorAgingDto> Vendors { get; set; } = new();
    public AgingSummaryDto Summary { get; set; } = new();
}

/// <summary>
/// AR aging report DTO
/// </summary>
public class ARAgingReportDto
{
    public DateTime AsOfDate { get; set; }
    public List<CustomerAgingDto> Customers { get; set; } = new();
    public AgingSummaryDto Summary { get; set; } = new();
}

/// <summary>
/// Cash flow week projection
/// </summary>
public class CashFlowWeekDto
{
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public decimal ExpectedInflows { get; set; }
    public decimal ExpectedOutflows { get; set; }
    public decimal NetCashFlow => ExpectedInflows - ExpectedOutflows;
}

/// <summary>
/// Cash flow forecast DTO
/// </summary>
public class CashFlowForecastDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal BeginningCashBalance { get; set; }
    public List<CashFlowWeekDto> WeeklyProjections { get; set; } = new();
    public decimal TotalExpectedInflows { get; set; }
    public decimal TotalExpectedOutflows { get; set; }
    public decimal ProjectedEndingBalance { get; set; }
}

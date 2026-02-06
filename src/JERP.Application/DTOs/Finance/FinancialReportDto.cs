/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Profit and Loss (Income Statement) report
/// </summary>
public class ProfitAndLossReportDto
{
    public Guid CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<AccountSummaryDto> Revenue { get; set; } = new();
    public decimal TotalRevenue { get; set; }
    public List<AccountSummaryDto> CostOfGoodsSold { get; set; } = new();
    public decimal TotalCOGS { get; set; }
    public decimal GrossProfit { get; set; }
    public List<AccountSummaryDto> Expenses { get; set; } = new();
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    
    // Cannabis 280E specific
    public decimal Total280EDeductible { get; set; }
    public decimal Total280ENonDeductible { get; set; }
}

/// <summary>
/// Balance Sheet report
/// </summary>
public class BalanceSheetReportDto
{
    public Guid CompanyId { get; set; }
    public DateTime AsOfDate { get; set; }
    public List<AccountSummaryDto> Assets { get; set; } = new();
    public decimal TotalAssets { get; set; }
    public List<AccountSummaryDto> Liabilities { get; set; } = new();
    public decimal TotalLiabilities { get; set; }
    public List<AccountSummaryDto> Equity { get; set; } = new();
    public decimal TotalEquity { get; set; }
}

/// <summary>
/// Account summary for reports
/// </summary>
public class AccountSummaryDto
{
    public Guid AccountId { get; set; }
    public required string AccountNumber { get; set; }
    public required string AccountName { get; set; }
    public decimal Balance { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Service for generating financial reports
/// </summary>
public class FinancialReportService : IFinancialReportService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<FinancialReportService> _logger;

    public FinancialReportService(
        JerpDbContext context,
        ILogger<FinancialReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProfitAndLossReportDto> GenerateProfitAndLossAsync(ReportRequestDto request)
    {
        _logger.LogInformation("Generating P&L report for company {CompanyId}", request.CompanyId);

        var report = new ProfitAndLossReportDto
        {
            CompanyId = request.CompanyId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PeriodDescription = $"{request.StartDate:MMM dd, yyyy} - {request.EndDate:MMM dd, yyyy}",
            GeneratedAt = DateTime.UtcNow
        };

        // Get company name
        var company = await _context.Companies.FindAsync(request.CompanyId);
        report.CompanyName = company?.Name ?? "Unknown Company";

        // Get GL entries for the period
        var glEntries = await _context.GeneralLedgerEntries
            .Include(e => e.Account)
            .Where(e => e.CompanyId == request.CompanyId
                && e.TransactionDate >= request.StartDate
                && e.TransactionDate <= request.EndDate)
            .ToListAsync();

        // Revenue items
        var revenueEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Revenue);
        foreach (var entry in revenueEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.CreditAmount - e.DebitAmount);
            
            if (amount != 0)
            {
                report.RevenueItems.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalRevenue = report.RevenueItems.Sum(i => i.CurrentAmount);

        // COGS items
        var cogsEntries = glEntries.Where(e => e.Account.IsCOGS);
        foreach (var entry in cogsEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (amount != 0)
            {
                report.COGSItems.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalCOGS = report.COGSItems.Sum(i => i.CurrentAmount);

        // Operating expenses
        var expenseEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Expense && !e.Account.IsCOGS);
        foreach (var entry in expenseEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (amount != 0)
            {
                report.OperatingExpenseItems.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalOperatingExpenses = report.OperatingExpenseItems.Sum(i => i.CurrentAmount);

        // Calculations
        report.GrossProfit = report.TotalRevenue - report.TotalCOGS;
        report.GrossProfitMargin = report.TotalRevenue != 0 ? (report.GrossProfit / report.TotalRevenue) * 100 : 0;
        report.OperatingIncome = report.GrossProfit - report.TotalOperatingExpenses;
        report.NetIncomeBeforeTaxes = report.OperatingIncome + report.TotalOtherIncome - report.TotalOtherExpense;
        report.NetIncome = report.NetIncomeBeforeTaxes - report.TaxExpense;
        report.NetProfitMargin = report.TotalRevenue != 0 ? (report.NetIncome / report.TotalRevenue) * 100 : 0;

        return report;
    }

    public async Task<BalanceSheetReportDto> GenerateBalanceSheetAsync(ReportRequestDto request)
    {
        _logger.LogInformation("Generating Balance Sheet for company {CompanyId}", request.CompanyId);

        var report = new BalanceSheetReportDto
        {
            CompanyId = request.CompanyId,
            AsOfDate = request.EndDate,
            GeneratedAt = DateTime.UtcNow
        };

        // Get company name
        var company = await _context.Companies.FindAsync(request.CompanyId);
        report.CompanyName = company?.Name ?? "Unknown Company";

        // Get GL entries up to the date
        var glEntries = await _context.GeneralLedgerEntries
            .Include(e => e.Account)
            .Where(e => e.CompanyId == request.CompanyId && e.TransactionDate <= request.EndDate)
            .ToListAsync();

        // Current Assets - simplified without SubType
        var currentAssetEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Asset && e.Account.AccountName.Contains("Current"));
        foreach (var entry in currentAssetEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (amount != 0)
            {
                report.CurrentAssets.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalCurrentAssets = report.CurrentAssets.Sum(i => i.CurrentAmount);

        // Fixed Assets
        var fixedAssetEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Asset && (e.Account.AccountName.Contains("Fixed") || e.Account.AccountName.Contains("Equipment")));
        foreach (var entry in fixedAssetEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (amount != 0)
            {
                report.FixedAssets.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalFixedAssets = report.FixedAssets.Sum(i => i.CurrentAmount);

        // Other Assets
        var otherAssetEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Asset && !e.Account.AccountName.Contains("Current") && !e.Account.AccountName.Contains("Fixed") && !e.Account.AccountName.Contains("Equipment"));
        foreach (var entry in otherAssetEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (amount != 0)
            {
                report.OtherAssets.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalOtherAssets = report.OtherAssets.Sum(i => i.CurrentAmount);
        report.TotalAssets = report.TotalCurrentAssets + report.TotalFixedAssets + report.TotalOtherAssets;

        // Current Liabilities
        var currentLiabilityEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Liability && e.Account.AccountName.Contains("Current"));
        foreach (var entry in currentLiabilityEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.CreditAmount - e.DebitAmount);
            
            if (amount != 0)
            {
                report.CurrentLiabilities.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalCurrentLiabilities = report.CurrentLiabilities.Sum(i => i.CurrentAmount);

        // Long-term Liabilities
        var longTermLiabilityEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Liability && !e.Account.AccountName.Contains("Current"));
        foreach (var entry in longTermLiabilityEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.CreditAmount - e.DebitAmount);
            
            if (amount != 0)
            {
                report.LongTermLiabilities.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalLongTermLiabilities = report.LongTermLiabilities.Sum(i => i.CurrentAmount);
        report.TotalLiabilities = report.TotalCurrentLiabilities + report.TotalLongTermLiabilities;

        // Equity
        var equityEntries = glEntries.Where(e => e.Account.Type == Core.Enums.AccountType.Equity);
        foreach (var entry in equityEntries.GroupBy(e => e.AccountId))
        {
            var account = entry.First().Account;
            var amount = entry.Sum(e => e.CreditAmount - e.DebitAmount);
            
            if (amount != 0)
            {
                report.EquityItems.Add(new ReportLineItemDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    CurrentAmount = amount
                });
            }
        }
        report.TotalEquity = report.EquityItems.Sum(i => i.CurrentAmount);
        report.TotalLiabilitiesAndEquity = report.TotalLiabilities + report.TotalEquity;

        // Calculate ratios
        report.IsBalanced = Math.Abs(report.TotalAssets - report.TotalLiabilitiesAndEquity) < 0.01m;
        report.CurrentRatio = report.TotalCurrentLiabilities != 0 ? report.TotalCurrentAssets / report.TotalCurrentLiabilities : 0;
        report.DebtToEquityRatio = report.TotalEquity != 0 ? report.TotalLiabilities / report.TotalEquity : 0;
        report.WorkingCapital = report.TotalCurrentAssets - report.TotalCurrentLiabilities;

        return report;
    }

    public async Task<CashFlowReportDto> GenerateCashFlowAsync(ReportRequestDto request)
    {
        _logger.LogInformation("Generating Cash Flow report for company {CompanyId}", request.CompanyId);

        var report = new CashFlowReportDto
        {
            CompanyId = request.CompanyId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GeneratedAt = DateTime.UtcNow
        };

        // Get company name
        var company = await _context.Companies.FindAsync(request.CompanyId);
        report.CompanyName = company?.Name ?? "Unknown Company";

        // Simplified cash flow - would need more complex logic for full implementation
        report.BeginningCashBalance = 0;
        report.NetCashFromOperating = 0;
        report.NetCashFromInvesting = 0;
        report.NetCashFromFinancing = 0;
        report.NetCashChange = report.NetCashFromOperating + report.NetCashFromInvesting + report.NetCashFromFinancing;
        report.EndingCashBalance = report.BeginningCashBalance + report.NetCashChange;

        return report;
    }

    public async Task<byte[]> ExportProfitAndLossToPdfAsync(ReportRequestDto request)
    {
        var report = await GenerateProfitAndLossAsync(request);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .AlignCenter()
                    .Text($"{report.CompanyName}\nProfit & Loss Statement\n{report.PeriodDescription}")
                    .FontSize(14)
                    .Bold();

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Item().Text("Revenue").Bold();
                        foreach (var item in report.RevenueItems)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"  {item.AccountName}");
                                row.ConstantItem(100).AlignRight().Text($"{item.CurrentAmount:C}");
                            });
                        }
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Total Revenue").Bold();
                            row.ConstantItem(100).AlignRight().Text($"{report.TotalRevenue:C}").Bold();
                        });

                        col.Item().PaddingTop(10).Text("Cost of Goods Sold").Bold();
                        foreach (var item in report.COGSItems)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"  {item.AccountName}");
                                row.ConstantItem(100).AlignRight().Text($"{item.CurrentAmount:C}");
                            });
                        }
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Total COGS").Bold();
                            row.ConstantItem(100).AlignRight().Text($"{report.TotalCOGS:C}").Bold();
                        });

                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Text("Gross Profit").Bold();
                            row.ConstantItem(100).AlignRight().Text($"{report.GrossProfit:C}").Bold();
                        });

                        col.Item().PaddingTop(10).Text("Operating Expenses").Bold();
                        foreach (var item in report.OperatingExpenseItems)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"  {item.AccountName}");
                                row.ConstantItem(100).AlignRight().Text($"{item.CurrentAmount:C}");
                            });
                        }
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Total Operating Expenses").Bold();
                            row.ConstantItem(100).AlignRight().Text($"{report.TotalOperatingExpenses:C}").Bold();
                        });

                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Text("Net Income").Bold().FontSize(12);
                            row.ConstantItem(100).AlignRight().Text($"{report.NetIncome:C}").Bold().FontSize(12);
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("MMM dd, yyyy"));
                    });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<byte[]> ExportBalanceSheetToExcelAsync(ReportRequestDto request)
    {
        // Placeholder - would need ClosedXML implementation
        var report = await GenerateBalanceSheetAsync(request);
        
        // For now, return empty byte array as we need ClosedXML package
        return Array.Empty<byte>();
    }
}

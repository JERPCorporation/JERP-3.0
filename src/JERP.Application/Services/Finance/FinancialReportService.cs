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

using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

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

    /// <summary>
    /// Generates a Profit and Loss (Income Statement) report
    /// </summary>
    public async Task<ProfitAndLossReportDto> GenerateProfitAndLossReportAsync(
        Guid companyId, 
        DateTime startDate, 
        DateTime endDate)
    {
        var report = new ProfitAndLossReportDto
        {
            CompanyId = companyId,
            StartDate = startDate,
            EndDate = endDate
        };

        // Get all GL entries for the period
        var glEntries = await _context.GeneralLedgerEntries
            .Include(e => e.Account)
            .Where(e => e.CompanyId == companyId 
                && e.TransactionDate >= startDate 
                && e.TransactionDate <= endDate)
            .ToListAsync();

        // Get accounts for categorization
        var accounts = await _context.Accounts
            .Where(a => a.CompanyId == companyId)
            .ToListAsync();

        // Revenue accounts
        var revenueAccounts = accounts.Where(a => a.Type == AccountType.Revenue).ToList();
        foreach (var account in revenueAccounts)
        {
            var entries = glEntries.Where(e => e.AccountId == account.Id);
            var balance = entries.Sum(e => e.CreditAmount - e.DebitAmount);
            
            if (balance != 0)
            {
                report.Revenue.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalRevenue = report.Revenue.Sum(a => a.Balance);

        // COGS accounts (Expense accounts marked as COGS)
        var cogsAccounts = accounts.Where(a => a.Type == AccountType.Expense && a.IsCOGS).ToList();
        foreach (var account in cogsAccounts)
        {
            var entries = glEntries.Where(e => e.AccountId == account.Id);
            var balance = entries.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (balance != 0)
            {
                report.CostOfGoodsSold.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalCOGS = report.CostOfGoodsSold.Sum(a => a.Balance);

        // Calculate Gross Profit
        report.GrossProfit = report.TotalRevenue - report.TotalCOGS;

        // Operating Expense accounts (non-COGS expenses)
        var expenseAccounts = accounts.Where(a => a.Type == AccountType.Expense && !a.IsCOGS).ToList();
        foreach (var account in expenseAccounts)
        {
            var entries = glEntries.Where(e => e.AccountId == account.Id);
            var balance = entries.Sum(e => e.DebitAmount - e.CreditAmount);
            
            if (balance != 0)
            {
                report.Expenses.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalExpenses = report.Expenses.Sum(a => a.Balance);

        // Calculate Net Income
        report.NetIncome = report.GrossProfit - report.TotalExpenses;

        // Calculate 280E compliance totals
        report.Total280EDeductible = report.CostOfGoodsSold.Sum(a => a.Balance);
        report.Total280ENonDeductible = report.Expenses.Where(a => a.IsNonDeductible).Sum(a => a.Balance);

        _logger.LogInformation("Generated P&L report for company {CompanyId} from {StartDate} to {EndDate}",
            companyId, startDate, endDate);

        return report;
    }

    /// <summary>
    /// Generates a Balance Sheet report
    /// </summary>
    public async Task<BalanceSheetReportDto> GenerateBalanceSheetReportAsync(
        Guid companyId, 
        DateTime asOfDate)
    {
        var report = new BalanceSheetReportDto
        {
            CompanyId = companyId,
            AsOfDate = asOfDate
        };

        // Get all accounts with their balances
        var accounts = await _context.Accounts
            .Where(a => a.CompanyId == companyId && a.IsActive)
            .ToListAsync();

        // For a proper balance sheet, we should calculate balances from GL entries up to the asOfDate
        // For simplicity in MVP, we'll use the current account balances
        // In production, you'd sum GL entries up to the asOfDate

        // Assets
        var assetAccounts = accounts.Where(a => a.Type == AccountType.Asset).ToList();
        foreach (var account in assetAccounts)
        {
            if (account.Balance != 0)
            {
                report.Assets.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = account.Balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalAssets = report.Assets.Sum(a => a.Balance);

        // Liabilities
        var liabilityAccounts = accounts.Where(a => a.Type == AccountType.Liability).ToList();
        foreach (var account in liabilityAccounts)
        {
            if (account.Balance != 0)
            {
                report.Liabilities.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = account.Balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalLiabilities = report.Liabilities.Sum(a => a.Balance);

        // Equity
        var equityAccounts = accounts.Where(a => a.Type == AccountType.Equity).ToList();
        foreach (var account in equityAccounts)
        {
            if (account.Balance != 0)
            {
                report.Equity.Add(new AccountSummaryDto
                {
                    AccountId = account.Id,
                    AccountNumber = account.AccountNumber,
                    AccountName = account.AccountName,
                    Balance = account.Balance,
                    IsCOGS = account.IsCOGS,
                    IsNonDeductible = account.IsNonDeductible
                });
            }
        }
        report.TotalEquity = report.Equity.Sum(a => a.Balance);

        _logger.LogInformation("Generated Balance Sheet report for company {CompanyId} as of {AsOfDate}",
            companyId, asOfDate);

        return report;
    }
}

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

using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for generating cash flow forecasts
/// </summary>
public class CashFlowForecastService : ICashFlowForecastService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<CashFlowForecastService> _logger;

    public CashFlowForecastService(
        JerpDbContext context,
        ILogger<CashFlowForecastService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Generate cash flow forecast based on AR and AP due dates
    /// </summary>
    public async Task<CashFlowForecastDto> GenerateForecastAsync(Guid companyId, DateTime startDate, int days = 30)
    {
        var endDate = startDate.AddDays(days);

        // Get current cash balance
        var cashAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountNumber == "1000");

        var beginningCashBalance = cashAccount?.Balance ?? 0;

        // Get unpaid invoices (expected inflows)
        var unpaidInvoices = await _context.CustomerInvoices
            .Where(i => i.CompanyId == companyId 
                && !i.IsPaid 
                && i.DueDate >= startDate 
                && i.DueDate <= endDate
                && (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue))
            .Select(i => new
            {
                i.DueDate,
                Amount = i.TotalAmount - i.AmountPaid
            })
            .ToListAsync();

        // Get unpaid bills (expected outflows)
        var unpaidBills = await _context.VendorBills
            .Where(b => b.CompanyId == companyId 
                && !b.IsPaid 
                && b.DueDate >= startDate 
                && b.DueDate <= endDate
                && b.Status == BillStatus.Approved)
            .Select(b => new
            {
                b.DueDate,
                Amount = b.TotalAmount - b.AmountPaid
            })
            .ToListAsync();

        // Group by week
        var weeklyProjections = new List<CashFlowWeekDto>();
        var currentDate = startDate;
        var runningBalance = beginningCashBalance;

        while (currentDate <= endDate)
        {
            var weekEnd = currentDate.AddDays(6);
            if (weekEnd > endDate)
                weekEnd = endDate;

            var weekInflows = unpaidInvoices
                .Where(i => i.DueDate >= currentDate && i.DueDate <= weekEnd)
                .Sum(i => i.Amount);

            var weekOutflows = unpaidBills
                .Where(b => b.DueDate >= currentDate && b.DueDate <= weekEnd)
                .Sum(b => b.Amount);

            var netCashFlow = weekInflows - weekOutflows;
            runningBalance += netCashFlow;

            weeklyProjections.Add(new CashFlowWeekDto
            {
                WeekStartDate = currentDate,
                WeekEndDate = weekEnd,
                ExpectedInflows = weekInflows,
                ExpectedOutflows = weekOutflows
            });

            currentDate = weekEnd.AddDays(1);
        }

        var totalInflows = weeklyProjections.Sum(w => w.ExpectedInflows);
        var totalOutflows = weeklyProjections.Sum(w => w.ExpectedOutflows);

        _logger.LogInformation("Generated cash flow forecast for company {CompanyId} from {StartDate} to {EndDate}. " +
            "Beginning balance: ${BeginningBalance}, Expected inflows: ${Inflows}, Expected outflows: ${Outflows}, " +
            "Projected ending balance: ${EndingBalance}",
            companyId, startDate, endDate, beginningCashBalance, totalInflows, totalOutflows, runningBalance);

        return new CashFlowForecastDto
        {
            StartDate = startDate,
            EndDate = endDate,
            BeginningCashBalance = beginningCashBalance,
            WeeklyProjections = weeklyProjections,
            TotalExpectedInflows = totalInflows,
            TotalExpectedOutflows = totalOutflows,
            ProjectedEndingBalance = runningBalance
        };
    }
}

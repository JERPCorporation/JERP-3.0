/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Service for dashboard and KPI generation
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        JerpDbContext context,
        ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardKPIDto> GetDashboardKPIsAsync(Guid companyId, DateTime? asOfDate = null)
    {
        var effectiveDate = asOfDate ?? DateTime.UtcNow;
        _logger.LogInformation("Getting dashboard KPIs for company {CompanyId} as of {Date}", companyId, effectiveDate);

        var kpi = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = effectiveDate,
            GeneratedAt = DateTime.UtcNow
        };

        // Financial KPIs
        var startOfMonth = new DateTime(effectiveDate.Year, effectiveDate.Month, 1);
        var startOfYear = new DateTime(effectiveDate.Year, 1, 1);

        // MTD Revenue
        var revenueEntriesMTD = await _context.GeneralLedgerEntries
            .Where(e => e.CompanyId == companyId
                && e.Account.Type == AccountType.Revenue
                && e.TransactionDate >= startOfMonth
                && e.TransactionDate <= effectiveDate)
            .SumAsync(e => e.CreditAmount - e.DebitAmount);
        kpi.TotalRevenueMTD = revenueEntriesMTD;

        // YTD Revenue
        var revenueEntriesYTD = await _context.GeneralLedgerEntries
            .Where(e => e.CompanyId == companyId
                && e.Account.Type == AccountType.Revenue
                && e.TransactionDate >= startOfYear
                && e.TransactionDate <= effectiveDate)
            .SumAsync(e => e.CreditAmount - e.DebitAmount);
        kpi.TotalRevenueYTD = revenueEntriesYTD;

        // Cash Balance (from Cash accounts)
        var cashBalance = await _context.GeneralLedgerEntries
            .Where(e => e.CompanyId == companyId
                && e.Account.Type == AccountType.Asset
                && e.Account.AccountName.Contains("Cash")
                && e.TransactionDate <= effectiveDate)
            .GroupBy(e => e.AccountId)
            .Select(g => g.Sum(e => e.DebitAmount - e.CreditAmount))
            .SumAsync();
        kpi.CashBalance = cashBalance;

        // Employee Count
        var employeeCount = await _context.Employees
            .Where(e => e.CompanyId == companyId)
            .CountAsync();
        kpi.EmployeeCount = employeeCount;

        // Payroll Cost MTD
        var payrollCostMTD = await _context.PayrollRecords
            .Where(p => p.Employee.CompanyId == companyId
                && p.PayPeriod.StartDate >= startOfMonth
                && p.PayPeriod.EndDate <= effectiveDate)
            .SumAsync(p => p.GrossPay);
        kpi.TotalPayrollCostMTD = payrollCostMTD;

        return kpi;
    }

    public async Task<List<AlertDto>> GetAlertsAsync(Guid companyId)
    {
        _logger.LogInformation("Getting alerts for company {CompanyId}", companyId);

        var alerts = new List<AlertDto>();

        // Check for low cash balance
        var cashBalance = await _context.GeneralLedgerEntries
            .Where(e => e.CompanyId == companyId
                && e.Account.Type == AccountType.Asset
                && e.Account.AccountName.Contains("Cash"))
            .GroupBy(e => e.AccountId)
            .Select(g => g.Sum(e => e.DebitAmount - e.CreditAmount))
            .SumAsync();

        if (cashBalance < 10000)
        {
            alerts.Add(new AlertDto
            {
                AlertId = Guid.NewGuid(),
                AlertType = "LowCash",
                Severity = "Warning",
                Title = "Low Cash Balance",
                Message = $"Cash balance is {cashBalance:C}, which is below the recommended threshold.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            });
        }

        return alerts;
    }
}

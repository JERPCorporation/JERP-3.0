/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Infrastructure.Data;
using JERP.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for dashboard statistics and metrics
/// </summary>
[Authorize]
[Route("api/v1/[controller]")]
public class DashboardController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        JerpDbContext context,
        ILogger<DashboardController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get dashboard overview statistics
    /// </summary>
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var today = DateTime.UtcNow.Date;
        var thisMonth = new DateTime(today.Year, today.Month, 1);

        var stats = new
        {
            // Employee statistics
            TotalEmployees = await _context.Employees.CountAsync(),
            ActiveEmployees = await _context.Employees.CountAsync(e => e.Status == EmployeeStatus.Active),
            NewHiresThisMonth = await _context.Employees.CountAsync(e => e.HireDate >= thisMonth),

            // Payroll statistics
            CurrentPayPeriod = await _context.PayPeriods
                .Where(p => p.StartDate <= today && p.EndDate >= today)
                .Select(p => new { p.Id, p.StartDate, p.EndDate, p.PayDate })
                .FirstOrDefaultAsync(),

            PendingTimesheets = await _context.Timesheets
                .CountAsync(t => t.Status == TimesheetStatus.Submitted || t.Status == TimesheetStatus.Draft),

            PendingPayrolls = await _context.PayrollRecords
                .CountAsync(p => p.Status == PayrollStatus.Calculated || p.Status == PayrollStatus.Draft),

            // Compliance statistics
            OpenViolations = await _context.ComplianceViolations
                .CountAsync(v => v.Status == ViolationStatus.Open),

            CriticalViolations = await _context.ComplianceViolations
                .CountAsync(v => v.Status == ViolationStatus.Open && v.Severity == ViolationSeverity.Critical),

            // Recent activity
            RecentActivity = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(10)
                .Select(a => new
                {
                    a.Action,
                    a.EntityType,
                    a.Timestamp,
                    a.UserId
                })
                .ToListAsync()
        };

        return Ok(stats);
    }

    /// <summary>
    /// Get payroll metrics
    /// </summary>
    [HttpGet("payroll-metrics")]
    public async Task<IActionResult> GetPayrollMetrics([FromQuery] int months = 6)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months);

        var metrics = await _context.PayrollRecords
            .Include(p => p.PayPeriod)
            .Where(p => p.PayPeriod.StartDate >= startDate)
            .GroupBy(p => new { p.PayPeriod.StartDate.Year, p.PayPeriod.StartDate.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalGrossPay = g.Sum(p => p.GrossPay),
                TotalNetPay = g.Sum(p => p.NetPay),
                TotalTaxes = g.Sum(p => p.TotalTaxes),
                TotalDeductions = g.Sum(p => p.TotalDeductions),
                EmployeeCount = g.Select(p => p.EmployeeId).Distinct().Count()
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToListAsync();

        return Ok(metrics);
    }

    /// <summary>
    /// Get compliance score trend
    /// </summary>
    [HttpGet("compliance-trend")]
    public async Task<IActionResult> GetComplianceTrend([FromQuery] int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var violations = await _context.ComplianceViolations
            .Where(v => v.DetectedAt >= startDate)
            .GroupBy(v => v.DetectedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count(),
                Critical = g.Count(v => v.Severity == ViolationSeverity.Critical),
                High = g.Count(v => v.Severity == ViolationSeverity.High),
                Medium = g.Count(v => v.Severity == ViolationSeverity.Medium),
                Low = g.Count(v => v.Severity == ViolationSeverity.Low)
            })
            .OrderBy(v => v.Date)
            .ToListAsync();

        return Ok(violations);
    }

    /// <summary>
    /// Get employee distribution by department
    /// </summary>
    [HttpGet("employee-distribution")]
    public async Task<IActionResult> GetEmployeeDistribution()
    {
        var distribution = await _context.Employees
            .Include(e => e.Department)
            .GroupBy(e => new { e.DepartmentId, e.Department.Name })
            .Select(g => new
            {
                DepartmentId = g.Key.DepartmentId,
                DepartmentName = g.Key.Name ?? "Unassigned",
                EmployeeCount = g.Count(),
                ActiveCount = g.Count(e => e.Status == EmployeeStatus.Active),
                AverageHourlyRate = g.Where(e => e.HourlyRate.HasValue).Average(e => e.HourlyRate),
                AverageSalary = g.Where(e => e.SalaryAmount.HasValue).Average(e => e.SalaryAmount)
            })
            .ToListAsync();

        return Ok(distribution);
    }

    /// <summary>
    /// Get recent timesheets requiring approval
    /// </summary>
    [HttpGet("pending-approvals")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        var pendingTimesheets = await _context.Timesheets
            .Include(t => t.Employee)
            .Where(t => t.Status == TimesheetStatus.Submitted)
            .OrderBy(t => t.SubmittedAt)
            .Take(20)
            .Select(t => new
            {
                t.Id,
                t.WorkDate,
                EmployeeName = $"{t.Employee.FirstName} {t.Employee.LastName}",
                t.TotalHours,
                t.SubmittedAt
            })
            .ToListAsync();

        var pendingPayrolls = await _context.PayrollRecords
            .Include(p => p.Employee)
            .Include(p => p.PayPeriod)
            .Where(p => p.Status == PayrollStatus.Calculated)
            .OrderBy(p => p.CalculatedAt)
            .Take(20)
            .Select(p => new
            {
                p.Id,
                EmployeeName = $"{p.Employee.FirstName} {p.Employee.LastName}",
                p.GrossPay,
                p.NetPay,
                PayPeriod = new { p.PayPeriod.StartDate, p.PayPeriod.EndDate },
                p.CalculatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            Timesheets = pendingTimesheets,
            Payrolls = pendingPayrolls
        });
    }
}

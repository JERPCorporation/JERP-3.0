using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Infrastructure.Data;
using JERP.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for generating various reports
/// </summary>
[Authorize]
[Route("api/v1/[controller]")]
public class ReportsController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        JerpDbContext context,
        ILogger<ReportsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get payroll summary report
    /// </summary>
    [HttpGet("payroll/summary")]
    public async Task<IActionResult> GetPayrollSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var payrolls = await _context.PayrollRecords
            .Include(p => p.Employee)
            .Include(p => p.PayPeriod)
            .Where(p => p.PayPeriod.StartDate >= start && p.PayPeriod.EndDate <= end)
            .ToListAsync();

        var summary = new
        {
            TotalRecords = payrolls.Count,
            TotalGrossPay = payrolls.Sum(p => p.GrossPay),
            TotalNetPay = payrolls.Sum(p => p.NetPay),
            TotalTaxes = payrolls.Sum(p => p.TotalTaxes),
            TotalDeductions = payrolls.Sum(p => p.TotalDeductions),
            AverageGrossPay = payrolls.Average(p => p.GrossPay),
            AverageNetPay = payrolls.Average(p => p.NetPay)
        };

        return Ok(summary);
    }

    /// <summary>
    /// Get employee hours report
    /// </summary>
    [HttpGet("employees/hours")]
    public async Task<IActionResult> GetEmployeeHoursReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var timesheets = await _context.Timesheets
            .Include(t => t.Employee)
            .Where(t => t.WorkDate >= start && t.WorkDate <= end)
            .GroupBy(t => new { t.EmployeeId, t.Employee.FirstName, t.Employee.LastName })
            .Select(g => new
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = $"{g.Key.FirstName} {g.Key.LastName}",
                TotalHours = g.Sum(t => t.TotalHours),
                RegularHours = g.Sum(t => t.RegularHours),
                OvertimeHours = g.Sum(t => t.OvertimeHours),
                DoubleTimeHours = g.Sum(t => t.DoubleTimeHours)
            })
            .ToListAsync();

        return Ok(timesheets);
    }

    /// <summary>
    /// Get compliance violations report
    /// </summary>
    [HttpGet("compliance/violations")]
    public async Task<IActionResult> GetComplianceReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var violations = await _context.ComplianceViolations
            .Where(v => v.DetectedAt >= start && v.DetectedAt <= end)
            .GroupBy(v => v.Severity)
            .Select(g => new
            {
                Severity = g.Key.ToString(),
                Count = g.Count(),
                Violations = g.Select(v => new
                {
                    v.Id,
                    v.RuleName,
                    v.Description,
                    v.DetectedAt,
                    v.EntityType,
                    v.EntityId
                }).ToList()
            })
            .ToListAsync();

        return Ok(violations);
    }

    /// <summary>
    /// Get department summary report
    /// </summary>
    [HttpGet("departments/summary")]
    public async Task<IActionResult> GetDepartmentSummary()
    {
        var departments = await _context.Departments
            .Include(d => d.Employees)
            .Select(d => new
            {
                d.Id,
                d.Name,
                EmployeeCount = d.Employees.Count,
                ActiveEmployees = d.Employees.Count(e => e.Status == EmployeeStatus.Active),
                AverageHourlyRate = d.Employees.Where(e => e.HourlyRate.HasValue).Average(e => e.HourlyRate),
                AverageSalary = d.Employees.Where(e => e.SalaryAmount.HasValue).Average(e => e.SalaryAmount)
            })
            .ToListAsync();

        return Ok(departments);
    }
}

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

using JERP.Compliance.Core;
using JERP.Compliance.LaborLaw.Rules;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JERP.Compliance.Services;

/// <summary>
/// Compliance engine that evaluates rules and tracks violations
/// </summary>
public class ComplianceEngine : IComplianceEngine
{
    private readonly JerpDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ComplianceEngine> _logger;
    private readonly List<Type> _registeredRuleTypes = new();

    public ComplianceEngine(
        JerpDbContext context,
        IServiceProvider serviceProvider,
        ILogger<ComplianceEngine> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public Task RegisterRuleAsync<T>() where T : IComplianceRule
    {
        var ruleType = typeof(T);
        if (!_registeredRuleTypes.Contains(ruleType))
        {
            _registeredRuleTypes.Add(ruleType);
            _logger.LogInformation("Registered compliance rule: {RuleName}", ruleType.Name);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<List<ComplianceViolation>> EvaluateEmployeeAsync(Guid employeeId)
    {
        _logger.LogInformation("Evaluating compliance for employee {EmployeeId}", employeeId);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            _logger.LogWarning("Employee {EmployeeId} not found", employeeId);
            return new List<ComplianceViolation>();
        }

        var allViolations = new List<ComplianceViolation>();

        // Evaluate minimum wage rule
        var minWageRule = _serviceProvider.GetRequiredService<MinimumWageRule>();
        var minWageResult = await minWageRule.EvaluateAsync(employee);
        allViolations.AddRange(minWageResult.Violations);

        // Check minimum age compliance (hire date vs DOB)
        if (employee.DateOfBirth.HasValue)
        {
            var ageAtHire = CalculateAge(employee.DateOfBirth.Value, employee.HireDate);
            if (ageAtHire < ComplianceConstants.MinimumWorkingAge)
            {
                var violation = new ComplianceViolation
                {
                    Id = Guid.NewGuid(),
                    ViolationType = ViolationType.LaborLaw,
                    Severity = ViolationSeverity.Critical,
                    Status = ViolationStatus.Open,
                    EntityType = "Employee",
                    EntityId = employeeId,
                    RuleName = "Minimum Age Requirement",
                    Description = $"Employee {employee.FirstName} {employee.LastName} was hired at age {ageAtHire}, below the federal minimum working age of {ComplianceConstants.MinimumWorkingAge}.",
                    DetectedAt = DateTime.UtcNow
                };
                allViolations.Add(violation);
            }
        }

        // Persist violations to database
        foreach (var violation in allViolations)
        {
            // Check if violation already exists
            var exists = await _context.ComplianceViolations
                .AnyAsync(v => v.EntityType == violation.EntityType &&
                              v.EntityId == violation.EntityId &&
                              v.RuleName == violation.RuleName &&
                              v.Status == ViolationStatus.Open);

            if (!exists)
            {
                await _context.ComplianceViolations.AddAsync(violation);
            }
        }

        await _context.SaveChangesAsync();

        return allViolations;
    }

    /// <inheritdoc/>
    public async Task<List<ComplianceViolation>> EvaluateTimesheetAsync(Guid timesheetId)
    {
        _logger.LogInformation("Evaluating compliance for timesheet {TimesheetId}", timesheetId);

        var timesheet = await _context.Timesheets
            .Include(t => t.Employee)
            .FirstOrDefaultAsync(t => t.Id == timesheetId);

        if (timesheet == null)
        {
            _logger.LogWarning("Timesheet {TimesheetId} not found", timesheetId);
            return new List<ComplianceViolation>();
        }

        var allViolations = new List<ComplianceViolation>();

        // Get timesheets for the week
        var weekStart = timesheet.WorkDate.AddDays(-(int)timesheet.WorkDate.DayOfWeek);
        var weekEnd = weekStart.AddDays(7);
        var weekTimesheets = await _context.Timesheets
            .Where(t => t.EmployeeId == timesheet.EmployeeId &&
                       t.WorkDate >= weekStart &&
                       t.WorkDate < weekEnd)
            .OrderBy(t => t.WorkDate)
            .ToListAsync();

        // Evaluate California daily overtime rule
        var dailyOvertimeRule = _serviceProvider.GetRequiredService<CaliforniaDailyOvertimeRule>();
        var dailyOvertimeResult = await dailyOvertimeRule.EvaluateAsync(timesheet);
        allViolations.AddRange(dailyOvertimeResult.Violations);

        // Evaluate California 7th day overtime rule
        try
        {
            var seventhDayRule = _serviceProvider.GetRequiredService<CaliforniaSeventhDayOvertimeRule>();
            var seventhDayResult = await seventhDayRule.EvaluateAsync((timesheet, weekTimesheets));
            allViolations.AddRange(seventhDayResult.Violations);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error evaluating 7th day overtime rule for timesheet {TimesheetId}", timesheetId);
        }

        // Evaluate Federal weekly overtime rule
        var weeklyOvertimeRule = _serviceProvider.GetRequiredService<FederalWeeklyOvertimeRule>();
        var weeklyOvertimeResult = await weeklyOvertimeRule.EvaluateAsync((timesheet.Employee, weekTimesheets));
        allViolations.AddRange(weeklyOvertimeResult.Violations);

        // Evaluate meal break rule
        var mealBreakRule = _serviceProvider.GetRequiredService<MealBreakRule>();
        var mealBreakResult = await mealBreakRule.EvaluateAsync(timesheet);
        allViolations.AddRange(mealBreakResult.Violations);

        // Evaluate rest break rule
        var restBreakRule = _serviceProvider.GetRequiredService<RestBreakRule>();
        var restBreakResult = await restBreakRule.EvaluateAsync(timesheet);
        allViolations.AddRange(restBreakResult.Violations);

        // Evaluate child labor rule (if applicable)
        if (timesheet.Employee.DateOfBirth.HasValue)
        {
            var age = CalculateAge(timesheet.Employee.DateOfBirth.Value, timesheet.WorkDate);
            if (age < ComplianceConstants.MinorAge)
            {
                var childLaborRule = _serviceProvider.GetRequiredService<ChildLaborRule>();
                var childLaborResult = await childLaborRule.EvaluateAsync((timesheet.Employee, timesheet));
                allViolations.AddRange(childLaborResult.Violations);
            }
        }

        // Persist violations to database
        foreach (var violation in allViolations)
        {
            // Check if violation already exists
            var exists = await _context.ComplianceViolations
                .AnyAsync(v => v.EntityType == violation.EntityType &&
                              v.EntityId == violation.EntityId &&
                              v.RuleName == violation.RuleName &&
                              v.Status == ViolationStatus.Open);

            if (!exists)
            {
                await _context.ComplianceViolations.AddAsync(violation);
            }
        }

        await _context.SaveChangesAsync();

        return allViolations;
    }

    /// <inheritdoc/>
    public async Task<List<ComplianceViolation>> EvaluatePayrollAsync(Guid payrollRecordId)
    {
        _logger.LogInformation("Evaluating compliance for payroll record {PayrollRecordId}", payrollRecordId);

        var payrollRecord = await _context.PayrollRecords
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.Id == payrollRecordId);

        if (payrollRecord == null)
        {
            _logger.LogWarning("Payroll record {PayrollRecordId} not found", payrollRecordId);
            return new List<ComplianceViolation>();
        }

        var allViolations = new List<ComplianceViolation>();

        // Validate minimum wage compliance
        if (payrollRecord.Employee.HourlyRate.HasValue && payrollRecord.RegularHours > 0)
        {
            var effectiveRate = payrollRecord.RegularPay / payrollRecord.RegularHours;
            var minimumWage = ComplianceConstants.CaliforniaMinimumWage;

            if (effectiveRate < minimumWage)
            {
                var violation = new ComplianceViolation
                {
                    Id = Guid.NewGuid(),
                    ViolationType = ViolationType.LaborLaw,
                    Severity = ViolationSeverity.Critical,
                    Status = ViolationStatus.Open,
                    EntityType = "PayrollRecord",
                    EntityId = payrollRecordId,
                    RuleName = "Minimum Wage Compliance",
                    Description = $"Effective pay rate ${effectiveRate:F2}/hour is below minimum wage of ${minimumWage:F2}/hour",
                    FinancialImpact = (minimumWage - effectiveRate) * payrollRecord.RegularHours,
                    DetectedAt = DateTime.UtcNow
                };
                allViolations.Add(violation);
            }
        }

        // Validate overtime pay calculations
        if (payrollRecord.OvertimeHours > 0)
        {
            var expectedOvertimePay = payrollRecord.OvertimeHours * (payrollRecord.Employee.HourlyRate ?? 0) * ComplianceConstants.OvertimeRate;
            var overtimeDiscrepancy = Math.Abs(payrollRecord.OvertimePay - expectedOvertimePay);

            if (overtimeDiscrepancy > 0.01m)
            {
                var violation = new ComplianceViolation
                {
                    Id = Guid.NewGuid(),
                    ViolationType = ViolationType.LaborLaw,
                    Severity = ViolationSeverity.High,
                    Status = ViolationStatus.Open,
                    EntityType = "PayrollRecord",
                    EntityId = payrollRecordId,
                    RuleName = "Overtime Pay Calculation",
                    Description = $"Overtime pay calculation error. Expected ${expectedOvertimePay:F2} but found ${payrollRecord.OvertimePay:F2}",
                    FinancialImpact = overtimeDiscrepancy,
                    DetectedAt = DateTime.UtcNow
                };
                allViolations.Add(violation);
            }
        }

        // Validate tax calculations (basic validation)
        // Note: Social Security wage base for 2024 is $168,600 annually
        // This is a simplified check - actual per-period limit varies by pay frequency
        var expectedSocialSecurity = Math.Min(payrollRecord.GrossPay * ComplianceConstants.SocialSecurityRate, 10453.20m); // Bi-weekly limit approximation
        var expectedMedicare = payrollRecord.GrossPay * ComplianceConstants.MedicareRate;

        if (Math.Abs(payrollRecord.SocialSecurityTax - expectedSocialSecurity) > 0.50m)
        {
            var violation = new ComplianceViolation
            {
                Id = Guid.NewGuid(),
                ViolationType = ViolationType.FinancialCompliance,
                Severity = ViolationSeverity.Medium,
                Status = ViolationStatus.Open,
                EntityType = "PayrollRecord",
                EntityId = payrollRecordId,
                RuleName = "Social Security Tax Calculation",
                Description = $"Social Security tax calculation appears incorrect. Expected ~${expectedSocialSecurity:F2} but found ${payrollRecord.SocialSecurityTax:F2}",
                DetectedAt = DateTime.UtcNow
            };
            allViolations.Add(violation);
        }

        // Check deduction limits (cannot exceed certain percentage of gross pay)
        // Note: This limit may vary by jurisdiction and deduction type
        var maxDeductionPercent = ComplianceConstants.MaxDeductionPercent;
        var totalDeductionPercent = payrollRecord.GrossPay > 0 
            ? payrollRecord.TotalDeductions / payrollRecord.GrossPay 
            : 0;

        if (totalDeductionPercent > maxDeductionPercent)
        {
            var violation = new ComplianceViolation
            {
                Id = Guid.NewGuid(),
                ViolationType = ViolationType.LaborLaw,
                Severity = ViolationSeverity.High,
                Status = ViolationStatus.Open,
                EntityType = "PayrollRecord",
                EntityId = payrollRecordId,
                RuleName = "Deduction Limit",
                Description = $"Total deductions ({totalDeductionPercent:P}) exceed maximum allowed ({maxDeductionPercent:P})",
                DetectedAt = DateTime.UtcNow
            };
            allViolations.Add(violation);
        }

        // Persist violations to database
        foreach (var violation in allViolations)
        {
            var exists = await _context.ComplianceViolations
                .AnyAsync(v => v.EntityType == violation.EntityType &&
                              v.EntityId == violation.EntityId &&
                              v.RuleName == violation.RuleName &&
                              v.Status == ViolationStatus.Open);

            if (!exists)
            {
                await _context.ComplianceViolations.AddAsync(violation);
            }
        }

        await _context.SaveChangesAsync();

        return allViolations;
    }

    /// <inheritdoc/>
    public async Task<List<ComplianceViolation>> EvaluateAllAsync()
    {
        _logger.LogInformation("Evaluating compliance for all entities");

        var allViolations = new List<ComplianceViolation>();

        // Evaluate all employees
        var employees = await _context.Employees
            .Where(e => !e.IsDeleted)
            .ToListAsync();

        foreach (var employee in employees)
        {
            var violations = await EvaluateEmployeeAsync(employee.Id);
            allViolations.AddRange(violations);
        }

        // Evaluate all recent timesheets (last 30 days)
        var recentDate = DateTime.UtcNow.AddDays(-30);
        var timesheets = await _context.Timesheets
            .Where(t => !t.IsDeleted && t.WorkDate >= recentDate)
            .ToListAsync();

        foreach (var timesheet in timesheets)
        {
            var violations = await EvaluateTimesheetAsync(timesheet.Id);
            allViolations.AddRange(violations);
        }

        // Evaluate all recent payroll records (last 90 days)
        var recentPayrollDate = DateTime.UtcNow.AddDays(-90);
        var payrollRecords = await _context.PayrollRecords
            .Where(p => !p.IsDeleted && p.CreatedAt >= recentPayrollDate)
            .ToListAsync();

        foreach (var payrollRecord in payrollRecords)
        {
            var violations = await EvaluatePayrollAsync(payrollRecord.Id);
            allViolations.AddRange(violations);
        }

        _logger.LogInformation("Compliance evaluation complete. Total violations: {ViolationCount}", allViolations.Count);

        return allViolations;
    }

    /// <inheritdoc/>
    public async Task<double> CalculateComplianceScoreAsync()
    {
        _logger.LogInformation("Calculating compliance score");

        // Get counts of entities checked
        var employeeCount = await _context.Employees.CountAsync(e => !e.IsDeleted);
        var timesheetCount = await _context.Timesheets.CountAsync(t => !t.IsDeleted);
        var payrollCount = await _context.PayrollRecords.CountAsync(p => !p.IsDeleted);

        var totalChecks = employeeCount + timesheetCount + payrollCount;

        if (totalChecks == 0)
        {
            return 100.0; // Perfect score if nothing to check
        }

        // Get count of open violations
        var violationCount = await _context.ComplianceViolations
            .CountAsync(v => v.Status == ViolationStatus.Open);

        // Calculate score: (total checks - violations) / total checks * 100
        var score = ((totalChecks - violationCount) / (double)totalChecks) * 100.0;
        score = Math.Max(0, Math.Min(100, score)); // Clamp between 0 and 100

        _logger.LogInformation("Compliance score: {Score:F2}% ({Violations} violations out of {TotalChecks} checks)", 
            score, violationCount, totalChecks);

        return score;
    }

    /// <inheritdoc/>
    public async Task<List<ComplianceViolation>> GetActiveViolationsAsync()
    {
        return await _context.ComplianceViolations
            .Where(v => v.Status == ViolationStatus.Open)
            .OrderByDescending(v => v.Severity)
            .ThenByDescending(v => v.DetectedAt)
            .ToListAsync();
    }

    private int CalculateAge(DateTime birthDate, DateTime referenceDate)
    {
        var age = referenceDate.Year - birthDate.Year;
        if (referenceDate.Month < birthDate.Month || 
            (referenceDate.Month == birthDate.Month && referenceDate.Day < birthDate.Day))
        {
            age--;
        }
        return age;
    }
}

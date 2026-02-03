using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.LaborLaw.Rules;

/// <summary>
/// Federal FLSA weekly overtime rule: >40 hours per week = 1.5x overtime
/// </summary>
public class FederalWeeklyOvertimeRule : ComplianceRuleBase
{
    public override string RuleName => "Federal Weekly Overtime (FLSA)";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.High;

    public FederalWeeklyOvertimeRule(ILogger<FederalWeeklyOvertimeRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not (Employee employee, List<Timesheet> weekTimesheets))
        {
            throw new ArgumentException("Context must be a tuple of (Employee, List<Timesheet>)", nameof(context));
        }

        LogEvaluationStart("Employee", employee.Id);

        var violations = new List<ComplianceViolation>();

        // Only non-exempt employees are eligible for overtime
        if (employee.Classification != EmployeeClassification.NonExempt)
        {
            return ComplianceResult.Compliant($"Employee {employee.Id} is exempt from overtime");
        }

        // Calculate total weekly hours
        var totalWeeklyHours = weekTimesheets.Sum(t => t.TotalHours);
        var totalWeeklyOvertimeHours = weekTimesheets.Sum(t => t.OvertimeHours);

        // Calculate expected federal overtime (only if weekly hours exceed 40)
        decimal expectedFederalOvertimeHours = 0;
        if (totalWeeklyHours > ComplianceConstants.FederalWeeklyOvertimeThreshold)
        {
            expectedFederalOvertimeHours = totalWeeklyHours - ComplianceConstants.FederalWeeklyOvertimeThreshold;
        }

        // Check if overtime is properly tracked
        if (totalWeeklyOvertimeHours < expectedFederalOvertimeHours - 0.01m)
        {
            var hourlyRate = employee.HourlyRate ?? 0;
            var unpaidOvertimeHours = expectedFederalOvertimeHours - totalWeeklyOvertimeHours;
            var financialImpact = unpaidOvertimeHours * hourlyRate * (ComplianceConstants.OvertimeRate - 1.0m);

            var details = new
            {
                WeekStart = weekTimesheets.Min(t => t.WorkDate),
                WeekEnd = weekTimesheets.Max(t => t.WorkDate),
                TotalWeeklyHours = totalWeeklyHours,
                ExpectedOvertimeHours = expectedFederalOvertimeHours,
                ActualOvertimeHours = totalWeeklyOvertimeHours,
                UnpaidOvertimeHours = unpaidOvertimeHours,
                HourlyRate = hourlyRate
            };

            violations.Add(CreateViolation(
                "Employee",
                employee.Id,
                $"Federal weekly overtime violation. Employee worked {totalWeeklyHours:F2} hours but only {totalWeeklyOvertimeHours:F2} hours classified as overtime. Expected {expectedFederalOvertimeHours:F2} overtime hours.",
                DefaultSeverity,
                financialImpact,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Employee {employee.Id} complies with Federal weekly overtime rules")
            : ComplianceResult.NonCompliant($"Employee {employee.Id} has {violations.Count} Federal weekly overtime violations", violations);
    }
}

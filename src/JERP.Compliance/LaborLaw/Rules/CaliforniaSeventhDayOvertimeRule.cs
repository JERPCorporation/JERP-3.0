using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.LaborLaw.Rules;

/// <summary>
/// California 7th consecutive day overtime rule: First 8 hours = 1.5x, After 8 hours = 2.0x
/// </summary>
public class CaliforniaSeventhDayOvertimeRule : ComplianceRuleBase
{
    public override string RuleName => "California 7th Day Overtime";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.High;

    private const int ConsecutiveDaysThreshold = 7;

    public CaliforniaSeventhDayOvertimeRule(ILogger<CaliforniaSeventhDayOvertimeRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not (Timesheet timesheet, List<Timesheet> weekTimesheets))
        {
            throw new ArgumentException("Context must be a tuple of (Timesheet, List<Timesheet>)", nameof(context));
        }

        LogEvaluationStart("Timesheet", timesheet.Id);

        var violations = new List<ComplianceViolation>();

        // Sort timesheets by date
        var sortedTimesheets = weekTimesheets
            .OrderBy(t => t.WorkDate)
            .ToList();

        // Check if this is the 7th consecutive day
        var consecutiveDays = CountConsecutiveDays(sortedTimesheets, timesheet.WorkDate);

        if (consecutiveDays >= ConsecutiveDaysThreshold)
        {
            // This is the 7th day - all hours should be at premium rates
            var hoursOn7thDay = timesheet.TotalHours;
            decimal expectedOvertimeHours = Math.Min(hoursOn7thDay, ComplianceConstants.CaliforniaDailyOvertimeThreshold);
            decimal expectedDoubleTimeHours = Math.Max(0, hoursOn7thDay - ComplianceConstants.CaliforniaDailyOvertimeThreshold);

            // Check if hours are properly classified
            var overtimeDiscrepancy = Math.Abs(timesheet.OvertimeHours - expectedOvertimeHours);
            var doubleTimeDiscrepancy = Math.Abs(timesheet.DoubleTimeHours - expectedDoubleTimeHours);

            if (overtimeDiscrepancy > 0.01m || doubleTimeDiscrepancy > 0.01m)
            {
                var details = new
                {
                    ConsecutiveDays = consecutiveDays,
                    WorkDate = timesheet.WorkDate,
                    TotalHours = hoursOn7thDay,
                    ExpectedOvertimeHours = expectedOvertimeHours,
                    ActualOvertimeHours = timesheet.OvertimeHours,
                    ExpectedDoubleTimeHours = expectedDoubleTimeHours,
                    ActualDoubleTimeHours = timesheet.DoubleTimeHours
                };

                violations.Add(CreateViolation(
                    "Timesheet",
                    timesheet.Id,
                    $"California 7th consecutive day overtime not correctly classified. This is day {consecutiveDays} of consecutive work. Expected {expectedOvertimeHours:F2}h OT and {expectedDoubleTimeHours:F2}h DT.",
                    DefaultSeverity,
                    null,
                    JsonSerializer.Serialize(details)
                ));
            }
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Timesheet {timesheet.Id} complies with California 7th day overtime rules")
            : ComplianceResult.NonCompliant($"Timesheet {timesheet.Id} has {violations.Count} California 7th day overtime violations", violations);
    }

    private int CountConsecutiveDays(List<Timesheet> timesheets, DateTime targetDate)
    {
        var consecutiveDays = 0;
        var currentDate = targetDate;

        // Count backwards to find consecutive days
        for (int i = 0; i < 7; i++)
        {
            if (timesheets.Any(t => t.WorkDate.Date == currentDate.Date))
            {
                consecutiveDays++;
                currentDate = currentDate.AddDays(-1);
            }
            else
            {
                break;
            }
        }

        return consecutiveDays;
    }
}

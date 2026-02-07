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

using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.LaborLaw.Rules;

/// <summary>
/// California daily overtime rule: >8 hours = 1.5x, >12 hours = 2.0x
/// </summary>
public class CaliforniaDailyOvertimeRule : ComplianceRuleBase
{
    public override string RuleName => "California Daily Overtime";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.High;

    public CaliforniaDailyOvertimeRule(ILogger<CaliforniaDailyOvertimeRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not Timesheet timesheet)
        {
            throw new ArgumentException("Context must be a Timesheet", nameof(context));
        }

        LogEvaluationStart("Timesheet", timesheet.Id);

        var violations = new List<ComplianceViolation>();
        var dailyHours = timesheet.TotalHours;

        // Calculate expected overtime and double time
        decimal expectedOvertimeHours = 0;
        decimal expectedDoubleTimeHours = 0;

        if (dailyHours > ComplianceConstants.CaliforniaDailyDoubleTimeThreshold)
        {
            // Hours over 12 are double time
            expectedDoubleTimeHours = dailyHours - ComplianceConstants.CaliforniaDailyDoubleTimeThreshold;
            // Hours 8-12 are overtime
            expectedOvertimeHours = ComplianceConstants.CaliforniaDailyDoubleTimeThreshold - ComplianceConstants.CaliforniaDailyOvertimeThreshold;
        }
        else if (dailyHours > ComplianceConstants.CaliforniaDailyOvertimeThreshold)
        {
            // Hours 8+ are overtime
            expectedOvertimeHours = dailyHours - ComplianceConstants.CaliforniaDailyOvertimeThreshold;
        }

        // Check if overtime hours are correctly classified
        var overtimeDiscrepancy = Math.Abs(timesheet.OvertimeHours - expectedOvertimeHours);
        var doubleTimeDiscrepancy = Math.Abs(timesheet.DoubleTimeHours - expectedDoubleTimeHours);

        if (overtimeDiscrepancy > 0.01m || doubleTimeDiscrepancy > 0.01m)
        {
            var details = new
            {
                DailyHours = dailyHours,
                ExpectedOvertimeHours = expectedOvertimeHours,
                ActualOvertimeHours = timesheet.OvertimeHours,
                ExpectedDoubleTimeHours = expectedDoubleTimeHours,
                ActualDoubleTimeHours = timesheet.DoubleTimeHours,
                WorkDate = timesheet.WorkDate
            };

            violations.Add(CreateViolation(
                "Timesheet",
                timesheet.Id,
                $"California daily overtime not correctly classified. Expected {expectedOvertimeHours:F2}h OT and {expectedDoubleTimeHours:F2}h DT, but found {timesheet.OvertimeHours:F2}h OT and {timesheet.DoubleTimeHours:F2}h DT.",
                DefaultSeverity,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Timesheet {timesheet.Id} complies with California daily overtime rules")
            : ComplianceResult.NonCompliant($"Timesheet {timesheet.Id} has {violations.Count} California daily overtime violations", violations);
    }
}

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
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.LaborLaw.Rules;

/// <summary>
/// California rest break rule: 10 min paid break per 4 hours worked
/// </summary>
public class RestBreakRule : ComplianceRuleBase
{
    public override string RuleName => "California Rest Break";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Medium;

    private const int RestBreakMinutes = 10;
    private const decimal HoursPerRestBreak = 4.0m;

    public RestBreakRule(ILogger<RestBreakRule> logger) : base(logger)
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
        var workHours = timesheet.TotalHours;

        // Calculate required rest breaks based on hours worked
        // 3.5-6 hours = 1 break (10 min)
        // 6-10 hours = 2 breaks (20 min)
        // 10-14 hours = 3 breaks (30 min)
        int requiredRestBreaks = 0;

        if (workHours >= 3.5m && workHours < 6.0m)
        {
            requiredRestBreaks = 1;
        }
        else if (workHours >= 6.0m && workHours < 10.0m)
        {
            requiredRestBreaks = 2;
        }
        else if (workHours >= 10.0m)
        {
            requiredRestBreaks = 3;
        }

        int requiredRestMinutes = requiredRestBreaks * RestBreakMinutes;

        // Note: Rest breaks are PAID and should be included in work hours
        // This rule checks if adequate breaks were provided based on total hours
        // In practice, we would need additional tracking to verify rest breaks were actually given
        // For now, we'll create an informational violation if the shift length requires breaks

        if (requiredRestBreaks > 0)
        {
            var details = new
            {
                WorkHours = workHours,
                RequiredRestBreaks = requiredRestBreaks,
                RequiredRestMinutes = requiredRestMinutes,
                WorkDate = timesheet.WorkDate,
                Note = "Rest breaks are paid and should be provided during the work shift. Additional tracking needed to verify compliance."
            };

            // Create informational metadata for tracking
            var result = new ComplianceResult
            {
                Summary = $"Timesheet {timesheet.Id} requires {requiredRestBreaks} rest break(s) totaling {requiredRestMinutes} minutes",
                Metadata = new Dictionary<string, object>
                {
                    ["RequiredRestBreaks"] = requiredRestBreaks,
                    ["RequiredRestMinutes"] = requiredRestMinutes,
                    ["WorkHours"] = workHours
                }
            };

            LogEvaluationComplete(true, 0);
            return result;
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Timesheet {timesheet.Id} does not require rest breaks")
            : ComplianceResult.NonCompliant($"Timesheet {timesheet.Id} has {violations.Count} rest break violations", violations);
    }
}

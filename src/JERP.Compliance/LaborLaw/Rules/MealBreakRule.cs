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
/// California meal break rule: 30 min break required for shifts >5 hours, second break for shifts >10 hours
/// </summary>
public class MealBreakRule : ComplianceRuleBase
{
    public override string RuleName => "California Meal Break";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Medium;

    private const decimal FirstMealBreakThreshold = 5.0m;
    private const decimal SecondMealBreakThreshold = 10.0m;
    private const int RequiredMealBreakMinutes = 30;

    public MealBreakRule(ILogger<MealBreakRule> logger) : base(logger)
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
        var breakMinutes = timesheet.BreakMinutes;

        // Calculate required meal break minutes
        int requiredMealBreakMinutes = 0;
        if (workHours > SecondMealBreakThreshold)
        {
            requiredMealBreakMinutes = RequiredMealBreakMinutes * 2; // Two 30-minute breaks
        }
        else if (workHours > FirstMealBreakThreshold)
        {
            requiredMealBreakMinutes = RequiredMealBreakMinutes; // One 30-minute break
        }

        // Check if meal breaks were taken
        if (breakMinutes < requiredMealBreakMinutes)
        {
            var missingBreakMinutes = requiredMealBreakMinutes - breakMinutes;
            var details = new
            {
                WorkHours = workHours,
                ActualBreakMinutes = breakMinutes,
                RequiredBreakMinutes = requiredMealBreakMinutes,
                MissingBreakMinutes = missingBreakMinutes,
                WorkDate = timesheet.WorkDate
            };

            var description = workHours > SecondMealBreakThreshold
                ? $"Employee worked {workHours:F2} hours but only took {breakMinutes} minutes of meal breaks. Two 30-minute meal breaks required (60 minutes total)."
                : $"Employee worked {workHours:F2} hours but only took {breakMinutes} minutes of meal breaks. One 30-minute meal break required.";

            violations.Add(CreateViolation(
                "Timesheet",
                timesheet.Id,
                description,
                DefaultSeverity,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Timesheet {timesheet.Id} complies with California meal break rules")
            : ComplianceResult.NonCompliant($"Timesheet {timesheet.Id} has {violations.Count} meal break violations", violations);
    }
}

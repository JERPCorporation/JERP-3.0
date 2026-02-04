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

using JERP.Compliance.Core;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JERP.Compliance.LaborLaw.Rules;

/// <summary>
/// Minimum wage compliance rule: Validates against CA and Federal minimum wage
/// </summary>
public class MinimumWageRule : ComplianceRuleBase
{
    public override string RuleName => "Minimum Wage Compliance";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Critical;

    public MinimumWageRule(ILogger<MinimumWageRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not Employee employee)
        {
            throw new ArgumentException("Context must be an Employee", nameof(context));
        }

        LogEvaluationStart("Employee", employee.Id);

        var violations = new List<ComplianceViolation>();

        // Determine applicable minimum wage (use higher of state or federal)
        var applicableMinimumWage = Math.Max(ComplianceConstants.CaliforniaMinimumWage, ComplianceConstants.FederalMinimumWage);

        // Check hourly rate for non-exempt employees
        if (employee.HourlyRate.HasValue)
        {
            if (employee.HourlyRate.Value < applicableMinimumWage)
            {
                var details = new
                {
                    ActualHourlyRate = employee.HourlyRate.Value,
                    MinimumWageRequired = applicableMinimumWage,
                    CaliforniaMinimumWage = ComplianceConstants.CaliforniaMinimumWage,
                    FederalMinimumWage = ComplianceConstants.FederalMinimumWage,
                    Shortfall = applicableMinimumWage - employee.HourlyRate.Value
                };

                violations.Add(CreateViolation(
                    "Employee",
                    employee.Id,
                    $"Hourly rate ${employee.HourlyRate.Value:F2} is below the minimum wage of ${applicableMinimumWage:F2}. Employee: {employee.FirstName} {employee.LastName}",
                    ViolationSeverity.Critical,
                    null,
                    JsonSerializer.Serialize(details)
                ));
            }
        }
        // Check effective hourly rate for salaried employees
        else if (employee.SalaryAmount.HasValue)
        {
            // Calculate effective hourly rate based on standard 40-hour work week
            var weeklyPay = employee.PayFrequency switch
            {
                PayFrequency.Weekly => employee.SalaryAmount.Value,
                PayFrequency.BiWeekly => employee.SalaryAmount.Value / 2,
                PayFrequency.SemiMonthly => employee.SalaryAmount.Value * 12 / 26,
                PayFrequency.Monthly => employee.SalaryAmount.Value * 12 / 52,
                _ => 0
            };

            var effectiveHourlyRate = weeklyPay / 40m; // Standard 40-hour work week

            if (effectiveHourlyRate < applicableMinimumWage)
            {
                var details = new
                {
                    SalaryAmount = employee.SalaryAmount.Value,
                    PayFrequency = employee.PayFrequency.ToString(),
                    EffectiveHourlyRate = effectiveHourlyRate,
                    MinimumWageRequired = applicableMinimumWage,
                    Shortfall = applicableMinimumWage - effectiveHourlyRate
                };

                violations.Add(CreateViolation(
                    "Employee",
                    employee.Id,
                    $"Effective hourly rate ${effectiveHourlyRate:F2} (based on salary) is below the minimum wage of ${applicableMinimumWage:F2}. Employee: {employee.FirstName} {employee.LastName}",
                    ViolationSeverity.Critical,
                    null,
                    JsonSerializer.Serialize(details)
                ));
            }
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Employee {employee.Id} meets minimum wage requirements")
            : ComplianceResult.NonCompliant($"Employee {employee.Id} has {violations.Count} minimum wage violations", violations);
    }
}

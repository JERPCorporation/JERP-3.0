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
/// Child labor law compliance rule: Restricts work hours and conditions for employees under 18
/// </summary>
public class ChildLaborRule : ComplianceRuleBase
{
    public override string RuleName => "Child Labor Law Compliance";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Critical;

    private const decimal MaxDailyHoursForMinors = 8.0m;
    private const decimal MaxSchoolDayHours = 3.0m;
    private const decimal MaxWeeklyHoursSchoolWeek = 18.0m;
    private const decimal MaxWeeklyHoursNonSchoolWeek = 40.0m;
    private const int EarliestWorkTimeSchoolYear = 7; // 7 AM
    private const int LatestWorkTimeSchoolYear = 19; // 7 PM

    public ChildLaborRule(ILogger<ChildLaborRule> logger) : base(logger)
    {
    }

    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        if (context is not (Employee employee, Timesheet timesheet))
        {
            throw new ArgumentException("Context must be a tuple of (Employee, Timesheet)", nameof(context));
        }

        LogEvaluationStart("Employee", employee.Id);

        var violations = new List<ComplianceViolation>();

        // Check if employee is a minor
        if (!employee.DateOfBirth.HasValue)
        {
            return ComplianceResult.Compliant($"Employee {employee.Id} date of birth not provided, cannot verify minor status");
        }

        var age = CalculateAge(employee.DateOfBirth.Value, timesheet.WorkDate);
        if (age >= ComplianceConstants.MinorAge)
        {
            return ComplianceResult.Compliant($"Employee {employee.Id} is not a minor (age {age})");
        }

        // Check daily hours
        if (timesheet.TotalHours > MaxDailyHoursForMinors)
        {
            var details = new
            {
                Age = age,
                WorkDate = timesheet.WorkDate,
                ActualHours = timesheet.TotalHours,
                MaxAllowedHours = MaxDailyHoursForMinors,
                ExcessHours = timesheet.TotalHours - MaxDailyHoursForMinors
            };

            violations.Add(CreateViolation(
                "Timesheet",
                timesheet.Id,
                $"Minor employee (age {age}) worked {timesheet.TotalHours:F2} hours, exceeding the maximum of {MaxDailyHoursForMinors} hours per day.",
                ViolationSeverity.Critical,
                null,
                JsonSerializer.Serialize(details)
            ));
        }

        // Check time of day restrictions during school year (Sept 1 - June 15)
        if (IsSchoolYear(timesheet.WorkDate))
        {
            if (timesheet.ClockIn.HasValue)
            {
                var clockInHour = timesheet.ClockIn.Value.Hour;
                if (clockInHour < EarliestWorkTimeSchoolYear)
                {
                    var details = new
                    {
                        Age = age,
                        WorkDate = timesheet.WorkDate,
                        ClockInTime = timesheet.ClockIn.Value,
                        EarliestAllowedTime = $"{EarliestWorkTimeSchoolYear}:00 AM"
                    };

                    violations.Add(CreateViolation(
                        "Timesheet",
                        timesheet.Id,
                        $"Minor employee (age {age}) clocked in at {timesheet.ClockIn.Value:HH:mm}, before the earliest allowed time of {EarliestWorkTimeSchoolYear}:00 AM during school year.",
                        ViolationSeverity.Critical,
                        null,
                        JsonSerializer.Serialize(details)
                    ));
                }
            }

            if (timesheet.ClockOut.HasValue)
            {
                var clockOutHour = timesheet.ClockOut.Value.Hour;
                if (clockOutHour >= LatestWorkTimeSchoolYear)
                {
                    var details = new
                    {
                        Age = age,
                        WorkDate = timesheet.WorkDate,
                        ClockOutTime = timesheet.ClockOut.Value,
                        LatestAllowedTime = $"{LatestWorkTimeSchoolYear}:00 (7:00 PM)"
                    };

                    violations.Add(CreateViolation(
                        "Timesheet",
                        timesheet.Id,
                        $"Minor employee (age {age}) clocked out at {timesheet.ClockOut.Value:HH:mm}, after the latest allowed time of 7:00 PM during school year.",
                        ViolationSeverity.Critical,
                        null,
                        JsonSerializer.Serialize(details)
                    ));
                }
            }

            // Check if this is a school day (Monday-Friday during school year)
            if (IsSchoolDay(timesheet.WorkDate))
            {
                if (timesheet.TotalHours > MaxSchoolDayHours)
                {
                    var details = new
                    {
                        Age = age,
                        WorkDate = timesheet.WorkDate,
                        DayOfWeek = timesheet.WorkDate.DayOfWeek.ToString(),
                        ActualHours = timesheet.TotalHours,
                        MaxAllowedHours = MaxSchoolDayHours
                    };

                    violations.Add(CreateViolation(
                        "Timesheet",
                        timesheet.Id,
                        $"Minor employee (age {age}) worked {timesheet.TotalHours:F2} hours on a school day, exceeding the maximum of {MaxSchoolDayHours} hours.",
                        ViolationSeverity.Critical,
                        null,
                        JsonSerializer.Serialize(details)
                    ));
                }
            }
        }

        LogEvaluationComplete(violations.Count == 0, violations.Count);

        return violations.Count == 0
            ? ComplianceResult.Compliant($"Timesheet {timesheet.Id} for minor employee complies with child labor laws")
            : ComplianceResult.NonCompliant($"Timesheet {timesheet.Id} has {violations.Count} child labor law violations", violations);
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

    private bool IsSchoolYear(DateTime date)
    {
        // School year is typically Sept 1 - June 15
        var month = date.Month;
        return (month >= 9 && month <= 12) || (month >= 1 && month <= 6);
    }

    private bool IsSchoolDay(DateTime date)
    {
        // School days are Monday-Friday during school year
        var dayOfWeek = date.DayOfWeek;
        return dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday && IsSchoolYear(date);
    }
}

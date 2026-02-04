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

namespace JERP.Compliance;

/// <summary>
/// Constants for compliance rules
/// </summary>
/// <remarks>
/// These values should ideally be moved to a configuration system or database
/// to allow updates without code deployment
/// </remarks>
public static class ComplianceConstants
{
    /// <summary>
    /// California minimum wage for 2024
    /// </summary>
    public const decimal CaliforniaMinimumWage = 16.00m;

    /// <summary>
    /// Federal minimum wage
    /// </summary>
    public const decimal FederalMinimumWage = 7.25m;

    /// <summary>
    /// Social Security tax rate (FICA)
    /// </summary>
    public const decimal SocialSecurityRate = 0.062m;

    /// <summary>
    /// Social Security wage base limit for 2024
    /// </summary>
    public const decimal SocialSecurityWageBase2024 = 168600m;

    /// <summary>
    /// Medicare tax rate
    /// </summary>
    public const decimal MedicareRate = 0.0145m;

    /// <summary>
    /// Maximum deduction percentage (varies by jurisdiction)
    /// </summary>
    /// <remarks>
    /// This is a general guideline. Actual limits may vary by state and deduction type
    /// </remarks>
    public const decimal MaxDeductionPercent = 0.25m;

    /// <summary>
    /// California daily overtime threshold (hours)
    /// </summary>
    public const decimal CaliforniaDailyOvertimeThreshold = 8.0m;

    /// <summary>
    /// California daily double-time threshold (hours)
    /// </summary>
    public const decimal CaliforniaDailyDoubleTimeThreshold = 12.0m;

    /// <summary>
    /// Federal weekly overtime threshold (hours)
    /// </summary>
    public const decimal FederalWeeklyOvertimeThreshold = 40.0m;

    /// <summary>
    /// Overtime rate multiplier
    /// </summary>
    public const decimal OvertimeRate = 1.5m;

    /// <summary>
    /// Double-time rate multiplier
    /// </summary>
    public const decimal DoubleTimeRate = 2.0m;

    /// <summary>
    /// Minimum age for employment (federal standard)
    /// </summary>
    public const int MinimumWorkingAge = 14;

    /// <summary>
    /// Age of majority for labor purposes
    /// </summary>
    public const int MinorAge = 18;
}

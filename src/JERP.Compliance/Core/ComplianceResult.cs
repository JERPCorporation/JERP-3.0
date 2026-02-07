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

using JERP.Core.Entities;
using JERP.Core.Enums;

namespace JERP.Compliance.Core;

/// <summary>
/// Represents the result of a compliance rule evaluation
/// </summary>
public class ComplianceResult
{
    /// <summary>
    /// Indicates whether the evaluation is compliant (no violations)
    /// </summary>
    public bool IsCompliant => Violations.Count == 0;

    /// <summary>
    /// List of violations detected during evaluation
    /// </summary>
    public List<ComplianceViolation> Violations { get; set; } = new();

    /// <summary>
    /// Summary of the evaluation results
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Additional metadata about the evaluation
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Creates a compliant result with no violations
    /// </summary>
    public static ComplianceResult Compliant(string summary)
    {
        return new ComplianceResult
        {
            Summary = summary
        };
    }

    /// <summary>
    /// Creates a non-compliant result with violations
    /// </summary>
    public static ComplianceResult NonCompliant(string summary, List<ComplianceViolation> violations)
    {
        return new ComplianceResult
        {
            Summary = summary,
            Violations = violations
        };
    }
}

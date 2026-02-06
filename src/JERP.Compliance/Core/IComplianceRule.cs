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

using JERP.Core.Enums;

namespace JERP.Compliance.Core;

/// <summary>
/// Interface for compliance rules that can be evaluated
/// </summary>
public interface IComplianceRule
{
    /// <summary>
    /// Unique name of the compliance rule
    /// </summary>
    string RuleName { get; }

    /// <summary>
    /// Type of violation this rule checks for
    /// </summary>
    ViolationType ViolationType { get; }

    /// <summary>
    /// Default severity level for violations of this rule
    /// </summary>
    ViolationSeverity DefaultSeverity { get; }

    /// <summary>
    /// Evaluates the rule against the provided context
    /// </summary>
    /// <param name="context">Context object containing data to evaluate</param>
    /// <returns>Compliance result with any violations detected</returns>
    Task<ComplianceResult> EvaluateAsync(object context);
}

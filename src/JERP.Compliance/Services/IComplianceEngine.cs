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

namespace JERP.Compliance.Services;

/// <summary>
/// Interface for the compliance engine that evaluates rules and tracks violations
/// </summary>
public interface IComplianceEngine
{
    /// <summary>
    /// Registers a compliance rule with the engine
    /// </summary>
    Task RegisterRuleAsync<T>() where T : IComplianceRule;

    /// <summary>
    /// Evaluates all applicable rules for a specific employee
    /// </summary>
    /// <param name="employeeId">ID of the employee to evaluate</param>
    /// <returns>List of violations detected</returns>
    Task<List<ComplianceViolation>> EvaluateEmployeeAsync(Guid employeeId);

    /// <summary>
    /// Evaluates all applicable rules for a specific timesheet
    /// </summary>
    /// <param name="timesheetId">ID of the timesheet to evaluate</param>
    /// <returns>List of violations detected</returns>
    Task<List<ComplianceViolation>> EvaluateTimesheetAsync(Guid timesheetId);

    /// <summary>
    /// Evaluates all applicable rules for a specific payroll record
    /// </summary>
    /// <param name="payrollRecordId">ID of the payroll record to evaluate</param>
    /// <returns>List of violations detected</returns>
    Task<List<ComplianceViolation>> EvaluatePayrollAsync(Guid payrollRecordId);

    /// <summary>
    /// Evaluates all registered rules against all applicable entities
    /// </summary>
    /// <returns>List of all violations detected</returns>
    Task<List<ComplianceViolation>> EvaluateAllAsync();

    /// <summary>
    /// Calculates the overall compliance score for the system
    /// </summary>
    /// <returns>Compliance score as a percentage (0-100)</returns>
    Task<double> CalculateComplianceScoreAsync();

    /// <summary>
    /// Gets all active (unresolved) violations
    /// </summary>
    /// <returns>List of active violations</returns>
    Task<List<ComplianceViolation>> GetActiveViolationsAsync();
}

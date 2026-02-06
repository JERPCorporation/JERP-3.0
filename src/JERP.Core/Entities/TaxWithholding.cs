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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents tax withholding information for an employee
/// </summary>
public class TaxWithholding : BaseEntity
{
    /// <summary>
    /// Foreign key to the employee
    /// </summary>
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Tax year this withholding applies to
    /// </summary>
    [Required]
    public int TaxYear { get; set; }

    /// <summary>
    /// Filing status for tax purposes
    /// </summary>
    [Required]
    public FilingStatus FilingStatus { get; set; } = FilingStatus.Single;

    /// <summary>
    /// Number of federal allowances claimed
    /// </summary>
    public int FederalAllowances { get; set; } = 0;

    /// <summary>
    /// Additional federal tax to withhold per pay period
    /// </summary>
    public decimal FederalExtraWithholding { get; set; } = 0;

    /// <summary>
    /// Number of state allowances claimed
    /// </summary>
    public int StateAllowances { get; set; } = 0;

    /// <summary>
    /// Additional state tax to withhold per pay period
    /// </summary>
    public decimal StateExtraWithholding { get; set; } = 0;

    /// <summary>
    /// Indicates if employee is exempt from federal tax
    /// </summary>
    public bool IsExemptFederal { get; set; } = false;

    /// <summary>
    /// Indicates if employee is exempt from state tax
    /// </summary>
    public bool IsExemptState { get; set; } = false;

    /// <summary>
    /// Date this withholding becomes effective
    /// </summary>
    [Required]
    public DateTime EffectiveDate { get; set; }

    // Navigation properties
    public Employee Employee { get; set; } = null!;
}

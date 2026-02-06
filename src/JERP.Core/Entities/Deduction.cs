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
/// Represents a payroll deduction for an employee
/// </summary>
public class Deduction : BaseEntity
{
    /// <summary>
    /// Foreign key to the employee
    /// </summary>
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Type of deduction
    /// </summary>
    [Required]
    public DeductionType DeductionType { get; set; }

    /// <summary>
    /// Description of the deduction
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Deduction amount (either fixed or percentage)
    /// </summary>
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// Indicates if amount is a percentage of gross pay
    /// </summary>
    public bool IsPercentage { get; set; } = false;

    /// <summary>
    /// Indicates if deduction is taken pre-tax
    /// </summary>
    public bool IsPreTax { get; set; } = false;

    /// <summary>
    /// Processing priority (lower numbers processed first)
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Indicates if this deduction is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date this deduction starts
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date this deduction ends (if applicable)
    /// </summary>
    public DateTime? EndDate { get; set; }

    // Navigation properties
    public Employee Employee { get; set; } = null!;
}

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
/// Represents a timesheet entry for an employee's work hours
/// </summary>
public class Timesheet : BaseEntity
{
    /// <summary>
    /// Foreign key to the employee
    /// </summary>
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Date of work
    /// </summary>
    [Required]
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// Clock in timestamp
    /// </summary>
    public DateTime? ClockIn { get; set; }

    /// <summary>
    /// Clock out timestamp
    /// </summary>
    public DateTime? ClockOut { get; set; }

    /// <summary>
    /// Total break time in minutes
    /// </summary>
    public int BreakMinutes { get; set; } = 0;

    /// <summary>
    /// Total hours worked (including overtime)
    /// </summary>
    public decimal TotalHours { get; set; } = 0;

    /// <summary>
    /// Regular hours worked (up to 40/week)
    /// </summary>
    public decimal RegularHours { get; set; } = 0;

    /// <summary>
    /// Overtime hours worked (1.5x rate)
    /// </summary>
    public decimal OvertimeHours { get; set; } = 0;

    /// <summary>
    /// Double time hours worked (2x rate)
    /// </summary>
    public decimal DoubleTimeHours { get; set; } = 0;

    /// <summary>
    /// Current status of the timesheet
    /// </summary>
    [Required]
    public TimesheetStatus Status { get; set; } = TimesheetStatus.Draft;

    /// <summary>
    /// Additional notes or comments
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Timestamp when the timesheet was submitted
    /// </summary>
    public DateTime? SubmittedAt { get; set; }

    /// <summary>
    /// Timestamp when the timesheet was approved
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Foreign key to the user who approved the timesheet
    /// </summary>
    public Guid? ApprovedById { get; set; }

    // Navigation properties
    public Employee Employee { get; set; } = null!;
    public Employee? ApprovedBy { get; set; }
}

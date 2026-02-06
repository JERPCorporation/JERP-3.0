/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

namespace JERP.Application.DTOs.Timesheets;

/// <summary>
/// Timesheet entry data transfer object
/// </summary>
public class TimesheetEntryDto
{
    public Guid Id { get; set; }
    public Guid TimesheetId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public required string WorkType { get; set; } // Regular, Overtime, Sick, Vacation, Holiday
    public string? ProjectCode { get; set; }
    public string? TaskDescription { get; set; }
    public string? Notes { get; set; }
}

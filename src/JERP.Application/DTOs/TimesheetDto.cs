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

namespace JERP.Application.DTOs;

/// <summary>
/// Weekly timesheet data transfer object
/// </summary>
public class TimesheetDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public decimal TotalHours { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public TimesheetStatus Status { get; set; }
    public List<TimesheetEntryDto> Entries { get; set; } = new();
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Timesheet entry for a single day
/// </summary>
public class TimesheetEntryDto
{
    public Guid Id { get; set; }
    public Guid TimesheetId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public string? Description { get; set; }
    public Guid? ProjectId { get; set; }
    public string? ProjectName { get; set; }
}

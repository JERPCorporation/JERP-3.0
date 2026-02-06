/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Timesheets;

/// <summary>
/// Timesheet data transfer object
/// </summary>
public class TimesheetDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    // Old fields for services
    public DateTime WorkDate { get; set; }
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public int BreakMinutes { get; set; }
    public decimal DoubleTimeHours { get; set; }
    public TimesheetStatus StatusEnum { get; set; }
    // New fields for Desktop
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public string Status { get; set; } = string.Empty; // Draft, Submitted, Approved, Rejected
    public decimal TotalHours { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public List<TimesheetEntryDto> Entries { get; set; } = new();
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedById { get; set; }
    public string? ApproverName { get; set; }
    public string? RejectionReason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Formatted work date for display (single day timesheet)
    /// </summary>
    public string WorkDateDisplay => WorkDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Short work date format
    /// </summary>
    public string WorkDateShort => WorkDate.ToString("ddd, MMM dd");
    
    /// <summary>
    /// Formatted week start date for display
    /// </summary>
    public string WeekStartDateDisplay => WeekStartDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Formatted week period for display
    /// </summary>
    public string WeekPeriodDisplay => $"{WeekStartDate:MM/dd} - {WeekEndDate:MM/dd}";
    
    /// <summary>
    /// Formatted total hours for display
    /// </summary>
    public string TotalHoursDisplay => $"{TotalHours:F2} hrs";
    
    /// <summary>
    /// Formatted regular hours for display
    /// </summary>
    public string RegularHoursDisplay => $"{RegularHours:F2} hrs";
    
    /// <summary>
    /// Formatted overtime hours for display
    /// </summary>
    public string OvertimeHoursDisplay => $"{OvertimeHours:F2} hrs";
    
    /// <summary>
    /// Friendly status display
    /// </summary>
    public string StatusDisplay => StatusEnum switch
    {
        TimesheetStatus.Draft => "Draft",
        TimesheetStatus.Submitted => "Submitted",
        TimesheetStatus.Approved => "Approved",
        TimesheetStatus.Rejected => "Rejected",
        TimesheetStatus.Paid => "Paid",
        _ => Status
    };
    
    // Computed Properties
    /// <summary>
    /// Indicates if there is overtime
    /// </summary>
    public bool IsOvertime => OvertimeHours > 0;
    
    /// <summary>
    /// Overtime display for UI
    /// </summary>
    public string OvertimeDisplay => IsOvertime ? $"+{OvertimeHours:F2} OT" : "";
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => StatusEnum switch
    {
        TimesheetStatus.Draft => "📝",
        TimesheetStatus.Submitted => "📤",
        TimesheetStatus.Approved => "✅",
        TimesheetStatus.Rejected => "❌",
        TimesheetStatus.Paid => "💰",
        _ => ""
    };
}

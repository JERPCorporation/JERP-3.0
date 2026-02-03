namespace JERP.Application.DTOs.Timesheets;

/// <summary>
/// Request to update an existing timesheet
/// </summary>
public class TimesheetUpdateRequest
{
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public int BreakMinutes { get; set; }
    public string? Notes { get; set; }
}

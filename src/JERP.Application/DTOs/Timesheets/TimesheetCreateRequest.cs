namespace JERP.Application.DTOs.Timesheets;

/// <summary>
/// Request to create a new timesheet
/// </summary>
public class TimesheetCreateRequest
{
    public Guid EmployeeId { get; set; }
    public DateTime WorkDate { get; set; }
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public int BreakMinutes { get; set; }
    public string? Notes { get; set; }
}

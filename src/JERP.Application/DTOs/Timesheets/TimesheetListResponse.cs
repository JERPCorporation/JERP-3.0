namespace JERP.Application.DTOs.Timesheets;

/// <summary>
/// Paginated response containing timesheet list
/// </summary>
public class TimesheetListResponse
{
    public List<TimesheetDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

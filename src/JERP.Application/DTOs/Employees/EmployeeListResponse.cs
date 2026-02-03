namespace JERP.Application.DTOs.Employees;

/// <summary>
/// Paginated response containing employee list
/// </summary>
public class EmployeeListResponse
{
    public List<EmployeeDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

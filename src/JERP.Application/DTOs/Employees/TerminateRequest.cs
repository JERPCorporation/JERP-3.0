namespace JERP.Application.DTOs.Employees;

public class TerminateRequest
{
    public DateTime TerminationDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

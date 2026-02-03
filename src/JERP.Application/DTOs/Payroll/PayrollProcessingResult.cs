namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Result of payroll processing operation
/// </summary>
public class PayrollProcessingResult
{
    public int ProcessedCount { get; set; }
    public decimal TotalGrossPay { get; set; }
    public decimal TotalNetPay { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool IsSuccess => Errors.Count == 0;
}

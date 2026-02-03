using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Deduction data transfer object
/// </summary>
public class DeductionDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DeductionType DeductionType { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public bool IsPercentage { get; set; }
    public bool IsPreTax { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

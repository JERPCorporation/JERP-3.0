using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Request to create a new pay period
/// </summary>
public class CreatePayPeriodRequest
{
    public Guid CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayDate { get; set; }
    public PayFrequency Frequency { get; set; }
}

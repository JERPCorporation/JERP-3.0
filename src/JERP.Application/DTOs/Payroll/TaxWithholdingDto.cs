using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Tax withholding data transfer object
/// </summary>
public class TaxWithholdingDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int TaxYear { get; set; }
    public FilingStatus FilingStatus { get; set; }
    public int FederalAllowances { get; set; }
    public decimal FederalExtraWithholding { get; set; }
    public int StateAllowances { get; set; }
    public decimal StateExtraWithholding { get; set; }
    public bool IsExemptFederal { get; set; }
    public bool IsExemptState { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

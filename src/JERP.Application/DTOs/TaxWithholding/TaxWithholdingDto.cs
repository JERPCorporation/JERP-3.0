namespace JERP.Application.DTOs.TaxWithholding;

public class TaxWithholdingDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int TaxYear { get; set; }
    public string FilingStatus { get; set; } = string.Empty;
    public int FederalAllowances { get; set; }
    public decimal FederalExtraWithholding { get; set; }
    public int StateAllowances { get; set; }
    public decimal StateExtraWithholding { get; set; }
    public bool IsExemptFederal { get; set; }
    public bool IsExemptState { get; set; }
    public DateTime EffectiveDate { get; set; }
}

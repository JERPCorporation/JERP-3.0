namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Request for tax calculation
/// </summary>
public class TaxCalculationRequest
{
    public Guid EmployeeId { get; set; }
    public decimal GrossPay { get; set; }
    public int PayPeriods { get; set; }
    public decimal YTDGrossPay { get; set; }
    public decimal YTDFederalTax { get; set; }
    public decimal YTDStateTax { get; set; }
    public decimal YTDSocialSecurity { get; set; }
    public decimal YTDMedicare { get; set; }
}

/// <summary>
/// Result of tax calculation
/// </summary>
public class TaxCalculationResult
{
    public decimal FederalTax { get; set; }
    public decimal StateTax { get; set; }
    public decimal SocialSecurityTax { get; set; }
    public decimal MedicareTax { get; set; }
    public decimal TotalTaxes => FederalTax + StateTax + SocialSecurityTax + MedicareTax;
}

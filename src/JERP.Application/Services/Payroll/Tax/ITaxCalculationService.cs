namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Interface for tax calculation services
/// </summary>
public interface ITaxCalculationService
{
    /// <summary>
    /// Calculates federal, state, and payroll taxes for an employee's pay period
    /// </summary>
    Task<TaxCalculationResult> CalculateTaxesAsync(TaxCalculationRequest request);
}

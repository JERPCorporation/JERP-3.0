using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Implementation of tax calculation services with 2024 tax brackets
/// </summary>
public class TaxCalculationService : ITaxCalculationService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<TaxCalculationService> _logger;

    // 2024 Tax year constants
    private const decimal SocialSecurityRate = 0.062m; // 6.2%
    private const decimal SocialSecurityWageBase = 168600m;
    private const decimal MedicareRate = 0.0145m; // 1.45%
    private const decimal AdditionalMedicareRate = 0.009m; // 0.9%
    private const decimal AdditionalMedicareThreshold = 200000m;

    // 2024 Federal tax brackets (Single)
    private static readonly (decimal Limit, decimal Rate)[] FederalBracketsSingle = new[]
    {
        (11600m, 0.10m),
        (47150m, 0.12m),
        (100525m, 0.22m),
        (191950m, 0.24m),
        (243725m, 0.32m),
        (609350m, 0.35m),
        (decimal.MaxValue, 0.37m)
    };

    // 2024 Federal tax brackets (Married Filing Jointly)
    private static readonly (decimal Limit, decimal Rate)[] FederalBracketsMarried = new[]
    {
        (23200m, 0.10m),
        (94300m, 0.12m),
        (201050m, 0.22m),
        (383900m, 0.24m),
        (487450m, 0.32m),
        (731200m, 0.35m),
        (decimal.MaxValue, 0.37m)
    };

    // 2024 California state tax brackets (Single)
    private static readonly (decimal Limit, decimal Rate)[] CaliforniaBracketsSingle = new[]
    {
        (10412m, 0.01m),
        (24684m, 0.02m),
        (38959m, 0.04m),
        (54081m, 0.06m),
        (68350m, 0.08m),
        (349137m, 0.093m),
        (418961m, 0.103m),
        (698271m, 0.113m),
        (1000000m, 0.123m),
        (decimal.MaxValue, 0.133m)
    };

    // 2024 California state tax brackets (Married)
    private static readonly (decimal Limit, decimal Rate)[] CaliforniaBracketsMarried = new[]
    {
        (20824m, 0.01m),
        (49368m, 0.02m),
        (77918m, 0.04m),
        (108162m, 0.06m),
        (136700m, 0.08m),
        (698274m, 0.093m),
        (837922m, 0.103m),
        (1000000m, 0.113m),
        (1396542m, 0.123m),
        (decimal.MaxValue, 0.133m)
    };

    public TaxCalculationService(
        JerpDbContext context,
        ILogger<TaxCalculationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TaxCalculationResult> CalculateTaxesAsync(TaxCalculationRequest request)
    {
        _logger.LogInformation("Calculating taxes for employee: {EmployeeId}", request.EmployeeId);

        // Get employee tax withholding information
        var taxWithholding = await _context.TaxWithholdings
            .Where(t => t.EmployeeId == request.EmployeeId && t.TaxYear == DateTime.UtcNow.Year)
            .OrderByDescending(t => t.EffectiveDate)
            .FirstOrDefaultAsync();

        var filingStatus = taxWithholding?.FilingStatus ?? FilingStatus.Single;
        var federalExtraWithholding = taxWithholding?.FederalExtraWithholding ?? 0;
        var stateExtraWithholding = taxWithholding?.StateExtraWithholding ?? 0;
        var isExemptFederal = taxWithholding?.IsExemptFederal ?? false;
        var isExemptState = taxWithholding?.IsExemptState ?? false;

        // Annualize the gross pay for bracket calculations
        var annualizedGrossPay = request.GrossPay * request.PayPeriods;

        var result = new TaxCalculationResult();

        // Calculate Federal Income Tax
        if (!isExemptFederal)
        {
            var federalTax = CalculateFederalTax(annualizedGrossPay, filingStatus);
            result.FederalTax = Math.Round((federalTax / request.PayPeriods) + federalExtraWithholding, 2);
        }

        // Calculate California State Tax
        if (!isExemptState)
        {
            var stateTax = CalculateCaliforniaTax(annualizedGrossPay, filingStatus);
            result.StateTax = Math.Round((stateTax / request.PayPeriods) + stateExtraWithholding, 2);
        }

        // Calculate Social Security Tax (with wage base limit)
        var newYTDForSS = request.YTDGrossPay + request.GrossPay;
        if (request.YTDGrossPay < SocialSecurityWageBase)
        {
            var taxableAmount = Math.Min(request.GrossPay, SocialSecurityWageBase - request.YTDGrossPay);
            result.SocialSecurityTax = Math.Round(taxableAmount * SocialSecurityRate, 2);
        }

        // Calculate Medicare Tax (with additional Medicare tax for high earners)
        result.MedicareTax = Math.Round(request.GrossPay * MedicareRate, 2);
        
        // Additional Medicare Tax for earnings over threshold
        var newYTDForMedicare = request.YTDGrossPay + request.GrossPay;
        if (newYTDForMedicare > AdditionalMedicareThreshold)
        {
            var additionalTaxableAmount = request.YTDGrossPay >= AdditionalMedicareThreshold
                ? request.GrossPay
                : newYTDForMedicare - AdditionalMedicareThreshold;
            
            result.MedicareTax += Math.Round(additionalTaxableAmount * AdditionalMedicareRate, 2);
        }

        _logger.LogInformation("Tax calculation completed for employee: {EmployeeId}, Total: {Total}",
            request.EmployeeId, result.TotalTaxes);

        return result;
    }

    private decimal CalculateFederalTax(decimal annualIncome, FilingStatus filingStatus)
    {
        var brackets = filingStatus == FilingStatus.Married
            ? FederalBracketsMarried
            : FederalBracketsSingle;

        return CalculateProgressiveTax(annualIncome, brackets);
    }

    private decimal CalculateCaliforniaTax(decimal annualIncome, FilingStatus filingStatus)
    {
        var brackets = filingStatus == FilingStatus.Married
            ? CaliforniaBracketsMarried
            : CaliforniaBracketsSingle;

        return CalculateProgressiveTax(annualIncome, brackets);
    }

    private decimal CalculateProgressiveTax(decimal income, (decimal Limit, decimal Rate)[] brackets)
    {
        decimal tax = 0;
        decimal previousLimit = 0;

        foreach (var (limit, rate) in brackets)
        {
            if (income <= previousLimit)
            {
                break;
            }

            var taxableInThisBracket = Math.Min(income, limit) - previousLimit;
            tax += taxableInThisBracket * rate;

            previousLimit = limit;

            if (income <= limit)
            {
                break;
            }
        }

        return tax;
    }
}

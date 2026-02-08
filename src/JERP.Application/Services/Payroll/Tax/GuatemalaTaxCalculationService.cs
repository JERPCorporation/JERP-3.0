/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Guatemala tax calculation service implementing ISR (Decreto 10-2012),
/// IGSS (Acuerdo 1235), and Guatemalan labor law provisions.
/// </summary>
public class GuatemalaTaxCalculationService : ITaxCalculationService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<GuatemalaTaxCalculationService> _logger;

    // IGSS Rates (Acuerdo 1235)
    private const decimal IGSSEmployeeRate = 0.0483m;  // 4.83% - Laboral
    private const decimal IGSSEmployerRate = 0.1267m;  // 12.67% - Patronal

    // ISR Thresholds (Decreto 10-2012)
    private const decimal ISRExemptAmount = 48000m;        // Q48,000 annual exempt
    private const decimal ISRFirstBracketLimit = 300000m;  // Q300,000 first bracket
    private const decimal ISRFirstBracketRate = 0.05m;     // 5%
    private const decimal ISRSecondBracketRate = 0.07m;    // 7%
    private const decimal ISRFixedAmountOnExcess = 15000m; // Q15,000 fixed when exceeding Q300,000
    
    // Personal deduction allowance (IVA crédito fiscal)
    private const decimal MaxPersonalDeduction = 12000m;   // Q12,000 max annual

    public GuatemalaTaxCalculationService(
        JerpDbContext context,
        ILogger<GuatemalaTaxCalculationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TaxCalculationResult> CalculateTaxesAsync(TaxCalculationRequest request)
    {
        _logger.LogInformation("Calculating Guatemala taxes for employee: {EmployeeId}", request.EmployeeId);

        var result = new TaxCalculationResult();

        // Annualize gross pay for ISR bracket calculation
        var annualizedGrossPay = request.GrossPay * request.PayPeriods;

        // 1. Calculate ISR (maps to FederalTax in the result)
        var annualISR = CalculateISR(annualizedGrossPay);
        result.FederalTax = Math.Round(annualISR / request.PayPeriods, 2);

        // 2. Calculate IGSS Employee contribution (maps to SocialSecurityTax)
        result.SocialSecurityTax = Math.Round(request.GrossPay * IGSSEmployeeRate, 2);

        // 3. StateTax = 0 (Guatemala has no state/department-level income tax)
        result.StateTax = 0;

        // 4. MedicareTax = 0 (Guatemala doesn't have a separate Medicare-style tax;
        //    health coverage is included in IGSS)
        result.MedicareTax = 0;

        _logger.LogInformation(
            "Guatemala tax calculation completed for employee: {EmployeeId}. ISR: {ISR}, IGSS: {IGSS}, Total: {Total}",
            request.EmployeeId, result.FederalTax, result.SocialSecurityTax, result.TotalTaxes);

        return result;
    }

    /// <summary>
    /// Calculates ISR (Impuesto Sobre la Renta) per Decreto 10-2012
    /// Regime: Relación de dependencia (employees)
    /// </summary>
    private decimal CalculateISR(decimal annualGrossIncome)
    {
        // Subtract exempt amount
        var taxableIncome = annualGrossIncome - ISRExemptAmount;

        if (taxableIncome <= 0)
            return 0;

        // Subtract max personal deduction (IVA crédito)
        taxableIncome = Math.Max(0, taxableIncome - MaxPersonalDeduction);

        if (taxableIncome <= 0)
            return 0;

        if (taxableIncome <= ISRFirstBracketLimit)
        {
            // 5% on income up to Q300,000
            return taxableIncome * ISRFirstBracketRate;
        }
        else
        {
            // Q15,000 fixed + 7% on excess over Q300,000
            var excessOverLimit = taxableIncome - ISRFirstBracketLimit;
            return ISRFixedAmountOnExcess + (excessOverLimit * ISRSecondBracketRate);
        }
    }
}

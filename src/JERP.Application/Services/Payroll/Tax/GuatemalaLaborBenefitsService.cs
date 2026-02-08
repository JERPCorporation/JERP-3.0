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

namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Calculates Guatemala-specific labor benefits:
/// Bono 14 (Decreto 42-92), Aguinaldo (Decreto 76-78),
/// Indemnización (Código de Trabajo Art. 82), Vacaciones (Art. 130)
/// </summary>
public class GuatemalaLaborBenefitsService
{
    // Bono 14: July bonus (1 month salary / 12 months)
    private const decimal Bono14MonthlyProvision = 1.0m / 12.0m; // 8.33%
    
    // Aguinaldo: December bonus (1 month salary / 12 months)
    private const decimal AguinaldoMonthlyProvision = 1.0m / 12.0m; // 8.33%
    
    // Indemnización: 1 month per year (provision rate)
    private const decimal IndemnizacionMonthlyProvision = 1.0m / 12.0m; // 8.33%
    
    // Vacaciones: 15 days per year
    private const int VacationDaysPerYear = 15;

    /// <summary>
    /// Calculates the monthly provision amounts for Guatemala labor benefits.
    /// These are employer costs that should be provisioned monthly.
    /// </summary>
    public GuatemalaLaborBenefitsResult CalculateMonthlyProvisions(decimal monthlySalary)
    {
        return new GuatemalaLaborBenefitsResult
        {
            Bono14Provision = Math.Round(monthlySalary * Bono14MonthlyProvision, 2),
            AguinaldoProvision = Math.Round(monthlySalary * AguinaldoMonthlyProvision, 2),
            IndemnizacionProvision = Math.Round(monthlySalary * IndemnizacionMonthlyProvision, 2),
            VacacionesProvision = Math.Round(monthlySalary * (VacationDaysPerYear / 365.0m), 2),
            IGSSPatronalCost = Math.Round(monthlySalary * 0.1267m, 2) // 12.67% employer IGSS
        };
    }

    /// <summary>
    /// Calculates the actual Bono 14 payment for an employee.
    /// Period: July 1 (previous year) to June 30 (current year)
    /// </summary>
    public decimal CalculateBono14(decimal monthlySalary, int monthsWorkedInPeriod)
    {
        var months = Math.Min(monthsWorkedInPeriod, 12);
        return Math.Round(monthlySalary * months / 12.0m, 2);
    }

    /// <summary>
    /// Calculates the actual Aguinaldo payment for an employee.
    /// Period: December 1 (previous year) to November 30 (current year)
    /// </summary>
    public decimal CalculateAguinaldo(decimal monthlySalary, int monthsWorkedInPeriod)
    {
        var months = Math.Min(monthsWorkedInPeriod, 12);
        return Math.Round(monthlySalary * months / 12.0m, 2);
    }

    /// <summary>
    /// Calculates indemnización (severance) upon termination.
    /// 1 month salary per year worked, proportional for partial years.
    /// </summary>
    public decimal CalculateIndemnizacion(decimal monthlySalary, int totalMonthsWorked)
    {
        return Math.Round(monthlySalary * totalMonthsWorked / 12.0m, 2);
    }

    /// <summary>
    /// Calculates the total employer cost for a Guatemalan employee.
    /// Useful for showing "real cost" to business owners.
    /// </summary>
    public decimal CalculateTotalEmployerMonthlyCost(decimal monthlySalary)
    {
        var provisions = CalculateMonthlyProvisions(monthlySalary);
        return monthlySalary +
               provisions.IGSSPatronalCost +
               provisions.Bono14Provision +
               provisions.AguinaldoProvision +
               provisions.IndemnizacionProvision +
               provisions.VacacionesProvision;
    }
}

public class GuatemalaLaborBenefitsResult
{
    /// <summary>Bono 14 monthly provision (Decreto 42-92)</summary>
    public decimal Bono14Provision { get; set; }
    
    /// <summary>Aguinaldo monthly provision (Decreto 76-78)</summary>
    public decimal AguinaldoProvision { get; set; }
    
    /// <summary>Indemnización monthly provision (Código de Trabajo Art. 82)</summary>
    public decimal IndemnizacionProvision { get; set; }
    
    /// <summary>Vacaciones monthly provision (Código de Trabajo Art. 130)</summary>
    public decimal VacacionesProvision { get; set; }
    
    /// <summary>IGSS Patronal monthly cost (Acuerdo 1235) — 12.67%</summary>
    public decimal IGSSPatronalCost { get; set; }
    
    /// <summary>Total monthly employer provisions (all benefits + IGSS patronal)</summary>
    public decimal TotalMonthlyProvisions =>
        Bono14Provision + AguinaldoProvision + IndemnizacionProvision +
        VacacionesProvision + IGSSPatronalCost;
}

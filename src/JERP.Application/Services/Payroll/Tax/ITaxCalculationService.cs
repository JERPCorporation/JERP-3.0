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
/// Interface for tax calculation services
/// </summary>
public interface ITaxCalculationService
{
    /// <summary>
    /// Calculates federal, state, and payroll taxes for an employee's pay period
    /// </summary>
    Task<TaxCalculationResult> CalculateTaxesAsync(TaxCalculationRequest request);
}

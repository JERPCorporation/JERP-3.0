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

using JERP.Application.DTOs.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for cash flow forecasting
/// </summary>
public interface ICashFlowForecastService
{
    /// <summary>
    /// Generate cash flow forecast based on AR and AP due dates
    /// </summary>
    Task<CashFlowForecastDto> GenerateForecastAsync(Guid companyId, DateTime startDate, int days = 30);
}

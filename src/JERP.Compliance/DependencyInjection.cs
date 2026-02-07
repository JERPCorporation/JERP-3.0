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

using JERP.Compliance.Financial.Rules;
using JERP.Compliance.LaborLaw.Rules;
using JERP.Compliance.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JERP.Compliance;

/// <summary>
/// Extension methods for registering compliance services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all compliance services and rules with the service collection
    /// </summary>
    public static IServiceCollection AddComplianceServices(this IServiceCollection services)
    {
        // Register the compliance engine
        services.AddScoped<IComplianceEngine, ComplianceEngine>();

        // Register labor law rules
        services.AddScoped<CaliforniaDailyOvertimeRule>();
        services.AddScoped<CaliforniaSeventhDayOvertimeRule>();
        services.AddScoped<FederalWeeklyOvertimeRule>();
        services.AddScoped<MealBreakRule>();
        services.AddScoped<RestBreakRule>();
        services.AddScoped<MinimumWageRule>();
        services.AddScoped<ChildLaborRule>();

        // Register financial rules
        services.AddScoped<GAAPBalanceSheetRule>();
        services.AddScoped<GAAPRevenueRecognitionRule>();
        services.AddScoped<IFRSInventoryValuationRule>();

        return services;
    }
}

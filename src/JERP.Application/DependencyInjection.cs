/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using FluentValidation;
using JERP.Application.Authorization;
using JERP.Application.Services.Auth;
using JERP.Application.Services.Employees;
using JERP.Application.Services.Payroll;
using JERP.Application.Services.Payroll.Pdf;
using JERP.Application.Services.Payroll.Tax;
using JERP.Application.Services.PurchaseOrders;
using JERP.Application.Services.Security;
using JERP.Application.Services.Timesheets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace JERP.Application;

/// <summary>
/// Dependency injection configuration for application services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services to the service collection
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Configure QuestPDF Community License
        QuestPDF.Settings.License = LicenseType.Community;

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ITimesheetService, TimesheetService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<ITaxCalculationService, TaxCalculationService>();
        services.AddScoped<IPdfGenerationService, PdfGenerationService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        
        // Register purchase order services
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<IPOReceiptService, POReceiptService>();
        services.AddScoped<IVendorBillService, VendorBillService>();

        // Register authorization handlers
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Register FluentValidation validators from assembly containing this type
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}

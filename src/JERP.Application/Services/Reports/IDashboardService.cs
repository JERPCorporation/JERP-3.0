/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for dashboard and KPI services
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Gets comprehensive dashboard KPIs for a company
    /// </summary>
    Task<DashboardKPIDto> GetDashboardKPIsAsync(Guid companyId, DateTime? asOfDate = null);
    
    /// <summary>
    /// Gets alerts and notifications for a company
    /// </summary>
    Task<List<AlertDto>> GetAlertsAsync(Guid companyId);
}

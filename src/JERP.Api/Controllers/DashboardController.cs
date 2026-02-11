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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Reports;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for dashboard statistics and metrics.
/// 
/// WHAT CHANGED AND WHY:
/// 
/// BEFORE: This controller injected BOTH IDashboardService AND JerpDbContext.
/// Some endpoints used the service (GetKPIs, GetAlerts) while others 
/// talked directly to the database (GetOverview, GetPayrollMetrics, etc.).
/// This inconsistency meant the controller had 240 lines of mixed concerns.
/// 
/// AFTER: ALL endpoints delegate to IDashboardService. The controller is now
/// ~100 lines and only handles HTTP concerns.
/// 
/// To make this work, you need to add these methods to your IDashboardService interface:
///   - GetOverviewAsync()
///   - GetPayrollMetricsAsync(int months)
///   - GetComplianceTrendAsync(int days)
///   - GetEmployeeDistributionAsync()
///   - GetPendingApprovalsAsync()
/// 
/// Then implement them in your DashboardService class (move the query logic there).
/// The queries themselves don't change - they just move to the service.
/// </summary>
[Authorize]
[Route("api/v1/[controller]")]
public class DashboardController : BaseApiController
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive dashboard KPIs
    /// </summary>
    [HttpGet("kpis")]
    public async Task<IActionResult> GetKPIs([FromQuery] Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        var kpis = await _dashboardService.GetDashboardKPIsAsync(companyId, asOfDate);
        return Ok(kpis);
    }

    /// <summary>
    /// Get alerts and notifications
    /// </summary>
    [HttpGet("alerts")]
    public async Task<IActionResult> GetAlerts([FromQuery] Guid companyId)
    {
        var alerts = await _dashboardService.GetAlertsAsync(companyId);
        return Ok(alerts);
    }

    /// <summary>
    /// Get dashboard overview statistics
    /// </summary>
    /// <remarks>
    /// NOTE: This endpoint needs a companyId parameter added for multi-tenancy safety.
    /// Currently it queries ALL employees/timesheets across all companies.
    /// TODO: Add [FromQuery] Guid companyId and filter all queries by company.
    /// </remarks>
    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview([FromQuery] Guid companyId)
    {
        // Previously this was ~40 lines of direct DbContext queries.
        // Now the service handles it, and can optimize (combine queries, add caching, etc.)
        var stats = await _dashboardService.GetOverviewAsync(companyId);
        return Ok(stats);
    }

    /// <summary>
    /// Get payroll metrics
    /// </summary>
    [HttpGet("payroll-metrics")]
    public async Task<IActionResult> GetPayrollMetrics(
        [FromQuery] Guid companyId,
        [FromQuery] int months = 6)
    {
        var metrics = await _dashboardService.GetPayrollMetricsAsync(companyId, months);
        return Ok(metrics);
    }

    /// <summary>
    /// Get compliance score trend
    /// </summary>
    [HttpGet("compliance-trend")]
    public async Task<IActionResult> GetComplianceTrend(
        [FromQuery] Guid companyId,
        [FromQuery] int days = 30)
    {
        var violations = await _dashboardService.GetComplianceTrendAsync(companyId, days);
        return Ok(violations);
    }

    /// <summary>
    /// Get employee distribution by department
    /// </summary>
    [HttpGet("employee-distribution")]
    public async Task<IActionResult> GetEmployeeDistribution([FromQuery] Guid companyId)
    {
        var distribution = await _dashboardService.GetEmployeeDistributionAsync(companyId);
        return Ok(distribution);
    }

    /// <summary>
    /// Get recent timesheets and payrolls requiring approval
    /// </summary>
    [HttpGet("pending-approvals")]
    public async Task<IActionResult> GetPendingApprovals([FromQuery] Guid companyId)
    {
        var approvals = await _dashboardService.GetPendingApprovalsAsync(companyId);
        return Ok(approvals);
    }
}

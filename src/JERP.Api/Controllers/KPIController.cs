/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Reports;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KPIController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<KPIController> _logger;

    public KPIController(
        IDashboardService dashboardService,
        ILogger<KPIController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get KPIs for a company — delegates to DashboardService (real GL data)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetKPIs([FromQuery] Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        _logger.LogInformation("KPI data requested for company {CompanyId}", companyId);

        var kpis = await _dashboardService.GetDashboardKPIsAsync(companyId, asOfDate);

        return Ok(kpis);
    }

    /// <summary>
    /// Get dashboard KPIs — same data, convenience alias
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardKPIs([FromQuery] Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        _logger.LogInformation("Dashboard KPI data requested for company {CompanyId}", companyId);

        var kpis = await _dashboardService.GetDashboardKPIsAsync(companyId, asOfDate);

        return Ok(kpis);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Reports;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KPIController : ControllerBase
{
    private readonly ILogger<KPIController> _logger;
    private readonly IDashboardService _dashboardService;

    public KPIController(
        ILogger<KPIController> logger,
        IDashboardService dashboardService)
    {
        _logger = logger;
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetKPIs([FromQuery] Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        _logger.LogInformation("KPI data requested for company {CompanyId}", companyId);
        
        var kpis = await _dashboardService.GetDashboardKPIsAsync(companyId, asOfDate);
        
        return Ok(kpis);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardKPIs([FromQuery] Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        _logger.LogInformation("Dashboard KPI data requested for company {CompanyId}", companyId);
        
        var kpis = await _dashboardService.GetDashboardKPIsAsync(companyId, asOfDate);
        
        return Ok(kpis);
    }
}

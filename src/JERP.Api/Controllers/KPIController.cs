using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KPIController : ControllerBase
{
    private readonly ILogger<KPIController> _logger;

    public KPIController(ILogger<KPIController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetKPIs()
    {
        _logger.LogInformation("KPI data requested");
        
        return Ok(new
        {
            revenue = new
            {
                current = 150000.00,
                previous = 120000.00,
                percentageChange = 25.0
            },
            customers = new
            {
                total = 450,
                new_this_month = 25,
                percentageChange = 5.9
            },
            orders = new
            {
                total = 320,
                pending = 15,
                completed = 305
            },
            timestamp = DateTime.UtcNow,
            message = "Mock KPI data - Implementation pending"
        });
    }

    [HttpGet("dashboard")]
    public IActionResult GetDashboardKPIs()
    {
        _logger.LogInformation("Dashboard KPI data requested");
        
        return Ok(new
        {
            salesThisMonth = 45000.00,
            ordersThisMonth = 89,
            activeUsers = 12,
            inventoryValue = 234500.00,
            timestamp = DateTime.UtcNow,
            message = "Mock dashboard data - Implementation pending"
        });
    }
}

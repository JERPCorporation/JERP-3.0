/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Health check endpoint for monitoring and load balancers.
/// 
/// FIXES:
/// 1. Route changed from "api/[controller]" (which resolves to /api/Health) 
///    to "api/v1/health" to match what your monitoring calls (see log: GET /api/v1/health → 404)
/// 2. Removed personal email and developer name - health endpoints are often public
///    and should not expose personal information
/// 3. Version now reads from assembly metadata instead of being hardcoded
/// 
/// FUTURE IMPROVEMENT: Replace this with ASP.NET Core's built-in health check system:
///   builder.Services.AddHealthChecks().AddSqlServer(connectionString);
///   app.MapHealthChecks("/api/v1/health");
/// This would actually test database connectivity instead of just returning "Healthy".
/// </summary>
[ApiController]
[Route("api/v1/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = typeof(HealthController).Assembly.GetName().Version?.ToString() ?? "3.0.0"
        });
    }
}

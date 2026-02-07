using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LicenseController : ControllerBase
{
    private readonly ILogger<LicenseController> _logger;

    public LicenseController(ILogger<LicenseController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetLicense()
    {
        _logger.LogInformation("License information requested");
        
        return Ok(new
        {
            licenseType = "Trial",
            expiryDate = DateTime.UtcNow.AddDays(14),
            isActive = true,
            message = "License endpoint - Implementation pending"
        });
    }

    [HttpPost("validate")]
    public IActionResult ValidateLicense([FromBody] ValidateLicenseRequest request)
    {
        _logger.LogInformation("License validation requested for key: {LicenseKey}", request.LicenseKey);
        
        return Ok(new
        {
            isValid = false,
            message = "License validation - Implementation pending"
        });
    }
}

public class ValidateLicenseRequest
{
    public string LicenseKey { get; set; } = string.Empty;
}

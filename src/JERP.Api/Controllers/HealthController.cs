using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            developer = "Julio Cesar Mendez Tobar Jr.",
            contact = "ichbincesartobar@yahoo.com"
        });
    }
}

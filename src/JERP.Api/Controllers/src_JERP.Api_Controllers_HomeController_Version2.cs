using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new
        {
            name = "JERP 3.0 API",
            version = "1.0.0",
            developer = "Julio Cesar Mendez Tobar Jr.",
            contact = "ichbincesartobar@yahoo.com",
            timestamp = DateTime.UtcNow
        });
    }
}
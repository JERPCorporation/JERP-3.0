using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Base controller providing common API functionality
/// </summary>
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Returns a successful response with data
    /// </summary>
    protected IActionResult Ok<T>(T data)
    {
        return base.Ok(new { success = true, data });
    }

    /// <summary>
    /// Returns a created response with data
    /// </summary>
    protected IActionResult Created<T>(T data)
    {
        return StatusCode(201, new { success = true, data });
    }

    /// <summary>
    /// Returns a bad request response with error message
    /// </summary>
    protected IActionResult BadRequest(string message)
    {
        return base.BadRequest(new { success = false, error = message });
    }

    /// <summary>
    /// Returns a not found response with error message
    /// </summary>
    protected new IActionResult NotFound(string message)
    {
        return base.NotFound(new { success = false, error = message });
    }
}

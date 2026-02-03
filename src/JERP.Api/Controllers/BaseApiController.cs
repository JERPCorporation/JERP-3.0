using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Base controller providing common API functionality
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult Success<T>(T data, string? message = null)
    {
        return Ok(new ApiResponse<T>
        {
            Success = true,
            Message = message ?? "Operation completed successfully",
            Data = data
        });
    }

    protected IActionResult Error(string message, int statusCode = 400)
    {
        return StatusCode(statusCode, new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null
        });
    }

    protected string? GetCurrentUserId()
    {
        return User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }

    protected string? GetCurrentUsername()
    {
        return User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
    }

    // Legacy methods for backward compatibility
    protected IActionResult Ok<T>(T data)
    {
        return base.Ok(new { success = true, data });
    }

    protected IActionResult Created<T>(T data)
    {
        return StatusCode(201, new { success = true, data });
    }

    protected IActionResult BadRequest(string message)
    {
        return base.BadRequest(new { success = false, error = message });
    }

    protected IActionResult NotFound(string message)
    {
        return base.NotFound(new { success = false, error = message });
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

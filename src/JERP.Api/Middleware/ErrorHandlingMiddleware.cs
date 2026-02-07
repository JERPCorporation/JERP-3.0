using System.Net;
using System.Text.Json;

namespace JERP.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        
        var result = JsonSerializer.Serialize(new
        {
            error = "An error occurred while processing your request",
            message = _environment.IsDevelopment() ? exception.Message : "Please contact support if the problem persists",
            timestamp = DateTime.UtcNow
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        
        return context.Response.WriteAsync(result);
    }
}

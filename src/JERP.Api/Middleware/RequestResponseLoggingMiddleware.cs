/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Diagnostics;
using System.Text;

namespace JERP.Api.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private static readonly HashSet<string> SensitiveEndpoints = new()
    {
        "/api/v1/auth/login",
        "/api/v1/auth/refresh"
    };

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        // Log request
        var isSensitive = SensitiveEndpoints.Any(endpoint => 
            request.Path.StartsWithSegments(endpoint));

        if (!isSensitive)
        {
            _logger.LogInformation(
                "HTTP {Method} {Path}{QueryString} from {IpAddress}",
                request.Method,
                request.Path,
                request.QueryString,
                context.Connection.RemoteIpAddress);
        }
        else
        {
            _logger.LogInformation(
                "HTTP {Method} {Path} from {IpAddress} [Sensitive]",
                request.Method,
                request.Path,
                context.Connection.RemoteIpAddress);
        }

        // Continue processing
        await _next(context);

        // Log response
        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;
        var logLevel = statusCode >= 500 ? LogLevel.Error :
                       statusCode >= 400 ? LogLevel.Warning :
                       LogLevel.Information;

        _logger.Log(
            logLevel,
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
            request.Method,
            request.Path,
            statusCode,
            stopwatch.ElapsedMilliseconds);
    }
}

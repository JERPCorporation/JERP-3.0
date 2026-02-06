/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JERP.Core.Entities;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JERP.Api.Middleware;

/// <summary>
/// Middleware for auditing state-changing operations
/// </summary>
public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditLoggingMiddleware> _logger;

    public AuditLoggingMiddleware(
        RequestDelegate next,
        ILogger<AuditLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, JerpDbContext dbContext)
    {
        var request = context.Request;

        // Only audit state-changing operations
        if (IsAuditableRequest(request.Method))
        {
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                // Only log successful operations
                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    await LogAuditAsync(context, dbContext);
                }
            }
            finally
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
        }
        else
        {
            await _next(context);
        }
    }

    private bool IsAuditableRequest(string method)
    {
        return method == HttpMethods.Post ||
               method == HttpMethods.Put ||
               method == HttpMethods.Delete ||
               method == HttpMethods.Patch;
    }

    private async Task LogAuditAsync(HttpContext context, JerpDbContext dbContext)
    {
        try
        {
            var userId = GetUserId(context);
            if (userId == null)
            {
                return; // Don't log if user is not authenticated
            }

            var entityInfo = ExtractEntityInfo(context.Request.Path);

            var auditLog = new AuditLog
            {
                UserId = userId.Value,
                Action = context.Request.Method,
                EntityType = entityInfo.EntityType,
                EntityId = entityInfo.EntityId ?? Guid.Empty,
                Timestamp = DateTime.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                OldValues = null, // Would need to capture before state
                NewValues = null  // Would need to capture after state
            };

            // Get the last audit log to create hash chain
            var lastAuditLog = await dbContext.AuditLogs
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            auditLog.PreviousHash = lastAuditLog?.CurrentHash ?? string.Empty;
            auditLog.CurrentHash = CalculateHash(auditLog);

            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Audit log created: User {UserId} performed {Action} on {EntityType} {EntityId}",
                userId,
                auditLog.Action,
                auditLog.EntityType,
                auditLog.EntityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log");
            // Don't throw - audit logging failure should not break the request
        }
    }

    private Guid? GetUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst("userId") ?? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    private (string EntityType, Guid? EntityId) ExtractEntityInfo(PathString path)
    {
        var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        
        if (segments.Length < 3)
        {
            return ("Unknown", null);
        }

        var entityType = segments[2]; // After "api/v1/"
        var entityId = segments.Length > 3 && Guid.TryParse(segments[3], out var id) ? id : (Guid?)null;

        return (entityType, entityId);
    }

    private string CalculateHash(AuditLog log)
    {
        var data = $"{log.UserId}|{log.Action}|{log.EntityType}|{log.EntityId}|" +
                   $"{log.Timestamp:O}|{log.PreviousHash}|{log.OldValues}|{log.NewValues}";

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hashBytes);
    }
}

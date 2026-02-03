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
            var entityInfo = ExtractEntityInfo(context.Request.Path);

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = context.Request.Method,
                EntityType = entityInfo.EntityType,
                EntityId = entityInfo.EntityId,
                Timestamp = DateTime.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                OldValues = null, // Would need to capture before state
                NewValues = null, // Would need to capture after state
                Changes = $"{context.Request.Method} {context.Request.Path}"
            };

            // Get the last audit log to create hash chain
            var lastAuditLog = await dbContext.AuditLogs
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            auditLog.PreviousHash = lastAuditLog?.Hash ?? string.Empty;
            auditLog.Hash = CalculateHash(auditLog);

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

    private int? GetUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst("userId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    private (string EntityType, string? EntityId) ExtractEntityInfo(PathString path)
    {
        var segments = path.Value?.Split('/', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        
        if (segments.Length < 3)
        {
            return ("Unknown", null);
        }

        var entityType = segments[2]; // After "api/v1/"
        var entityId = segments.Length > 3 && int.TryParse(segments[3], out _) ? segments[3] : null;

        return (entityType, entityId);
    }

    private string CalculateHash(AuditLog log)
    {
        var data = $"{log.UserId}|{log.Action}|{log.EntityType}|{log.EntityId}|" +
                   $"{log.Timestamp:O}|{log.PreviousHash}|{log.Changes}";

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hashBytes);
    }
}

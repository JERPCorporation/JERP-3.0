/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using JERP.Application.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for admin operations including audit log management.
/// 
/// SECURITY FIX: Changed from [Authorize] to [Authorize(Roles = "Admin,SuperAdmin")]
/// 
/// BEFORE: Any authenticated user (employees, basic users) could access audit logs,
/// verify audit chains, and export sensitive compliance data.
/// 
/// AFTER: Only users with the Admin or SuperAdmin role can access these endpoints.
/// The [Authorize(Roles = "...")] attribute checks the role claims in the JWT token.
/// If a user's token doesn't contain one of these roles, they get a 403 Forbidden response.
/// 
/// HOW IT WORKS:
/// When a user logs in, your AuthService creates a JWT token with claims like:
///   { "role": "Admin" } or { "role": "Employee" }
/// The [Authorize(Roles = "Admin")] attribute checks for that claim automatically.
/// You need to make sure your AuthService includes role claims in the token.
/// </summary>
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminController : BaseApiController
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IAuditLogService auditLogService,
        ILogger<AdminController> logger)
    {
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get audit logs for a company with optional filtering
    /// </summary>
    /// <param name="companyId">Company ID</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="action">Optional action type filter</param>
    /// <param name="userId">Optional user ID filter</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 50, max: 100)</param>
    /// <returns>Paginated audit log entries</returns>
    [HttpGet("audit-log")]
    public async Task<IActionResult> GetAuditLog(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? action = null,
        [FromQuery] Guid? userId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1)
            return BadRequest("Page number must be greater than 0");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100");

        var (logs, total) = await _auditLogService.GetAuditLog(
            companyId, startDate, endDate, action, userId, page, pageSize);

        return Success(new
        {
            logs = logs.Select(log => new
            {
                id = log.Id,
                companyId = log.CompanyId,
                sequenceNumber = log.SequenceNumber,
                timestamp = log.Timestamp,
                userId = log.UserId,
                userEmail = log.UserEmail,
                userName = log.UserName,
                action = log.Action,
                resource = log.Resource ?? log.EntityType,
                details = log.Details,
                ipAddress = log.IpAddress,
                previousHash = log.PreviousHash,
                currentHash = log.CurrentHash
            }),
            pagination = new
            {
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            }
        });
    }

    /// <summary>
    /// Verify the integrity of the audit chain for a company
    /// </summary>
    /// <param name="request">Verification request containing company ID</param>
    /// <returns>Verification result with any broken chain links</returns>
    [HttpPost("audit-log/verify")]
    public async Task<IActionResult> VerifyAuditChain([FromBody] VerifyAuditChainRequest request)
    {
        var (isValid, message, brokenLinks) = await _auditLogService.VerifyAuditChain(request.CompanyId);

        return Success(new
        {
            isValid,
            message,
            brokenLinks,
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Export audit logs to CSV format
    /// </summary>
    /// <param name="companyId">Company ID</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>CSV file download</returns>
    [HttpGet("audit-log/export")]
    public async Task<IActionResult> ExportAuditLog(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var csv = await _auditLogService.ExportToCsv(companyId, startDate, endDate);

        var fileName = $"audit-log-{companyId}-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

        return File(
            System.Text.Encoding.UTF8.GetBytes(csv),
            "text/csv",
            fileName);
    }
}

/// <summary>
/// Request model for verifying audit chain
/// </summary>
public class VerifyAuditChainRequest
{
    /// <summary>
    /// Company ID to verify audit chain for
    /// </summary>
    public Guid CompanyId { get; set; }
}

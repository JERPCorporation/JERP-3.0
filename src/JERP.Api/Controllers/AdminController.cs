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

using JERP.Application.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for admin operations including audit log management
/// TODO: Add role-based authorization [Authorize(Roles = "Admin,SuperAdmin")] or 
/// policy-based authorization [Authorize(Policy = "AdminOnly")] to restrict access
/// </summary>
[Authorize]
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
        try
        {
            if (page < 1)
            {
                return BadRequest("Page number must be greater than 0");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100");
            }

            var (logs, total) = await _auditLogService.GetAuditLog(
                companyId,
                startDate,
                endDate,
                action,
                userId,
                page,
                pageSize);

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs for company {CompanyId}", companyId);
            return Error("Failed to retrieve audit logs", 500);
        }
    }

    /// <summary>
    /// Verify the integrity of the audit chain for a company
    /// </summary>
    /// <param name="request">Verification request containing company ID</param>
    /// <returns>Verification result with any broken chain links</returns>
    [HttpPost("audit-log/verify")]
    public async Task<IActionResult> VerifyAuditChain([FromBody] VerifyAuditChainRequest request)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying audit chain for company {CompanyId}", request.CompanyId);
            return Error("Failed to verify audit chain", 500);
        }
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
        try
        {
            var csv = await _auditLogService.ExportToCsv(companyId, startDate, endDate);

            var fileName = $"audit-log-{companyId}-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

            return File(
                System.Text.Encoding.UTF8.GetBytes(csv),
                "text/csv",
                fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting audit logs for company {CompanyId}", companyId);
            return Error("Failed to export audit logs", 500);
        }
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

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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.AuditLog;

namespace JERP.Api.Controllers;

/// <summary>
/// Audit log management endpoints with hash-chain integrity verification
/// </summary>
[Route("api/v1/audit-log")]
[Authorize]
public class AuditController : BaseApiController
{
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<AuditController> _logger;

    public AuditController(IAuditLogService auditLogService, ILogger<AuditController> logger)
    {
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get audit log entries for a company
    /// </summary>
    /// <param name="companyId">Company ID to retrieve audit logs for</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="action">Optional action filter</param>
    /// <param name="limit">Optional limit on number of results</param>
    [HttpGet]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? action = null,
        [FromQuery] int? limit = null)
    {
        try
        {
            var logs = await _auditLogService.GetAuditLogsAsync(companyId, startDate, endDate, action, limit);
            return Ok(logs);
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
    /// <param name="companyId">Company ID to verify audit chain for</param>
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyAuditChain([FromQuery] Guid companyId)
    {
        try
        {
            var (isValid, errorMessage, totalEntries, firstInvalidSequence) = 
                await _auditLogService.VerifyAuditChainAsync(companyId);

            var result = new
            {
                isValid,
                errorMessage,
                totalEntries,
                firstInvalidSequence,
                verifiedAt = DateTime.UtcNow
            };

            if (!isValid)
            {
                _logger.LogWarning("Audit chain verification failed for company {CompanyId}: {ErrorMessage}", 
                    companyId, errorMessage);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying audit chain for company {CompanyId}", companyId);
            return Error("Failed to verify audit chain", 500);
        }
    }

    /// <summary>
    /// Export audit log to CSV
    /// </summary>
    /// <param name="companyId">Company ID to export audit logs for</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAuditLog(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var csvData = await _auditLogService.ExportToCsvAsync(companyId, startDate, endDate);
            var fileName = $"audit-log-{companyId}-{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

            return File(csvData, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting audit log for company {CompanyId}", companyId);
            return Error("Failed to export audit log", 500);
        }
    }
}

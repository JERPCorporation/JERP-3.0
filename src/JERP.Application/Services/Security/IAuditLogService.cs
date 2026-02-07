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

using JERP.Core.Entities;

namespace JERP.Application.Services.Security;

/// <summary>
/// Service interface for audit logging with hash chain integrity
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Logs an action with hash chain verification
    /// </summary>
    /// <param name="companyId">ID of the company</param>
    /// <param name="userId">ID of the user performing the action</param>
    /// <param name="userEmail">Email of the user</param>
    /// <param name="userName">Name of the user</param>
    /// <param name="action">Action type (e.g., "payroll_processed")</param>
    /// <param name="resource">Resource type being affected</param>
    /// <param name="details">Descriptive details about the action</param>
    /// <param name="ipAddress">IP address of the user</param>
    /// <returns>The created audit log entry</returns>
    Task<AuditLog> LogAction(
        Guid companyId,
        Guid userId,
        string userEmail,
        string userName,
        string action,
        string resource,
        string details,
        string? ipAddress = null);

    /// <summary>
    /// Retrieves audit logs for a company with optional filtering
    /// </summary>
    /// <param name="companyId">ID of the company</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <param name="action">Optional action filter</param>
    /// <param name="userId">Optional user filter</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <returns>Paginated audit logs</returns>
    Task<(IEnumerable<AuditLog> Logs, int Total)> GetAuditLog(
        Guid companyId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? action = null,
        Guid? userId = null,
        int page = 1,
        int pageSize = 50);

    /// <summary>
    /// Verifies the integrity of the audit chain for a company
    /// </summary>
    /// <param name="companyId">ID of the company</param>
    /// <returns>Verification result with details of any broken links</returns>
    Task<(bool IsValid, string Message, List<string> BrokenLinks)> VerifyAuditChain(Guid companyId);

    /// <summary>
    /// Exports audit logs to CSV format
    /// </summary>
    /// <param name="companyId">ID of the company</param>
    /// <param name="startDate">Optional start date filter</param>
    /// <param name="endDate">Optional end date filter</param>
    /// <returns>CSV content as string</returns>
    Task<string> ExportToCsv(Guid companyId, DateTime? startDate = null, DateTime? endDate = null);
}

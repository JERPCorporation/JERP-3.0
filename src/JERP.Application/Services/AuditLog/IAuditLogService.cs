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

using JERP.Core.Entities;

namespace JERP.Application.Services.AuditLog;

/// <summary>
/// Service for managing hash-chained audit log entries
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Logs an action to the audit trail with hash-chain integrity
    /// </summary>
    Task<Core.Entities.AuditLog> LogActionAsync(
        Guid companyId,
        Guid userId,
        string userEmail,
        string action,
        string resource,
        string details,
        string ipAddress);

    /// <summary>
    /// Verifies the integrity of the entire audit chain for a company
    /// </summary>
    Task<(bool IsValid, string? ErrorMessage, int TotalEntries, int? FirstInvalidSequence)> VerifyAuditChainAsync(Guid companyId);

    /// <summary>
    /// Gets audit log entries for a company with optional filtering
    /// </summary>
    Task<IEnumerable<Core.Entities.AuditLog>> GetAuditLogsAsync(
        Guid companyId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? action = null,
        int? limit = null);

    /// <summary>
    /// Exports audit log to CSV format
    /// </summary>
    Task<byte[]> ExportToCsvAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null);
}

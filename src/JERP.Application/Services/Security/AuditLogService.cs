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
using JERP.Core.Entities;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JERP.Application.Services.Security;

/// <summary>
/// Service for audit logging with blockchain-style hash chain integrity
/// </summary>
public class AuditLogService : IAuditLogService
{
    private readonly JerpDbContext _context;
    private static readonly object _lockObject = new object();

    public AuditLogService(JerpDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<AuditLog> LogAction(
        Guid companyId,
        Guid userId,
        string userEmail,
        string userName,
        string action,
        string resource,
        string details,
        string? ipAddress = null)
    {
        // Note: Using synchronous lock for simplicity. In production with multiple instances,
        // consider using distributed locking (e.g., Redis) or database-level optimistic concurrency
        AuditLog auditLog;
        
        lock (_lockObject)
        {
            // Get the last audit log entry for this company
            var lastEntry = _context.AuditLogs
                .Where(a => a.CompanyId == companyId)
                .OrderByDescending(a => a.SequenceNumber)
                .FirstOrDefault();

            var sequenceNumber = (lastEntry?.SequenceNumber ?? 0) + 1;
            var previousHash = lastEntry?.CurrentHash ?? "GENESIS";

            // Create the new audit log entry
            auditLog = new AuditLog
            {
                CompanyId = companyId,
                UserId = userId,
                UserEmail = userEmail,
                UserName = userName,
                Action = action,
                EntityType = resource,
                Resource = resource,
                EntityId = Guid.Empty, // Default, can be set later if needed
                Details = details,
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow,
                PreviousHash = previousHash,
                SequenceNumber = sequenceNumber
            };

            // Calculate the current hash
            auditLog.CurrentHash = CalculateHash(auditLog);

            _context.AuditLogs.Add(auditLog);
        }
        
        // Save asynchronously outside the lock
        await _context.SaveChangesAsync();

        return auditLog;
    }

    /// <inheritdoc/>
    public async Task<(IEnumerable<AuditLog> Logs, int Total)> GetAuditLog(
        Guid companyId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? action = null,
        Guid? userId = null,
        int page = 1,
        int pageSize = 50)
    {
        var query = _context.AuditLogs
            .Where(a => a.CompanyId == companyId);

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Timestamp >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Timestamp <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(action))
        {
            query = query.Where(a => a.Action == action);
        }

        if (userId.HasValue)
        {
            query = query.Where(a => a.UserId == userId.Value);
        }

        var total = await query.CountAsync();

        var logs = await query
            .OrderBy(a => a.SequenceNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.User)
            .ToListAsync();

        return (logs, total);
    }

    /// <inheritdoc/>
    public async Task<(bool IsValid, string Message, List<string> BrokenLinks)> VerifyAuditChain(Guid companyId)
    {
        var logs = await _context.AuditLogs
            .Where(a => a.CompanyId == companyId)
            .OrderBy(a => a.SequenceNumber)
            .ToListAsync();

        if (!logs.Any())
        {
            return (true, "No audit logs found for this company.", new List<string>());
        }

        var brokenLinks = new List<string>();

        // Check first entry
        var firstLog = logs.First();
        if (firstLog.PreviousHash != "GENESIS")
        {
            brokenLinks.Add($"First entry (Seq {firstLog.SequenceNumber}) should have PreviousHash='GENESIS' but has '{firstLog.PreviousHash}'");
        }

        // Verify hash of first entry
        var calculatedHash = CalculateHash(firstLog);
        if (calculatedHash != firstLog.CurrentHash)
        {
            brokenLinks.Add($"Entry Seq {firstLog.SequenceNumber}: Hash mismatch. Expected {calculatedHash}, got {firstLog.CurrentHash}");
        }

        // Check subsequent entries
        for (int i = 1; i < logs.Count; i++)
        {
            var currentLog = logs[i];
            var previousLog = logs[i - 1];

            // Verify hash calculation
            calculatedHash = CalculateHash(currentLog);
            if (calculatedHash != currentLog.CurrentHash)
            {
                brokenLinks.Add($"Entry Seq {currentLog.SequenceNumber}: Hash mismatch. Expected {calculatedHash}, got {currentLog.CurrentHash}");
            }

            // Verify chain link
            if (currentLog.PreviousHash != previousLog.CurrentHash)
            {
                brokenLinks.Add($"Entry Seq {currentLog.SequenceNumber}: Chain broken. PreviousHash '{currentLog.PreviousHash}' does not match previous entry's CurrentHash '{previousLog.CurrentHash}'");
            }
        }

        if (brokenLinks.Any())
        {
            return (false, $"Chain verification failed with {brokenLinks.Count} broken link(s).", brokenLinks);
        }

        return (true, $"Chain verified successfully. All {logs.Count} entries are valid.", new List<string>());
    }

    /// <inheritdoc/>
    public async Task<string> ExportToCsv(Guid companyId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.AuditLogs
            .Where(a => a.CompanyId == companyId);

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Timestamp >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Timestamp <= endDate.Value);
        }

        var logs = await query
            .OrderBy(a => a.SequenceNumber)
            .Include(a => a.User)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("SequenceNumber,Timestamp,UserId,UserEmail,UserName,Action,Resource,Details,IpAddress,PreviousHash,CurrentHash");

        foreach (var log in logs)
        {
            csv.AppendLine($"{log.SequenceNumber}," +
                          $"\"{log.Timestamp:yyyy-MM-dd HH:mm:ss}\"," +
                          $"{log.UserId}," +
                          $"\"{EscapeCsv(log.UserEmail)}\"," +
                          $"\"{EscapeCsv(log.UserName)}\"," +
                          $"\"{EscapeCsv(log.Action)}\"," +
                          $"\"{EscapeCsv(log.Resource)}\"," +
                          $"\"{EscapeCsv(log.Details)}\"," +
                          $"\"{EscapeCsv(log.IpAddress)}\"," +
                          $"\"{log.PreviousHash}\"," +
                          $"\"{log.CurrentHash}\"");
        }

        return csv.ToString();
    }

    /// <summary>
    /// Calculates SHA256 hash for an audit log entry
    /// Format: CompanyId|Timestamp|UserId|Action|Resource|Details|IpAddress|PreviousHash|SequenceNumber
    /// Returns first 12 characters of the hash as specified in requirements
    /// Note: 12-char hash provides 48-bit space (2^48 ≈ 281 trillion combinations).
    /// This is sufficient for most use cases but may have collision risk in very large systems.
    /// For enhanced security, consider extending to 16-20 characters.
    /// </summary>
    private string CalculateHash(AuditLog log)
    {
        var data = $"{log.CompanyId}|" +
                   $"{log.Timestamp:O}|" +
                   $"{log.UserId}|" +
                   $"{log.Action}|" +
                   $"{log.Resource ?? log.EntityType}|" +
                   $"{log.Details}|" +
                   $"{log.IpAddress}|" +
                   $"{log.PreviousHash}|" +
                   $"{log.SequenceNumber}";

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        var fullHash = Convert.ToHexString(hashBytes);
        
        // Return first 12 characters as specified in requirements
        return fullHash.Substring(0, 12);
    }

    /// <summary>
    /// Escapes special characters for CSV format
    /// </summary>
    private string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        // Escape double quotes by doubling them
        return value.Replace("\"", "\"\"");
    }
}

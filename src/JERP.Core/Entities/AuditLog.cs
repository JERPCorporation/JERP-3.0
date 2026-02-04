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

using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities;

/// <summary>
/// Represents an immutable audit log entry for tracking system changes with hash-chain integrity
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Foreign key to the company this audit entry belongs to
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Timestamp when the action occurred
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Foreign key to the user who performed the action
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Email of the user who performed the action (cached for reporting)
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Action performed (e.g., "payroll_approved", "employee_created")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Resource that was affected (e.g., "Payroll Period 2024-01-01")
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Additional details about the action
    /// </summary>
    [MaxLength(2000)]
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// IP address of the user who performed the action
    /// </summary>
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Hash of the previous audit log entry for integrity verification (blockchain-style)
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string PreviousHash { get; set; } = string.Empty;

    /// <summary>
    /// Hash of this audit log entry for integrity verification
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string CurrentHash { get; set; } = string.Empty;

    /// <summary>
    /// Sequence number for ordering within a company's audit chain
    /// </summary>
    [Required]
    public long SequenceNumber { get; set; }

    // Legacy fields - kept for backward compatibility
    /// <summary>
    /// Type of entity that was modified (optional, for backward compatibility)
    /// </summary>
    [MaxLength(100)]
    public string? EntityType { get; set; }

    /// <summary>
    /// ID of the entity that was modified (optional, for backward compatibility)
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// JSON representation of values before the change (optional, for backward compatibility)
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// JSON representation of values after the change (optional, for backward compatibility)
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// User agent string from the client (optional)
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public User User { get; set; } = null!;
}

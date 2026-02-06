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

using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities;

/// <summary>
/// Represents an immutable audit log entry for tracking system changes
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Foreign key to the company this audit log entry belongs to
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the user who performed the action
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Email address of the user who performed the action
    /// </summary>
    [MaxLength(255)]
    public string? UserEmail { get; set; }

    /// <summary>
    /// Name of the user who performed the action
    /// </summary>
    [MaxLength(200)]
    public string? UserName { get; set; }

    /// <summary>
    /// Action performed (e.g., "Create", "Update", "Delete")
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Type of entity that was modified (also referred to as Resource)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Resource type (alias for EntityType for compatibility)
    /// </summary>
    [MaxLength(100)]
    public string? Resource { get; set; }

    /// <summary>
    /// ID of the entity that was modified
    /// </summary>
    [Required]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Descriptive details about the action performed
    /// </summary>
    [MaxLength(1000)]
    public string? Details { get; set; }

    /// <summary>
    /// JSON representation of values before the change
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// JSON representation of values after the change
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// IP address of the user who performed the action
    /// </summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent string from the client
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Timestamp when the action occurred
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Hash of the previous audit log entry for integrity verification
    /// </summary>
    [MaxLength(100)]
    public string? PreviousHash { get; set; }

    /// <summary>
    /// Hash of this audit log entry for integrity verification
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string CurrentHash { get; set; } = string.Empty;

    /// <summary>
    /// Sequential number for ordering audit log entries within a company
    /// </summary>
    [Required]
    public long SequenceNumber { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public User User { get; set; } = null!;
}

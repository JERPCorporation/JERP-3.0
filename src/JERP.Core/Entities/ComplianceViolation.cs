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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a compliance violation detected in the system
/// </summary>
public class ComplianceViolation : BaseEntity
{
    /// <summary>
    /// Type of violation
    /// </summary>
    [Required]
    public ViolationType ViolationType { get; set; }

    /// <summary>
    /// Severity level of the violation
    /// </summary>
    [Required]
    public ViolationSeverity Severity { get; set; }

    /// <summary>
    /// Current status of the violation
    /// </summary>
    [Required]
    public ViolationStatus Status { get; set; } = ViolationStatus.Open;

    /// <summary>
    /// Type of entity where the violation was detected
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// ID of the entity where the violation was detected
    /// </summary>
    [Required]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Name of the rule that was violated
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of the violation
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional details in JSON format
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Financial impact of the violation
    /// </summary>
    public decimal? FinancialImpact { get; set; }

    /// <summary>
    /// Timestamp when the violation was detected
    /// </summary>
    [Required]
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the violation was resolved
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// Foreign key to the user who resolved the violation
    /// </summary>
    public Guid? ResolvedById { get; set; }

    /// <summary>
    /// Notes about the resolution
    /// </summary>
    [MaxLength(1000)]
    public string? ResolutionNotes { get; set; }

    // Navigation properties
    public Employee? ResolvedBy { get; set; }
}

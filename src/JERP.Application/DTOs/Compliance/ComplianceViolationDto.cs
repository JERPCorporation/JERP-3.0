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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance violation data transfer object
/// </summary>
public class ComplianceViolationDto
{
    public Guid Id { get; set; }
    public ViolationType ViolationType { get; set; }
    public ViolationSeverity Severity { get; set; }
    public ViolationStatus Status { get; set; }
    public required string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public required string RuleName { get; set; }
    public required string Description { get; set; }
    public string? Details { get; set; }
    public decimal? FinancialImpact { get; set; }
    public DateTime DetectedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedById { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

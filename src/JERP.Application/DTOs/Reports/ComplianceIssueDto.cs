/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class ComplianceIssueDto
{
    public Guid IssueId { get; set; }
    
    public string IssueType { get; set; } = string.Empty;
    
    public string Severity { get; set; } = string.Empty; // Critical, High, Medium, Low
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime IdentifiedDate { get; set; }
    
    public string Status { get; set; } = string.Empty; // Open, Resolved, InProgress
    
    public string? Resolution { get; set; }
    
    public DateTime? ResolvedDate { get; set; }
}

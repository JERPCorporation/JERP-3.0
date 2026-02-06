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

namespace JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance report data transfer object
/// </summary>
public class ComplianceReportDto
{
    public required string CompanyName { get; set; }
    public DateTime ReportDate { get; set; }
    public ComplianceScoreDto Score { get; set; } = new();
    public List<ComplianceViolationDto> Violations { get; set; } = new();
}

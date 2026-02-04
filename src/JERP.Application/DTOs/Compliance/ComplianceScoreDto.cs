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

namespace JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance score summary
/// </summary>
public class ComplianceScoreDto
{
    public decimal Score { get; set; }
    public int TotalChecks { get; set; }
    public int TotalViolations { get; set; }
    public Dictionary<string, int> ViolationsByCategory { get; set; } = new();
}

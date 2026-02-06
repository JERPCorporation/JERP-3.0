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

namespace JERP.Application.DTOs.Statistics;

/// <summary>
/// Employee statistics data transfer object
/// </summary>
public class EmployeeStatsDto
{
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int TotalCount { get; set; }
    public int OnLeaveCount { get; set; }
}

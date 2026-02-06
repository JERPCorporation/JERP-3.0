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

namespace JERP.Application.DTOs;

/// <summary>
/// Dashboard data transfer object with aggregated metrics
/// </summary>
public class DashboardDto
{
    public ComplianceMetricsDto ComplianceMetrics { get; set; } = new();
    public PayrollMetricsDto PayrollMetrics { get; set; } = new();
    public FinanceMetricsDto FinanceMetrics { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

/// <summary>
/// Compliance metrics for dashboard
/// </summary>
public class ComplianceMetricsDto
{
    public int TotalViolations { get; set; }
    public int CriticalViolations { get; set; }
    public int ActiveViolations { get; set; }
    public int ResolvedViolations { get; set; }
    public decimal ComplianceScore { get; set; }
}

/// <summary>
/// Payroll metrics for dashboard
/// </summary>
public class PayrollMetricsDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public decimal CurrentPayrollAmount { get; set; }
    public int PendingTimesheets { get; set; }
}

/// <summary>
/// Finance metrics for dashboard
/// </summary>
public class FinanceMetricsDto
{
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
}

/// <summary>
/// Recent activity item for dashboard
/// </summary>
public class RecentActivityDto
{
    public Guid Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Username { get; set; } = string.Empty;
}

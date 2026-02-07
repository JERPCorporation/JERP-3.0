/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

namespace JERP.Application.DTOs.Dashboard;

/// <summary>
/// Dashboard overview data transfer object
/// </summary>
public class DashboardOverviewDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    public int OpenInvoicesCount { get; set; }
    public int UnpaidBillsCount { get; set; }
    public decimal TotalReceivables { get; set; }
    public decimal TotalPayables { get; set; }
    public decimal CashBalance { get; set; }
    public int ActiveEmployeesCount { get; set; }
    public int PendingTimesheetsCount { get; set; }
    
    // Alias properties for backward compatibility
    public int CriticalViolations { get; set; }
    public int ActiveEmployees => ActiveEmployeesCount;
    public int PendingTimesheets => PendingTimesheetsCount;
}

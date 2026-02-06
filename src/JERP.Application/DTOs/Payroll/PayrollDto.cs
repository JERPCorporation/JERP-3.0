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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Payroll processing data transfer object
/// </summary>
public class PayrollDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid PayPeriodId { get; set; }
    public string PayPeriodName { get; set; } = string.Empty;
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public DateTime PayDate { get; set; }
    public PayrollStatus Status { get; set; }
    public int TotalEmployees { get; set; }
    public decimal TotalGrossPay { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalNetPay { get; set; }
    public List<PayrollLineDto> Lines { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Formatted pay period display
    /// </summary>
    public string PayPeriodDisplay => $"{PayPeriodStart:MM/dd} - {PayPeriodEnd:MM/dd}";
    
    /// <summary>
    /// Formatted pay date for display
    /// </summary>
    public string PayDateDisplay => PayDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Formatted total gross pay for display
    /// </summary>
    public string TotalGrossPayDisplay => TotalGrossPay.ToString("C2");
    
    /// <summary>
    /// Formatted total deductions for display
    /// </summary>
    public string TotalDeductionsDisplay => TotalDeductions.ToString("C2");
    
    /// <summary>
    /// Formatted total net pay for display
    /// </summary>
    public string TotalNetPayDisplay => TotalNetPay.ToString("C2");
    
    /// <summary>
    /// Friendly status display
    /// </summary>
    public string StatusDisplay => Status switch
    {
        PayrollStatus.Draft => "Draft",
        PayrollStatus.Calculated => "Calculated",
        PayrollStatus.Submitted => "Submitted",
        PayrollStatus.Approved => "Approved",
        PayrollStatus.Paid => "Paid",
        PayrollStatus.Rejected => "Rejected",
        _ => "Unknown"
    };
    
    // Computed Properties
    /// <summary>
    /// Deduction rate as percentage of gross pay
    /// </summary>
    public decimal DeductionRate => TotalGrossPay > 0 ? (TotalDeductions / TotalGrossPay) * 100 : 0;
    
    /// <summary>
    /// Formatted deduction rate for display
    /// </summary>
    public string DeductionRateDisplay => $"{DeductionRate:F1}%";
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => Status switch
    {
        PayrollStatus.Draft => "📝",
        PayrollStatus.Calculated => "🔢",
        PayrollStatus.Submitted => "📤",
        PayrollStatus.Approved => "✅",
        PayrollStatus.Paid => "💰",
        PayrollStatus.Rejected => "❌",
        _ => ""
    };
}

/// <summary>
/// Payroll line item for individual employee
/// </summary>
public class PayrollLineDto
{
    public Guid Id { get; set; }
    public Guid PayrollId { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public decimal GrossPay { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
}

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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a payroll record for an employee in a specific pay period
/// </summary>
public class PayrollRecord : BaseEntity
{
    /// <summary>
    /// Foreign key to the pay period
    /// </summary>
    [Required]
    public Guid PayPeriodId { get; set; }

    /// <summary>
    /// Foreign key to the employee
    /// </summary>
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Current status of the payroll record
    /// </summary>
    [Required]
    public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

    // Hours
    /// <summary>
    /// Regular hours worked
    /// </summary>
    public decimal RegularHours { get; set; } = 0;

    /// <summary>
    /// Overtime hours worked (1.5x rate)
    /// </summary>
    public decimal OvertimeHours { get; set; } = 0;

    /// <summary>
    /// Double time hours worked (2x rate)
    /// </summary>
    public decimal DoubleTimeHours { get; set; } = 0;

    // Earnings
    /// <summary>
    /// Total gross pay before taxes and deductions
    /// </summary>
    public decimal GrossPay { get; set; } = 0;

    /// <summary>
    /// Pay for regular hours
    /// </summary>
    public decimal RegularPay { get; set; } = 0;

    /// <summary>
    /// Pay for overtime hours
    /// </summary>
    public decimal OvertimePay { get; set; } = 0;

    /// <summary>
    /// Pay for double time hours
    /// </summary>
    public decimal DoubleTimePay { get; set; } = 0;

    /// <summary>
    /// Additional bonus payment
    /// </summary>
    public decimal BonusPay { get; set; } = 0;

    /// <summary>
    /// Commission payment
    /// </summary>
    public decimal CommissionPay { get; set; } = 0;

    // Taxes
    /// <summary>
    /// Federal income tax withheld
    /// </summary>
    public decimal FederalTax { get; set; } = 0;

    /// <summary>
    /// State income tax withheld
    /// </summary>
    public decimal StateTax { get; set; } = 0;

    /// <summary>
    /// Social Security tax withheld
    /// </summary>
    public decimal SocialSecurityTax { get; set; } = 0;

    /// <summary>
    /// Medicare tax withheld
    /// </summary>
    public decimal MedicareTax { get; set; } = 0;

    /// <summary>
    /// Total taxes withheld
    /// </summary>
    public decimal TotalTaxes { get; set; } = 0;

    // Deductions
    /// <summary>
    /// Total pre-tax deductions
    /// </summary>
    public decimal PreTaxDeductions { get; set; } = 0;

    /// <summary>
    /// Total post-tax deductions
    /// </summary>
    public decimal PostTaxDeductions { get; set; } = 0;

    /// <summary>
    /// Total deductions
    /// </summary>
    public decimal TotalDeductions { get; set; } = 0;

    // Net Pay
    /// <summary>
    /// Net pay after taxes and deductions
    /// </summary>
    public decimal NetPay { get; set; } = 0;

    // Year-to-Date Totals
    /// <summary>
    /// Year-to-date gross pay
    /// </summary>
    public decimal YTDGrossPay { get; set; } = 0;

    /// <summary>
    /// Year-to-date federal tax
    /// </summary>
    public decimal YTDFederalTax { get; set; } = 0;

    /// <summary>
    /// Year-to-date state tax
    /// </summary>
    public decimal YTDStateTax { get; set; } = 0;

    /// <summary>
    /// Year-to-date Social Security tax
    /// </summary>
    public decimal YTDSocialSecurity { get; set; } = 0;

    /// <summary>
    /// Year-to-date Medicare tax
    /// </summary>
    public decimal YTDMedicare { get; set; } = 0;

    /// <summary>
    /// Year-to-date net pay
    /// </summary>
    public decimal YTDNetPay { get; set; } = 0;

    // Timestamps
    /// <summary>
    /// Timestamp when the payroll was calculated
    /// </summary>
    public DateTime? CalculatedAt { get; set; }

    /// <summary>
    /// Timestamp when the payroll was approved
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    // Navigation properties
    public PayPeriod PayPeriod { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public ICollection<PayrollRecordDetail> Details { get; set; } = new List<PayrollRecordDetail>();
}

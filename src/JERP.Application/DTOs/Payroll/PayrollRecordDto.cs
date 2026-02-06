/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Payroll record data transfer object
/// </summary>
public class PayrollRecordDto
{
    public Guid Id { get; set; }
    public Guid PayPeriodId { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public PayrollStatus Status { get; set; }
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal DoubleTimeHours { get; set; }
    public decimal RegularPay { get; set; }
    public decimal OvertimePay { get; set; }
    public decimal DoubleTimePay { get; set; }
    public decimal BonusPay { get; set; }
    public decimal CommissionPay { get; set; }
    public decimal GrossPay { get; set; }
    public decimal FederalTax { get; set; }
    public decimal StateTax { get; set; }
    public decimal SocialSecurityTax { get; set; }
    public decimal MedicareTax { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal PreTaxDeductions { get; set; }
    public decimal PostTaxDeductions { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }
    public decimal YTDGrossPay { get; set; }
    public decimal YTDFederalTax { get; set; }
    public decimal YTDStateTax { get; set; }
    public decimal YTDSocialSecurity { get; set; }
    public decimal YTDMedicare { get; set; }
    public decimal YTDNetPay { get; set; }
    public DateTime? CalculatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }
    public List<PayrollRecordDetailDto> Details { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Payroll record detail line item
/// </summary>
public class PayrollRecordDetailDto
{
    public Guid Id { get; set; }
    public PayrollRecordDetailType Type { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public bool IsYTD { get; set; }
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Payroll;

/// <summary>
/// Pay period data transfer object
/// </summary>
public class PayPeriodDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayDate { get; set; }
    public PayFrequency PayFrequency { get; set; }
    public PayFrequency Frequency { get; set; } // Alias for PayFrequency for backward compatibility
    public PayrollStatus StatusEnum { get; set; }
    public string Status { get; set; } = string.Empty; // Draft, Processing, Approved, Paid, Closed
    public int EmployeeCount { get; set; }
    public decimal TotalGrossPay { get; set; }
    public decimal TotalNetPay { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal TotalDeductions { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessedById { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

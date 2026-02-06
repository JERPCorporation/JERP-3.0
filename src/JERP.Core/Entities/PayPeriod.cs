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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a pay period for processing payroll
/// </summary>
public class PayPeriod : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Start date of the pay period
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the pay period
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Date employees will be paid
    /// </summary>
    [Required]
    public DateTime PayDate { get; set; }

    /// <summary>
    /// Current status of the pay period
    /// </summary>
    [Required]
    public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

    /// <summary>
    /// Pay frequency for this period
    /// </summary>
    [Required]
    public PayFrequency Frequency { get; set; }

    /// <summary>
    /// Total gross pay for all employees in this period
    /// </summary>
    public decimal TotalGrossPay { get; set; } = 0;

    /// <summary>
    /// Total net pay for all employees in this period
    /// </summary>
    public decimal TotalNetPay { get; set; } = 0;

    /// <summary>
    /// Total taxes withheld in this period
    /// </summary>
    public decimal TotalTaxes { get; set; } = 0;

    /// <summary>
    /// Total deductions in this period
    /// </summary>
    public decimal TotalDeductions { get; set; } = 0;

    /// <summary>
    /// Timestamp when the payroll was processed
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Timestamp when the payroll was approved
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Foreign key to the user who approved the payroll
    /// </summary>
    public Guid? ApprovedById { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
    public Employee? ApprovedBy { get; set; }
}

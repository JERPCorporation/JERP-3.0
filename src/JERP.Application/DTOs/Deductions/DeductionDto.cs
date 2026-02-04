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

namespace JERP.Application.DTOs.Deductions;

public class DeductionDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DeductionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsPreTax { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

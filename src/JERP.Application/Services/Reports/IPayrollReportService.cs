/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for generating payroll reports
/// </summary>
public interface IPayrollReportService
{
    /// <summary>
    /// Generates a payroll summary report
    /// </summary>
    Task<PayrollSummaryReportDto> GeneratePayrollSummaryAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports payroll summary to PDF
    /// </summary>
    Task<byte[]> ExportPayrollSummaryToPdfAsync(ReportRequestDto request);
    
    /// <summary>
    /// Generates Form 941 report for a quarter
    /// </summary>
    Task<byte[]> Generate941ReportAsync(Guid companyId, int quarter, int year);
    
    /// <summary>
    /// Generates W-2 forms for all employees for a tax year
    /// </summary>
    Task<byte[]> GenerateW2sAsync(Guid companyId, int year);
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for generating sales reports
/// </summary>
public interface ISalesReportService
{
    /// <summary>
    /// Generates a sales report with grouping and filtering
    /// </summary>
    Task<SalesReportDto> GenerateSalesReportAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports sales report to Excel
    /// </summary>
    Task<byte[]> ExportSalesReportToExcelAsync(ReportRequestDto request);
}

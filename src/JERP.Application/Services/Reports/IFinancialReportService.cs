/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for generating financial reports with export capabilities
/// </summary>
public interface IFinancialReportService
{
    /// <summary>
    /// Generates a comprehensive Profit and Loss report
    /// </summary>
    Task<ProfitAndLossReportDto> GenerateProfitAndLossAsync(ReportRequestDto request);
    
    /// <summary>
    /// Generates a Balance Sheet report
    /// </summary>
    Task<BalanceSheetReportDto> GenerateBalanceSheetAsync(ReportRequestDto request);
    
    /// <summary>
    /// Generates a Cash Flow Statement report
    /// </summary>
    Task<CashFlowReportDto> GenerateCashFlowAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports Profit and Loss report to PDF
    /// </summary>
    Task<byte[]> ExportProfitAndLossToPdfAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports Balance Sheet to Excel
    /// </summary>
    Task<byte[]> ExportBalanceSheetToExcelAsync(ReportRequestDto request);
}

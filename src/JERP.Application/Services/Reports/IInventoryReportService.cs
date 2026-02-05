/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for generating inventory reports
/// </summary>
public interface IInventoryReportService
{
    /// <summary>
    /// Generates an inventory valuation report
    /// </summary>
    Task<InventoryValuationReportDto> GenerateValuationReportAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports inventory valuation report to PDF
    /// </summary>
    Task<byte[]> ExportValuationToPdfAsync(ReportRequestDto request);
}

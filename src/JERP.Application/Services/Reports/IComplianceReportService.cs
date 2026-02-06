/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Interface for generating compliance reports
/// </summary>
public interface IComplianceReportService
{
    /// <summary>
    /// Generates a Cannabis 280E tax compliance report
    /// </summary>
    Task<Cannabis280EReportDto> Generate280EReportAsync(ReportRequestDto request);
    
    /// <summary>
    /// Generates a METRC compliance report
    /// </summary>
    Task<MetrcComplianceReportDto> GenerateMetrcComplianceReportAsync(ReportRequestDto request);
    
    /// <summary>
    /// Exports 280E report to PDF
    /// </summary>
    Task<byte[]> Export280EReportToPdfAsync(ReportRequestDto request);
}

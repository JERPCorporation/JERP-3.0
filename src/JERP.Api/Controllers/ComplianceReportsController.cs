/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.Reports;
using JERP.Application.Services.Reports;

namespace JERP.Api.Controllers;

/// <summary>
/// Compliance reports endpoints
/// </summary>
[Route("api/v1/reports/compliance")]
[Authorize]
public class ComplianceReportsController : BaseApiController
{
    private readonly IComplianceReportService _reportService;
    private readonly ILogger<ComplianceReportsController> _logger;

    public ComplianceReportsController(
        IComplianceReportService reportService,
        ILogger<ComplianceReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate Cannabis 280E report
    /// </summary>
    [HttpPost("280e")]
    public async Task<IActionResult> Get280EReport([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.Generate280EReportAsync(request);
        
        _logger.LogInformation("Generated 280E report for company {CompanyId} tax year {TaxYear}",
            request.CompanyId, request.StartDate.Year);

        return Ok(report);
    }

    /// <summary>
    /// Export 280E report to PDF
    /// </summary>
    [HttpPost("280e/pdf")]
    public async Task<IActionResult> Export280EPdf([FromBody] ReportRequestDto request)
    {
        var pdfBytes = await _reportService.Export280EReportToPdfAsync(request);
        
        return File(pdfBytes, "application/pdf", 
            $"280E_Report_{request.CompanyId}_{request.StartDate.Year}.pdf");
    }

    /// <summary>
    /// Generate METRC compliance report
    /// </summary>
    [HttpPost("metrc")]
    public async Task<IActionResult> GetMetrcComplianceReport([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateMetrcComplianceReportAsync(request);
        
        _logger.LogInformation("Generated METRC compliance report for company {CompanyId} from {StartDate} to {EndDate}",
            request.CompanyId, request.StartDate, request.EndDate);

        return Ok(report);
    }
}

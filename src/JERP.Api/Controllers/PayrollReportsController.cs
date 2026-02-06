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
/// Payroll reports endpoints
/// </summary>
[Route("api/v1/reports/payroll")]
[Authorize]
public class PayrollReportsController : BaseApiController
{
    private readonly IPayrollReportService _reportService;
    private readonly ILogger<PayrollReportsController> _logger;

    public PayrollReportsController(
        IPayrollReportService reportService,
        ILogger<PayrollReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate payroll summary report
    /// </summary>
    [HttpPost("summary")]
    public async Task<IActionResult> GetPayrollSummary([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GeneratePayrollSummaryAsync(request);
        
        _logger.LogInformation("Generated payroll summary for company {CompanyId} from {StartDate} to {EndDate}",
            request.CompanyId, request.StartDate, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Export payroll summary to PDF
    /// </summary>
    [HttpPost("summary/pdf")]
    public async Task<IActionResult> ExportPayrollSummaryPdf([FromBody] ReportRequestDto request)
    {
        var pdfBytes = await _reportService.ExportPayrollSummaryToPdfAsync(request);
        
        return File(pdfBytes, "application/pdf", 
            $"PayrollSummary_{request.CompanyId}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.pdf");
    }

    /// <summary>
    /// Generate Form 941 report
    /// </summary>
    [HttpPost("941")]
    public async Task<IActionResult> Generate941Report(
        [FromQuery] Guid companyId,
        [FromQuery] int quarter,
        [FromQuery] int year)
    {
        var pdfBytes = await _reportService.Generate941ReportAsync(companyId, quarter, year);
        
        return File(pdfBytes, "application/pdf", $"Form941_Q{quarter}_{year}.pdf");
    }

    /// <summary>
    /// Generate W-2 forms
    /// </summary>
    [HttpPost("w2")]
    public async Task<IActionResult> GenerateW2s(
        [FromQuery] Guid companyId,
        [FromQuery] int year)
    {
        var pdfBytes = await _reportService.GenerateW2sAsync(companyId, year);
        
        return File(pdfBytes, "application/pdf", $"W2Forms_{year}.pdf");
    }
}

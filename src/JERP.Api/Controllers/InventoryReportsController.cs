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
/// Inventory reports endpoints
/// </summary>
[Route("api/v1/reports/inventory")]
[Authorize]
public class InventoryReportsController : BaseApiController
{
    private readonly IInventoryReportService _reportService;
    private readonly ILogger<InventoryReportsController> _logger;

    public InventoryReportsController(
        IInventoryReportService reportService,
        ILogger<InventoryReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate inventory valuation report
    /// </summary>
    [HttpPost("valuation")]
    public async Task<IActionResult> GetValuationReport([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateValuationReportAsync(request);
        
        _logger.LogInformation("Generated inventory valuation report for company {CompanyId} as of {AsOfDate}",
            request.CompanyId, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Export inventory valuation report to PDF
    /// </summary>
    [HttpPost("valuation/pdf")]
    public async Task<IActionResult> ExportValuationPdf([FromBody] ReportRequestDto request)
    {
        var pdfBytes = await _reportService.ExportValuationToPdfAsync(request);
        
        return File(pdfBytes, "application/pdf", 
            $"InventoryValuation_{request.CompanyId}_{request.EndDate:yyyyMMdd}.pdf");
    }
}

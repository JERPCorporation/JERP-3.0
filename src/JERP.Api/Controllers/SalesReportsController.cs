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
/// Sales reports endpoints
/// </summary>
[Route("api/v1/reports/sales")]
[Authorize]
public class SalesReportsController : BaseApiController
{
    private readonly ISalesReportService _reportService;
    private readonly ILogger<SalesReportsController> _logger;

    public SalesReportsController(
        ISalesReportService reportService,
        ILogger<SalesReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate sales report
    /// </summary>
    [HttpPost("summary")]
    public async Task<IActionResult> GetSalesReport([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateSalesReportAsync(request);
        
        _logger.LogInformation("Generated sales report for company {CompanyId} from {StartDate} to {EndDate}",
            request.CompanyId, request.StartDate, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Export sales report to Excel
    /// </summary>
    [HttpPost("summary/excel")]
    public async Task<IActionResult> ExportSalesReportExcel([FromBody] ReportRequestDto request)
    {
        var excelBytes = await _reportService.ExportSalesReportToExcelAsync(request);
        
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"SalesReport_{request.CompanyId}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.xlsx");
    }
}

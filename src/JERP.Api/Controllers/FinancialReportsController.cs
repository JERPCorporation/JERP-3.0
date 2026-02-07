/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.Reports;
using JERP.Application.Services.Reports;

namespace JERP.Api.Controllers;

/// <summary>
/// Financial reports endpoints
/// </summary>
[Route("api/v1/reports/financial")]
[Authorize]
public class FinancialReportsController : BaseApiController
{
    private readonly IFinancialReportService _reportService;
    private readonly ILogger<FinancialReportsController> _logger;

    public FinancialReportsController(
        IFinancialReportService reportService,
        ILogger<FinancialReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate Profit and Loss (Income Statement) report
    /// </summary>
    [HttpPost("profit-and-loss")]
    public async Task<IActionResult> GetProfitAndLoss([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateProfitAndLossAsync(request);
        
        _logger.LogInformation("Generated P&L report for company {CompanyId} from {StartDate} to {EndDate}",
            request.CompanyId, request.StartDate, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Generate Balance Sheet report
    /// </summary>
    [HttpPost("balance-sheet")]
    public async Task<IActionResult> GetBalanceSheet([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateBalanceSheetAsync(request);
        
        _logger.LogInformation("Generated Balance Sheet report for company {CompanyId} as of {AsOfDate}",
            request.CompanyId, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Generate Cash Flow Statement report
    /// </summary>
    [HttpPost("cash-flow")]
    public async Task<IActionResult> GetCashFlow([FromBody] ReportRequestDto request)
    {
        var report = await _reportService.GenerateCashFlowAsync(request);
        
        _logger.LogInformation("Generated Cash Flow report for company {CompanyId} from {StartDate} to {EndDate}",
            request.CompanyId, request.StartDate, request.EndDate);

        return Ok(report);
    }

    /// <summary>
    /// Export Profit and Loss report to PDF
    /// </summary>
    [HttpPost("profit-and-loss/pdf")]
    public async Task<IActionResult> ExportProfitAndLossPdf([FromBody] ReportRequestDto request)
    {
        var pdfBytes = await _reportService.ExportProfitAndLossToPdfAsync(request);
        
        return File(pdfBytes, "application/pdf", $"P&L_{request.CompanyId}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.pdf");
    }

    /// <summary>
    /// Export Balance Sheet to Excel
    /// </summary>
    [HttpPost("balance-sheet/excel")]
    public async Task<IActionResult> ExportBalanceSheetExcel([FromBody] ReportRequestDto request)
    {
        var excelBytes = await _reportService.ExportBalanceSheetToExcelAsync(request);
        
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"BalanceSheet_{request.CompanyId}_{request.EndDate:yyyyMMdd}.xlsx");
    }
}

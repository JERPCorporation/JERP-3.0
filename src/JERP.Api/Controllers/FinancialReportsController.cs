/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Finance;

namespace JERP.Api.Controllers;

/// <summary>
/// Financial reports endpoints
/// </summary>
[Route("api/v1/finance/reports")]
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
    [HttpGet("profit-and-loss")]
    public async Task<IActionResult> GetProfitAndLoss(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var report = await _reportService.GenerateProfitAndLossReportAsync(companyId, startDate, endDate);
        
        _logger.LogInformation("Generated P&L report for company {CompanyId} from {StartDate} to {EndDate}",
            companyId, startDate, endDate);

        return Ok(report);
    }

    /// <summary>
    /// Generate Balance Sheet report
    /// </summary>
    [HttpGet("balance-sheet")]
    public async Task<IActionResult> GetBalanceSheet(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime asOfDate)
    {
        var report = await _reportService.GenerateBalanceSheetReportAsync(companyId, asOfDate);
        
        _logger.LogInformation("Generated Balance Sheet report for company {CompanyId} as of {AsOfDate}",
            companyId, asOfDate);

        return Ok(report);
    }
}

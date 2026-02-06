/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.TaxWithholding;
using JERP.Application.Services.TaxWithholding;

namespace JERP.Api.Controllers;

/// <summary>
/// Tax withholding management endpoints
/// </summary>
[Route("api/v1/taxwithholding")]
[Authorize]
public class TaxWithholdingController : BaseApiController
{
    private readonly ITaxWithholdingService _taxWithholdingService;
    private readonly ILogger<TaxWithholdingController> _logger;

    public TaxWithholdingController(
        ITaxWithholdingService taxWithholdingService,
        ILogger<TaxWithholdingController> logger)
    {
        _taxWithholdingService = taxWithholdingService;
        _logger = logger;
    }

    /// <summary>
    /// Get tax withholding records for an employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(Guid employeeId)
    {
        var records = await _taxWithholdingService.GetByEmployeeAsync(employeeId);
        return Ok(records);
    }

    /// <summary>
    /// Create a new tax withholding record
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaxWithholdingDto dto)
    {
        var record = await _taxWithholdingService.CreateAsync(dto);
        _logger.LogInformation("Tax withholding record created for employee {EmployeeId}", dto.EmployeeId);
        return Created(record);
    }

    /// <summary>
    /// Update a tax withholding record
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TaxWithholdingDto dto)
    {
        var record = await _taxWithholdingService.UpdateAsync(id, dto);
        
        if (record == null)
        {
            return NotFound($"Tax withholding record with ID {id} not found");
        }

        _logger.LogInformation("Tax withholding record updated: {RecordId}", id);
        return Ok(record);
    }
}

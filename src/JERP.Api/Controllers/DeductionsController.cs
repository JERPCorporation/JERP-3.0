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
using JERP.Application.DTOs.Deductions;
using JERP.Application.Services.Deductions;

namespace JERP.Api.Controllers;

/// <summary>
/// Deduction management endpoints
/// </summary>
[Route("api/v1/deductions")]
[Authorize]
public class DeductionsController : BaseApiController
{
    private readonly IDeductionService _deductionService;
    private readonly ILogger<DeductionsController> _logger;

    public DeductionsController(IDeductionService deductionService, ILogger<DeductionsController> logger)
    {
        _deductionService = deductionService;
        _logger = logger;
    }

    /// <summary>
    /// Get deductions for an employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(Guid employeeId)
    {
        var deductions = await _deductionService.GetByEmployeeAsync(employeeId);
        return Ok(deductions);
    }

    /// <summary>
    /// Get deduction by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var deduction = await _deductionService.GetByIdAsync(id);
        
        if (deduction == null)
        {
            return NotFound($"Deduction with ID {id} not found");
        }

        return Ok(deduction);
    }

    /// <summary>
    /// Create a new deduction
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DeductionDto dto)
    {
        var deduction = await _deductionService.CreateAsync(dto);
        _logger.LogInformation("Deduction created for employee {EmployeeId}", dto.EmployeeId);
        return Created(deduction);
    }

    /// <summary>
    /// Update a deduction
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DeductionDto dto)
    {
        var deduction = await _deductionService.UpdateAsync(id, dto);
        
        if (deduction == null)
        {
            return NotFound($"Deduction with ID {id} not found");
        }

        _logger.LogInformation("Deduction updated: {DeductionId}", id);
        return Ok(deduction);
    }

    /// <summary>
    /// Delete a deduction
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deductionService.DeleteAsync(id);
        
        if (!result)
        {
            return NotFound($"Deduction with ID {id} not found");
        }

        _logger.LogInformation("Deduction deleted: {DeductionId}", id);
        return NoContent();
    }
}

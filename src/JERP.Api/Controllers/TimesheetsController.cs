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
using JERP.Application.DTOs.Timesheets;
using JERP.Application.Services.Timesheets;

namespace JERP.Api.Controllers;

/// <summary>
/// Timesheet management endpoints
/// </summary>
[Route("api/v1/timesheets")]
[Authorize]
public class TimesheetsController : BaseApiController
{
    private readonly ITimesheetService _timesheetService;
    private readonly ILogger<TimesheetsController> _logger;

    public TimesheetsController(ITimesheetService timesheetService, ILogger<TimesheetsController> logger)
    {
        _timesheetService = timesheetService;
        _logger = logger;
    }

    /// <summary>
    /// Get timesheets by employee with pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid employeeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _timesheetService.GetByEmployeeAsync(employeeId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get timesheet by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var timesheet = await _timesheetService.GetByIdAsync(id);
        
        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        return Ok(timesheet);
    }

    /// <summary>
    /// Get timesheets by employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _timesheetService.GetByEmployeeAsync(employeeId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Create a new timesheet
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TimesheetCreateRequest request)
    {
        var timesheet = await _timesheetService.CreateAsync(request);
        _logger.LogInformation("Timesheet created: {TimesheetId}", timesheet.Id);
        return Created(timesheet);
    }

    /// <summary>
    /// Update an existing timesheet
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TimesheetUpdateRequest request)
    {
        var timesheet = await _timesheetService.UpdateAsync(id, request);
        
        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        _logger.LogInformation("Timesheet updated: {TimesheetId}", id);
        return Ok(timesheet);
    }

    /// <summary>
    /// Delete a timesheet
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _timesheetService.DeleteAsync(id);
        
        _logger.LogInformation("Timesheet deleted: {TimesheetId}", id);
        return NoContent();
    }

    /// <summary>
    /// Submit a timesheet for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var timesheet = await _timesheetService.SubmitAsync(id);
        
        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        _logger.LogInformation("Timesheet submitted: {TimesheetId}", id);
        return Ok(timesheet);
    }

    /// <summary>
    /// Approve a timesheet
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id, [FromQuery] Guid approverId)
    {
        var timesheet = await _timesheetService.ApproveAsync(id, approverId);
        
        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        _logger.LogInformation("Timesheet approved: {TimesheetId}", id);
        return Ok(timesheet);
    }

    /// <summary>
    /// Reject a timesheet
    /// </summary>
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectRequest request)
    {
        var timesheet = await _timesheetService.RejectAsync(id, request.Reason);
        
        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        _logger.LogInformation("Timesheet rejected: {TimesheetId}", id);
        return Ok(timesheet);
    }
}

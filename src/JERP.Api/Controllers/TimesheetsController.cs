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
    /// Get all timesheets with filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? employeeId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var result = await _timesheetService.GetAllAsync(page, pageSize, employeeId, startDate, endDate);
        return Ok(result);
    }

    /// <summary>
    /// Get timesheet by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
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
        int employeeId,
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
    public async Task<IActionResult> Update(int id, [FromBody] TimesheetUpdateRequest request)
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
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _timesheetService.DeleteAsync(id);
        
        if (!result)
        {
            return NotFound($"Timesheet with ID {id} not found");
        }

        _logger.LogInformation("Timesheet deleted: {TimesheetId}", id);
        return NoContent();
    }

    /// <summary>
    /// Submit a timesheet for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(int id)
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
    public async Task<IActionResult> Approve(int id)
    {
        var timesheet = await _timesheetService.ApproveAsync(id);
        
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
    public async Task<IActionResult> Reject(int id, [FromBody] RejectRequest request)
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

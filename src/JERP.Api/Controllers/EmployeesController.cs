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
using JERP.Application.DTOs.Employees;
using JERP.Application.Services.Employees;

namespace JERP.Api.Controllers;

/// <summary>
/// Employee management endpoints
/// </summary>
[Route("api/v1/employees")]
[Authorize]
public class EmployeesController : BaseApiController
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees with pagination and search
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _employeeService.GetAllAsync(page, pageSize, searchTerm);
        return Ok(result);
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found");
        }

        return Ok(employee);
    }

    /// <summary>
    /// Get employee by employee number
    /// </summary>
    [HttpGet("number/{employeeNumber}")]
    public async Task<IActionResult> GetByEmployeeNumber(string employeeNumber)
    {
        var employee = await _employeeService.GetByEmployeeNumberAsync(employeeNumber);
        
        if (employee == null)
        {
            return NotFound($"Employee with number {employeeNumber} not found");
        }

        return Ok(employee);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest request)
    {
        var employee = await _employeeService.CreateAsync(request);
        _logger.LogInformation("Employee created: {EmployeeId}", employee.Id);
        return Created(employee);
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeUpdateRequest request)
    {
        var employee = await _employeeService.UpdateAsync(id, request);
        
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found");
        }

        _logger.LogInformation("Employee updated: {EmployeeId}", id);
        return Ok(employee);
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _employeeService.DeleteAsync(id);
        
        _logger.LogInformation("Employee deleted: {EmployeeId}", id);
        return NoContent();
    }

    /// <summary>
    /// Terminate an employee
    /// </summary>
    [HttpPost("{id}/terminate")]
    public async Task<IActionResult> Terminate(Guid id, [FromBody] TerminateRequest request)
    {
        var employee = await _employeeService.TerminateAsync(id, request.TerminationDate, request.Reason);
        
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found");
        }

        _logger.LogInformation("Employee terminated: {EmployeeId}", id);
        return Ok(employee);
    }
}

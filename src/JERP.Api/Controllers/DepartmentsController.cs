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
using JERP.Core.Entities;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JERP.Api.Controllers;

/// <summary>
/// Controller for managing departments
/// </summary>
[Authorize]
[Route("api/v1/[controller]")]
public class DepartmentsController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(
        JerpDbContext context,
        ILogger<DepartmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _context.Departments
            .Include(d => d.Employees)
            .ToListAsync();

        return Ok(departments);
    }

    /// <summary>
    /// Get department by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var department = await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return NotFound($"Department with ID {id} not found");
        }

        return Ok(department);
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        var department = new Department
        {
            Name = request.Name,
            Description = request.Description,
            ManagerId = request.ManagerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} created", department.Id);

        return Created(department);
    }

    /// <summary>
    /// Update a department
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            return NotFound($"Department with ID {id} not found");
        }

        department.Name = request.Name ?? department.Name;
        department.Description = request.Description ?? department.Description;
        department.ManagerId = request.ManagerId ?? department.ManagerId;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} updated", department.Id);

        return Ok(department);
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            return NotFound($"Department with ID {id} not found");
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Department {DepartmentId} deleted", department.Id);

        return Ok(new { message = "Department deleted successfully" });
    }
}

public record CreateDepartmentRequest(string Name, string? Description, Guid? ManagerId);
public record UpdateDepartmentRequest(string? Name, string? Description, Guid? ManagerId);

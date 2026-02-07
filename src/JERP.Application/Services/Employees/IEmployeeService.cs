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

using JERP.Application.DTOs.Employees;

namespace JERP.Application.Services.Employees;

/// <summary>
/// Interface for employee management services
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Gets an employee by ID
    /// </summary>
    Task<EmployeeDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets an employee by employee number
    /// </summary>
    Task<EmployeeDto> GetByEmployeeNumberAsync(string employeeNumber);

    /// <summary>
    /// Gets a paginated list of employees with optional search
    /// </summary>
    Task<EmployeeListResponse> GetAllAsync(int page, int pageSize, string? searchTerm = null);

    /// <summary>
    /// Creates a new employee with compliance validation
    /// </summary>
    Task<EmployeeDto> CreateAsync(EmployeeCreateRequest request);

    /// <summary>
    /// Updates an existing employee with compliance validation
    /// </summary>
    Task<EmployeeDto> UpdateAsync(Guid id, EmployeeUpdateRequest request);

    /// <summary>
    /// Soft deletes an employee
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Terminates an employee
    /// </summary>
    Task<EmployeeDto> TerminateAsync(Guid id, DateTime terminationDate, string reason);
}

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

using JERP.Application.DTOs.Timesheets;

namespace JERP.Application.Services.Timesheets;

/// <summary>
/// Interface for timesheet management services
/// </summary>
public interface ITimesheetService
{
    /// <summary>
    /// Gets a timesheet by ID
    /// </summary>
    Task<TimesheetDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets paginated timesheets for an employee
    /// </summary>
    Task<TimesheetListResponse> GetByEmployeeAsync(Guid employeeId, int page, int pageSize);

    /// <summary>
    /// Creates a new timesheet
    /// </summary>
    Task<TimesheetDto> CreateAsync(TimesheetCreateRequest request);

    /// <summary>
    /// Updates an existing timesheet
    /// </summary>
    Task<TimesheetDto> UpdateAsync(Guid id, TimesheetUpdateRequest request);

    /// <summary>
    /// Soft deletes a timesheet
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Submits a timesheet for approval
    /// </summary>
    Task<TimesheetDto> SubmitAsync(Guid id);

    /// <summary>
    /// Approves a timesheet
    /// </summary>
    Task<TimesheetDto> ApproveAsync(Guid id, Guid approverId);

    /// <summary>
    /// Rejects a timesheet
    /// </summary>
    Task<TimesheetDto> RejectAsync(Guid id, string reason);
}

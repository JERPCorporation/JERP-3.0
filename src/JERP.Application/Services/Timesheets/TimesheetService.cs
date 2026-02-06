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

using JERP.Application.DTOs.Timesheets;
using JERP.Compliance.Services;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Timesheets;

/// <summary>
/// Implementation of timesheet management services with California overtime rules
/// </summary>
public class TimesheetService : ITimesheetService
{
    private readonly JerpDbContext _context;
    private readonly IComplianceEngine _complianceEngine;
    private readonly ILogger<TimesheetService> _logger;

    public TimesheetService(
        JerpDbContext context,
        IComplianceEngine complianceEngine,
        ILogger<TimesheetService> logger)
    {
        _context = context;
        _complianceEngine = complianceEngine;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> GetByIdAsync(Guid id)
    {
        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        return MapToDto(timesheet);
    }

    /// <inheritdoc/>
    public async Task<TimesheetListResponse> GetByEmployeeAsync(Guid employeeId, int page, int pageSize)
    {
        var query = _context.Timesheets
            .Where(t => t.EmployeeId == employeeId && !t.IsDeleted);

        var totalCount = await query.CountAsync();
        var timesheets = await query
            .OrderByDescending(t => t.WorkDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new TimesheetListResponse
        {
            Items = timesheets.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> CreateAsync(TimesheetCreateRequest request)
    {
        _logger.LogInformation("Creating timesheet for employee: {EmployeeId}, Date: {WorkDate}",
            request.EmployeeId, request.WorkDate);

        var timesheet = new Timesheet
        {
            EmployeeId = request.EmployeeId,
            WorkDate = request.WorkDate.Date,
            ClockIn = request.ClockIn,
            ClockOut = request.ClockOut,
            BreakMinutes = request.BreakMinutes,
            Notes = request.Notes,
            Status = TimesheetStatus.Draft
        };

        CalculateHours(timesheet);

        _context.Timesheets.Add(timesheet);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet created: {TimesheetId}", timesheet.Id);

        return MapToDto(timesheet);
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> UpdateAsync(Guid id, TimesheetUpdateRequest request)
    {
        _logger.LogInformation("Updating timesheet: {TimesheetId}", id);

        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        if (timesheet.Status == TimesheetStatus.Approved)
        {
            throw new InvalidOperationException("Cannot update an approved timesheet");
        }

        timesheet.ClockIn = request.ClockIn;
        timesheet.ClockOut = request.ClockOut;
        timesheet.BreakMinutes = request.BreakMinutes;
        timesheet.Notes = request.Notes;

        CalculateHours(timesheet);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet updated: {TimesheetId}", id);

        return MapToDto(timesheet);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting timesheet: {TimesheetId}", id);

        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        if (timesheet.Status == TimesheetStatus.Approved)
        {
            throw new InvalidOperationException("Cannot delete an approved timesheet");
        }

        _context.Timesheets.Remove(timesheet);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet deleted: {TimesheetId}", id);
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> SubmitAsync(Guid id)
    {
        _logger.LogInformation("Submitting timesheet: {TimesheetId}", id);

        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        if (timesheet.Status != TimesheetStatus.Draft)
        {
            throw new InvalidOperationException("Only draft timesheets can be submitted");
        }

        // Run compliance checks before submission
        var violations = await _complianceEngine.EvaluateTimesheetAsync(timesheet.Id);
        if (violations.Any(v => v.Severity == ViolationSeverity.Critical))
        {
            throw new InvalidOperationException("Cannot submit timesheet with critical compliance violations");
        }

        timesheet.Status = TimesheetStatus.Submitted;
        timesheet.SubmittedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet submitted: {TimesheetId}", id);

        return MapToDto(timesheet);
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> ApproveAsync(Guid id, Guid approverId)
    {
        _logger.LogInformation("Approving timesheet: {TimesheetId} by {ApproverId}", id, approverId);

        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        if (timesheet.Status != TimesheetStatus.Submitted)
        {
            throw new InvalidOperationException("Only submitted timesheets can be approved");
        }

        timesheet.Status = TimesheetStatus.Approved;
        timesheet.ApprovedAt = DateTime.UtcNow;
        timesheet.ApprovedById = approverId;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet approved: {TimesheetId}", id);

        return MapToDto(timesheet);
    }

    /// <inheritdoc/>
    public async Task<TimesheetDto> RejectAsync(Guid id, string reason)
    {
        _logger.LogInformation("Rejecting timesheet: {TimesheetId}, Reason: {Reason}", id, reason);

        var timesheet = await _context.Timesheets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (timesheet == null)
        {
            throw new InvalidOperationException($"Timesheet with ID {id} not found");
        }

        if (timesheet.Status != TimesheetStatus.Submitted)
        {
            throw new InvalidOperationException("Only submitted timesheets can be rejected");
        }

        timesheet.Status = TimesheetStatus.Rejected;
        timesheet.Notes = $"Rejected: {reason}\n{timesheet.Notes}";

        await _context.SaveChangesAsync();

        _logger.LogInformation("Timesheet rejected: {TimesheetId}", id);

        return MapToDto(timesheet);
    }

    /// <summary>
    /// Calculates hours with California overtime rules:
    /// - 1.5x after 8 hours in a day
    /// - 2x after 12 hours in a day
    /// </summary>
    private void CalculateHours(Timesheet timesheet)
    {
        if (!timesheet.ClockIn.HasValue || !timesheet.ClockOut.HasValue)
        {
            timesheet.TotalHours = 0;
            timesheet.RegularHours = 0;
            timesheet.OvertimeHours = 0;
            timesheet.DoubleTimeHours = 0;
            return;
        }

        var totalMinutes = (timesheet.ClockOut.Value - timesheet.ClockIn.Value).TotalMinutes;
        totalMinutes -= timesheet.BreakMinutes;
        
        var totalHours = (decimal)(totalMinutes / 60.0);
        timesheet.TotalHours = Math.Round(totalHours, 2);

        // California daily overtime rules
        if (totalHours <= 8)
        {
            // All regular hours
            timesheet.RegularHours = timesheet.TotalHours;
            timesheet.OvertimeHours = 0;
            timesheet.DoubleTimeHours = 0;
        }
        else if (totalHours <= 12)
        {
            // 8 regular hours, rest is overtime
            timesheet.RegularHours = 8;
            timesheet.OvertimeHours = Math.Round(timesheet.TotalHours - 8, 2);
            timesheet.DoubleTimeHours = 0;
        }
        else
        {
            // 8 regular, 4 overtime, rest is double time
            timesheet.RegularHours = 8;
            timesheet.OvertimeHours = 4;
            timesheet.DoubleTimeHours = Math.Round(timesheet.TotalHours - 12, 2);
        }
    }

    private TimesheetDto MapToDto(Timesheet timesheet)
    {
        return new TimesheetDto
        {
            Id = timesheet.Id,
            EmployeeId = timesheet.EmployeeId,
            WorkDate = timesheet.WorkDate,
            ClockIn = timesheet.ClockIn,
            ClockOut = timesheet.ClockOut,
            BreakMinutes = timesheet.BreakMinutes,
            TotalHours = timesheet.TotalHours,
            RegularHours = timesheet.RegularHours,
            OvertimeHours = timesheet.OvertimeHours,
            DoubleTimeHours = timesheet.DoubleTimeHours,
            StatusEnum = timesheet.Status,
            Status = timesheet.Status.ToString(),
            Notes = timesheet.Notes,
            SubmittedAt = timesheet.SubmittedAt,
            ApprovedAt = timesheet.ApprovedAt,
            ApprovedById = timesheet.ApprovedById,
            CreatedAt = timesheet.CreatedAt,
            UpdatedAt = timesheet.UpdatedAt
        };
    }
}

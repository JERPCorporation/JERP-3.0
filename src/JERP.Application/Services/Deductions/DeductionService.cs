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

using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Deductions;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;

namespace JERP.Application.Services.Deductions;

public class DeductionService : IDeductionService
{
    private readonly JerpDbContext _context;

    public DeductionService(JerpDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeductionDto>> GetByEmployeeAsync(Guid employeeId)
    {
        return await _context.Deductions
            .Where(d => d.EmployeeId == employeeId)
            .Select(d => MapToDto(d))
            .ToListAsync();
    }

    public async Task<DeductionDto?> GetByIdAsync(Guid id)
    {
        var deduction = await _context.Deductions.FindAsync(id);
        return deduction != null ? MapToDto(deduction) : null;
    }

    public async Task<DeductionDto> CreateAsync(DeductionDto dto)
    {
        var deduction = new Deduction
        {
            EmployeeId = dto.EmployeeId,
            Description = dto.Description,
            DeductionType = Enum.Parse<DeductionType>(dto.DeductionType),
            Amount = dto.Amount,
            IsPreTax = dto.IsPreTax,
            IsPercentage = dto.IsPercentage,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive
        };

        _context.Deductions.Add(deduction);
        await _context.SaveChangesAsync();

        return MapToDto(deduction);
    }

    public async Task<DeductionDto?> UpdateAsync(Guid id, DeductionDto dto)
    {
        var deduction = await _context.Deductions.FindAsync(id);
        if (deduction == null) return null;

        deduction.Description = dto.Description;
        deduction.DeductionType = Enum.Parse<DeductionType>(dto.DeductionType);
        deduction.Amount = dto.Amount;
        deduction.IsPreTax = dto.IsPreTax;
        deduction.IsPercentage = dto.IsPercentage;
        deduction.StartDate = dto.StartDate;
        deduction.EndDate = dto.EndDate;
        deduction.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return MapToDto(deduction);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deduction = await _context.Deductions.FindAsync(id);
        if (deduction == null) return false;

        _context.Deductions.Remove(deduction);
        await _context.SaveChangesAsync();
        return true;
    }

    private static DeductionDto MapToDto(Deduction deduction)
    {
        return new DeductionDto
        {
            Id = deduction.Id,
            EmployeeId = deduction.EmployeeId,
            Description = deduction.Description,
            DeductionType = deduction.DeductionType.ToString(),
            Amount = deduction.Amount,
            IsPreTax = deduction.IsPreTax,
            IsPercentage = deduction.IsPercentage,
            StartDate = deduction.StartDate,
            EndDate = deduction.EndDate,
            IsActive = deduction.IsActive
        };
    }
}

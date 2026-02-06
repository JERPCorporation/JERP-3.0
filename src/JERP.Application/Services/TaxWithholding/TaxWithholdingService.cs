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

using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.TaxWithholding;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;

namespace JERP.Application.Services.TaxWithholding;

public class TaxWithholdingService : ITaxWithholdingService
{
    private readonly JerpDbContext _context;

    public TaxWithholdingService(JerpDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaxWithholdingDto>> GetByEmployeeAsync(Guid employeeId)
    {
        return await _context.TaxWithholdings
            .Where(t => t.EmployeeId == employeeId)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    public async Task<TaxWithholdingDto> CreateAsync(TaxWithholdingDto dto)
    {
        var taxWithholding = new Core.Entities.TaxWithholding
        {
            EmployeeId = dto.EmployeeId,
            TaxYear = dto.TaxYear,
            FilingStatus = Enum.Parse<FilingStatus>(dto.FilingStatus),
            FederalAllowances = dto.FederalAllowances,
            FederalExtraWithholding = dto.FederalExtraWithholding,
            StateAllowances = dto.StateAllowances,
            StateExtraWithholding = dto.StateExtraWithholding,
            IsExemptFederal = dto.IsExemptFederal,
            IsExemptState = dto.IsExemptState,
            EffectiveDate = dto.EffectiveDate
        };

        _context.TaxWithholdings.Add(taxWithholding);
        await _context.SaveChangesAsync();

        return MapToDto(taxWithholding);
    }

    public async Task<TaxWithholdingDto?> UpdateAsync(Guid id, TaxWithholdingDto dto)
    {
        var taxWithholding = await _context.TaxWithholdings.FindAsync(id);
        if (taxWithholding == null) return null;

        taxWithholding.TaxYear = dto.TaxYear;
        taxWithholding.FilingStatus = Enum.Parse<FilingStatus>(dto.FilingStatus);
        taxWithholding.FederalAllowances = dto.FederalAllowances;
        taxWithholding.FederalExtraWithholding = dto.FederalExtraWithholding;
        taxWithholding.StateAllowances = dto.StateAllowances;
        taxWithholding.StateExtraWithholding = dto.StateExtraWithholding;
        taxWithholding.IsExemptFederal = dto.IsExemptFederal;
        taxWithholding.IsExemptState = dto.IsExemptState;
        taxWithholding.EffectiveDate = dto.EffectiveDate;

        await _context.SaveChangesAsync();
        return MapToDto(taxWithholding);
    }

    private static TaxWithholdingDto MapToDto(Core.Entities.TaxWithholding taxWithholding)
    {
        return new TaxWithholdingDto
        {
            Id = taxWithholding.Id,
            EmployeeId = taxWithholding.EmployeeId,
            TaxYear = taxWithholding.TaxYear,
            FilingStatus = taxWithholding.FilingStatus.ToString(),
            FederalAllowances = taxWithholding.FederalAllowances,
            FederalExtraWithholding = taxWithholding.FederalExtraWithholding,
            StateAllowances = taxWithholding.StateAllowances,
            StateExtraWithholding = taxWithholding.StateExtraWithholding,
            IsExemptFederal = taxWithholding.IsExemptFederal,
            IsExemptState = taxWithholding.IsExemptState,
            EffectiveDate = taxWithholding.EffectiveDate
        };
    }
}

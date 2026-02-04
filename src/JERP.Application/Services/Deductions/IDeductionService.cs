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

using JERP.Application.DTOs.Deductions;

namespace JERP.Application.Services.Deductions;

public interface IDeductionService
{
    Task<List<DeductionDto>> GetByEmployeeAsync(Guid employeeId);
    Task<DeductionDto?> GetByIdAsync(Guid id);
    Task<DeductionDto> CreateAsync(DeductionDto dto);
    Task<DeductionDto?> UpdateAsync(Guid id, DeductionDto dto);
    Task<bool> DeleteAsync(Guid id);
}

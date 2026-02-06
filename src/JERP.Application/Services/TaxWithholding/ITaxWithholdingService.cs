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

using JERP.Application.DTOs.TaxWithholding;

namespace JERP.Application.Services.TaxWithholding;

public interface ITaxWithholdingService
{
    Task<List<TaxWithholdingDto>> GetByEmployeeAsync(Guid employeeId);
    Task<TaxWithholdingDto> CreateAsync(TaxWithholdingDto dto);
    Task<TaxWithholdingDto?> UpdateAsync(Guid id, TaxWithholdingDto dto);
}

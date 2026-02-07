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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JERP.Application.DTOs.Inventory;

namespace JERP.Application.Services.Inventory;

public interface IBatchLotService
{
    Task<BatchLotDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<BatchLotDto>> GetByItemIdAsync(Guid itemId);
    
    Task<IEnumerable<BatchLotDto>> GetActiveByItemIdAsync(Guid itemId);
    
    Task<IEnumerable<BatchLotDto>> GetExpiringBatchesAsync(Guid companyId, int daysAhead);
    
    Task<IEnumerable<BatchLotDto>> GetQuarantinedBatchesAsync(Guid companyId);
    
    Task<BatchLotDto> CreateAsync(CreateBatchLotRequest request, string userId);
    
    Task<BatchLotDto> UpdateAsync(Guid id, CreateBatchLotRequest request, string userId);
    
    Task<bool> QuarantineAsync(Guid id, string reason, string userId);
    
    Task<bool> ReleaseFromQuarantineAsync(Guid id, string userId);
    
    Task<bool> DeactivateAsync(Guid id);
}

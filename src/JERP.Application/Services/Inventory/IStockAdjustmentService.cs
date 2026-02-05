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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JERP.Application.DTOs.Inventory;

namespace JERP.Application.Services.Inventory;

public interface IStockAdjustmentService
{
    Task<StockAdjustmentDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<StockAdjustmentDto>> GetAllAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null);
    
    Task<IEnumerable<StockAdjustmentDto>> GetByStatusAsync(Guid companyId, string status);
    
    Task<IEnumerable<StockAdjustmentDto>> GetByItemIdAsync(Guid itemId);
    
    Task<StockAdjustmentDto> CreateAsync(Guid companyId, CreateStockAdjustmentRequest request, string userId);
    
    Task<StockAdjustmentDto> ApproveAsync(Guid id, string userId);
    
    Task<StockAdjustmentDto> PostAsync(Guid id, string userId);
    
    Task<bool> RejectAsync(Guid id, string reason, string userId);
}

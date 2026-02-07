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

public interface IStockMovementService
{
    Task<StockMovementDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<StockMovementDto>> GetByItemIdAsync(Guid itemId, DateTime? startDate = null, DateTime? endDate = null);
    
    Task<IEnumerable<StockMovementDto>> GetByWarehouseIdAsync(Guid warehouseId, DateTime? startDate = null, DateTime? endDate = null);
    
    Task<IEnumerable<StockMovementDto>> GetByTypeAsync(Guid companyId, string movementType, DateTime? startDate = null, DateTime? endDate = null);
    
    Task<StockMovementDto> CreateReceiptAsync(CreateStockMovementRequest request, string userId);
    
    Task<StockMovementDto> CreateIssueAsync(CreateStockMovementRequest request, string userId);
    
    Task<StockMovementDto> CreateTransferAsync(CreateStockMovementRequest request, string userId);
    
    Task<StockMovementDto> CreateReturnAsync(CreateStockMovementRequest request, string userId);
    
    Task<bool> ReverseMovementAsync(Guid movementId, string reason, string userId);
}

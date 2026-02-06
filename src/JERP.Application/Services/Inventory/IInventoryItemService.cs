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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JERP.Application.DTOs.Inventory;

namespace JERP.Application.Services.Inventory;

public interface IInventoryItemService
{
    Task<InventoryItemDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<InventoryItemDto>> GetAllAsync(Guid companyId);
    
    Task<IEnumerable<InventoryItemDto>> SearchAsync(Guid companyId, string searchTerm);
    
    Task<IEnumerable<InventoryItemDto>> GetByTypeAsync(Guid companyId, string itemType);
    
    Task<IEnumerable<InventoryItemDto>> GetByCategoryAsync(Guid companyId, string category);
    
    Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync(Guid companyId);
    
    Task<IEnumerable<InventoryItemDto>> GetCannabisProductsAsync(Guid companyId);
    
    Task<InventoryItemDto> CreateAsync(Guid companyId, CreateInventoryItemRequest request, string userId);
    
    Task<InventoryItemDto> UpdateAsync(Guid id, UpdateInventoryItemRequest request, string userId);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<bool> ActivateAsync(Guid id);
    
    Task<bool> DeactivateAsync(Guid id);
    
    Task<decimal> GetAvailableQuantityAsync(Guid itemId);
    
    Task<decimal> GetTotalValueAsync(Guid companyId);
}

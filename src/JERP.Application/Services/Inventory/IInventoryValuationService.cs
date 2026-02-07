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

public interface IInventoryValuationService
{
    Task<IEnumerable<InventoryValuationDto>> GetValuationByCompanyAsync(Guid companyId, DateTime? asOfDate = null);
    
    Task<IEnumerable<InventoryValuationDto>> GetValuationByWarehouseAsync(Guid warehouseId, DateTime? asOfDate = null);
    
    Task<InventoryValuationDto> GetValuationByItemAsync(Guid itemId, DateTime? asOfDate = null);
    
    Task<decimal> GetTotalInventoryValueAsync(Guid companyId, DateTime? asOfDate = null);
}

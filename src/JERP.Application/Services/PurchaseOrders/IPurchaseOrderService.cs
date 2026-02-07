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

using JERP.Application.DTOs.PurchaseOrders;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service interface for purchase order management
/// </summary>
public interface IPurchaseOrderService
{
    /// <summary>
    /// Get purchase order by ID with full details
    /// </summary>
    Task<PurchaseOrderDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all purchase orders for a company
    /// </summary>
    Task<List<PurchaseOrderListDto>> GetAllAsync(Guid companyId, string? status = null);
    
    /// <summary>
    /// Create a new purchase order
    /// </summary>
    Task<PurchaseOrderDto> CreateAsync(Guid companyId, CreatePurchaseOrderRequest request, string? userId = null);
    
    /// <summary>
    /// Update an existing purchase order
    /// </summary>
    Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderRequest request);
    
    /// <summary>
    /// Approve a purchase order
    /// </summary>
    Task<PurchaseOrderDto> ApproveAsync(Guid id, string? userId = null);
    
    /// <summary>
    /// Cancel a purchase order
    /// </summary>
    Task<PurchaseOrderDto> CancelAsync(Guid id);
    
    /// <summary>
    /// Close a purchase order
    /// </summary>
    Task<PurchaseOrderDto> CloseAsync(Guid id);
    
    /// <summary>
    /// Generate next PO number
    /// </summary>
    Task<string> GeneratePONumberAsync(Guid companyId);
}

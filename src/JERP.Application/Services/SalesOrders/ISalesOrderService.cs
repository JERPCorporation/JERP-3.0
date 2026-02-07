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

using JERP.Application.DTOs.SalesOrders;
using JERP.Core.Enums;

namespace JERP.Application.Services.SalesOrders;

/// <summary>
/// Service interface for sales order management
/// </summary>
public interface ISalesOrderService
{
    /// <summary>
    /// Get sales order by ID with line items
    /// </summary>
    Task<SalesOrderDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all sales orders for a company
    /// </summary>
    Task<List<SalesOrderDto>> GetAllAsync(Guid companyId, SalesOrderStatus? status = null);
    
    /// <summary>
    /// Get sales orders by customer
    /// </summary>
    Task<List<SalesOrderDto>> GetByCustomerAsync(Guid customerId);
    
    /// <summary>
    /// Create a new sales order
    /// </summary>
    Task<SalesOrderDto> CreateAsync(Guid companyId, CreateSalesOrderRequest request);
    
    /// <summary>
    /// Update an existing sales order (only if Draft)
    /// </summary>
    Task<SalesOrderDto> UpdateAsync(Guid id, CreateSalesOrderRequest request);
    
    /// <summary>
    /// Submit sales order for approval
    /// </summary>
    Task<SalesOrderDto> SubmitAsync(Guid id);
    
    /// <summary>
    /// Approve a sales order (with credit check)
    /// </summary>
    Task<SalesOrderDto> ApproveAsync(Guid id, string approvedBy);
    
    /// <summary>
    /// Cancel a sales order
    /// </summary>
    Task<SalesOrderDto> CancelAsync(Guid id, string reason);
    
    /// <summary>
    /// Close a completed sales order
    /// </summary>
    Task<SalesOrderDto> CloseAsync(Guid id);
    
    /// <summary>
    /// Check if customer has available credit for order
    /// </summary>
    Task<bool> CheckCreditLimitAsync(Guid customerId, decimal orderAmount);
}

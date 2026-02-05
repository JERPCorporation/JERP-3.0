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

using JERP.Application.DTOs.SalesOrders;

namespace JERP.Application.Services.SalesOrders;

/// <summary>
/// Service interface for sales return (RMA) management
/// </summary>
public interface ISalesReturnService
{
    /// <summary>
    /// Get sales return by ID with line items
    /// </summary>
    Task<SalesReturnDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all sales returns for a company
    /// </summary>
    Task<List<SalesReturnDto>> GetAllAsync(Guid companyId);
    
    /// <summary>
    /// Get sales returns by customer
    /// </summary>
    Task<List<SalesReturnDto>> GetByCustomerAsync(Guid customerId);
    
    /// <summary>
    /// Request a new sales return (RMA)
    /// </summary>
    Task<SalesReturnDto> RequestAsync(Guid companyId, Guid customerId, Guid? salesOrderId, string reason);
    
    /// <summary>
    /// Approve a sales return request
    /// </summary>
    Task<SalesReturnDto> ApproveAsync(Guid id, string approvedBy);
    
    /// <summary>
    /// Receive returned items
    /// </summary>
    Task<SalesReturnDto> ReceiveAsync(Guid id, string receivedBy);
    
    /// <summary>
    /// Process refund
    /// </summary>
    Task<SalesReturnDto> RefundAsync(Guid id);
    
    /// <summary>
    /// Close a sales return
    /// </summary>
    Task<SalesReturnDto> CloseAsync(Guid id);
}

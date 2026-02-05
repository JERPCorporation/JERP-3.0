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

using JERP.Application.DTOs.PurchaseOrders;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service interface for purchase order receipt management
/// </summary>
public interface IPOReceiptService
{
    /// <summary>
    /// Get receipt by ID
    /// </summary>
    Task<POReceiptDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all receipts for a purchase order
    /// </summary>
    Task<List<POReceiptDto>> GetByPurchaseOrderAsync(Guid purchaseOrderId);
    
    /// <summary>
    /// Create a new receipt
    /// </summary>
    Task<POReceiptDto> CreateAsync(Guid companyId, CreatePOReceiptRequest request, string? userId = null);
    
    /// <summary>
    /// Generate next receipt number
    /// </summary>
    Task<string> GenerateReceiptNumberAsync(Guid companyId);
}

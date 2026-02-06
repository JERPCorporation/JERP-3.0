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

using JERP.Application.DTOs.PurchaseOrders;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service interface for vendor bill management
/// </summary>
public interface IVendorBillService
{
    /// <summary>
    /// Get bill by ID
    /// </summary>
    Task<VendorBillDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all bills for a vendor
    /// </summary>
    Task<List<VendorBillDto>> GetByVendorAsync(Guid vendorId);
    
    /// <summary>
    /// Create a new vendor bill
    /// </summary>
    Task<VendorBillDto> CreateAsync(Guid companyId, CreateVendorBillRequest request);
    
    /// <summary>
    /// Generate next bill number
    /// </summary>
    Task<string> GenerateBillNumberAsync(Guid companyId);
}

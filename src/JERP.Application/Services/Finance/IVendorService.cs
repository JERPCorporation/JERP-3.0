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

using JERP.Application.DTOs.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for vendor management
/// </summary>
public interface IVendorService
{
    /// <summary>
    /// Get vendor by ID
    /// </summary>
    Task<VendorDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all vendors for a company
    /// </summary>
    Task<List<VendorListDto>> GetAllAsync(Guid companyId, bool? isActive = null);
    
    /// <summary>
    /// Create a new vendor
    /// </summary>
    Task<VendorDto> CreateAsync(Guid companyId, CreateVendorDto dto);
    
    /// <summary>
    /// Update an existing vendor
    /// </summary>
    Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto dto);
    
    /// <summary>
    /// Soft delete a vendor
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Get vendor balance
    /// </summary>
    Task<decimal> GetVendorBalanceAsync(Guid vendorId);
    
    /// <summary>
    /// Generate next vendor number
    /// </summary>
    Task<string> GenerateVendorNumberAsync(Guid companyId);
}

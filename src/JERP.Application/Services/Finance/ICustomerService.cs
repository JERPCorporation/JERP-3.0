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

using JERP.Application.DTOs.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for customer management
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Get customer by ID
    /// </summary>
    Task<CustomerDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get all customers for a company
    /// </summary>
    Task<List<CustomerListDto>> GetAllAsync(Guid companyId, bool? isActive = null);
    
    /// <summary>
    /// Create a new customer
    /// </summary>
    Task<CustomerDto> CreateAsync(Guid companyId, CreateCustomerDto dto);
    
    /// <summary>
    /// Update an existing customer
    /// </summary>
    Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
    
    /// <summary>
    /// Soft delete a customer
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Get customer balance
    /// </summary>
    Task<decimal> GetCustomerBalanceAsync(Guid customerId);
    
    /// <summary>
    /// Get available credit for customer
    /// </summary>
    Task<decimal> GetAvailableCreditAsync(Guid customerId);
}

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
using JERP.Core.Enums;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for accounts payable management
/// </summary>
public interface IAPService
{
    /// <summary>
    /// Get bill by ID with line items
    /// </summary>
    Task<BillDto?> GetBillByIdAsync(Guid id);
    
    /// <summary>
    /// Get bills for a company
    /// </summary>
    Task<List<BillListDto>> GetBillsAsync(Guid companyId, BillStatus? status = null);
    
    /// <summary>
    /// Create a new bill
    /// </summary>
    Task<BillDto> CreateBillAsync(Guid companyId, CreateBillDto dto);
    
    /// <summary>
    /// Update an existing bill (only if Draft)
    /// </summary>
    Task<BillDto> UpdateBillAsync(Guid id, UpdateBillDto dto);
    
    /// <summary>
    /// Approve a bill and post to GL
    /// </summary>
    Task<BillDto> ApproveBillAsync(Guid id);
    
    /// <summary>
    /// Create a payment for a bill
    /// </summary>
    Task<Guid> CreatePaymentAsync(Guid companyId, CreateBillPaymentDto dto);
    
    /// <summary>
    /// Create batch payments for multiple bills
    /// </summary>
    Task<List<Guid>> CreateBatchPaymentsAsync(Guid companyId, List<CreateBillPaymentDto> payments);
    
    /// <summary>
    /// Void a bill and reverse GL entries
    /// </summary>
    Task VoidBillAsync(Guid id);
    
    /// <summary>
    /// Generate next bill number
    /// </summary>
    Task<string> GenerateBillNumberAsync(Guid companyId);
    
    /// <summary>
    /// Generate next payment number
    /// </summary>
    Task<string> GeneratePaymentNumberAsync(Guid companyId);
}

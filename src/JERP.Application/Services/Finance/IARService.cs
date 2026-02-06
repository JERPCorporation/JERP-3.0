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
using JERP.Core.Enums;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for accounts receivable management
/// </summary>
public interface IARService
{
    /// <summary>
    /// Get invoice by ID with line items
    /// </summary>
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);
    
    /// <summary>
    /// Get invoices for a company
    /// </summary>
    Task<List<InvoiceListDto>> GetInvoicesAsync(Guid companyId, InvoiceStatus? status = null);
    
    /// <summary>
    /// Create a new invoice and post to GL
    /// </summary>
    Task<InvoiceDto> CreateInvoiceAsync(Guid companyId, CreateInvoiceDto dto);
    
    /// <summary>
    /// Update an existing invoice (only if Draft)
    /// </summary>
    Task<InvoiceDto> UpdateInvoiceAsync(Guid id, UpdateInvoiceDto dto);
    
    /// <summary>
    /// Send invoice to customer (change status to Sent)
    /// </summary>
    Task<InvoiceDto> SendInvoiceAsync(Guid id);
    
    /// <summary>
    /// Record a payment for an invoice
    /// </summary>
    Task<Guid> RecordPaymentAsync(Guid companyId, CreateInvoicePaymentDto dto);
    
    /// <summary>
    /// Void an invoice and reverse GL entries
    /// </summary>
    Task VoidInvoiceAsync(Guid id);
    
    /// <summary>
    /// Generate next invoice number
    /// </summary>
    Task<string> GenerateInvoiceNumberAsync(Guid companyId);
    
    /// <summary>
    /// Generate next receipt number
    /// </summary>
    Task<string> GenerateReceiptNumberAsync(Guid companyId);
}

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

namespace JERP.Application.Services.Payroll.Pdf;

/// <summary>
/// Interface for PDF generation services
/// </summary>
public interface IPdfGenerationService
{
    /// <summary>
    /// Generates a pay stub PDF for a payroll record
    /// </summary>
    Task<byte[]> GeneratePayStubAsync(Guid payrollRecordId);

    /// <summary>
    /// Generates pay stubs for all records in a pay period
    /// </summary>
    Task<byte[]> GenerateBulkPayStubsAsync(Guid payPeriodId);
}

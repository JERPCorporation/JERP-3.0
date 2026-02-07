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

namespace JERP.Application.Services.Finance;

/// <summary>
/// Interface for posting payroll transactions to the general ledger
/// </summary>
public interface IPayrollToFinanceService
{
    /// <summary>
    /// Posts a payroll record to the general ledger
    /// </summary>
    /// <param name="payrollRecordId">The ID of the payroll record to post</param>
    /// <returns>The journal entry ID created</returns>
    Task<Guid> PostPayrollToGLAsync(Guid payrollRecordId);
}

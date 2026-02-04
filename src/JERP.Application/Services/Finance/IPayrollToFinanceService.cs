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

using JERP.Core.Entities;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for automatically posting payroll transactions to the general ledger
/// </summary>
public interface IPayrollToFinanceService
{
    /// <summary>
    /// Create journal entries for a payroll when it's approved
    /// </summary>
    Task PostPayrollToGeneralLedgerAsync(Guid payPeriodId);

    /// <summary>
    /// Create journal entry for a single payroll record
    /// </summary>
    Task PostPayrollRecordToGeneralLedgerAsync(PayrollRecord payrollRecord);
}

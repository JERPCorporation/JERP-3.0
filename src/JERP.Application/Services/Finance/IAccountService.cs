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

using JERP.Core.Entities.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for managing the Chart of Accounts
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Get all accounts for a company
    /// </summary>
    Task<List<Account>> GetAccountsAsync(Guid companyId);

    /// <summary>
    /// Get account by ID
    /// </summary>
    Task<Account?> GetAccountByIdAsync(Guid id);

    /// <summary>
    /// Get account by account code
    /// </summary>
    Task<Account?> GetAccountByCodeAsync(Guid companyId, string accountCode);

    /// <summary>
    /// Create a new account
    /// </summary>
    Task<Account> CreateAccountAsync(Account account);

    /// <summary>
    /// Update an existing account
    /// </summary>
    Task<Account> UpdateAccountAsync(Account account);

    /// <summary>
    /// Delete an account (soft delete)
    /// </summary>
    Task DeleteAccountAsync(Guid id);

    /// <summary>
    /// Get account balance
    /// </summary>
    Task<decimal> GetAccountBalanceAsync(Guid accountId);
}

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
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Implementation of account service
/// </summary>
public class AccountService : IAccountService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        JerpDbContext context,
        ILogger<AccountService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Account>> GetAccountsAsync(Guid companyId)
    {
        return await _context.Accounts
            .Where(a => a.CompanyId == companyId && !a.IsDeleted)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<Account?> GetAccountByCodeAsync(Guid companyId, string accountCode)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && 
                                     a.AccountCode == accountCode && 
                                     !a.IsDeleted);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        _logger.LogInformation("Creating account {AccountCode} for company {CompanyId}", 
            account.AccountCode, account.CompanyId);

        // Check for duplicate account code
        var existing = await GetAccountByCodeAsync(account.CompanyId, account.AccountCode);
        if (existing != null)
        {
            throw new InvalidOperationException($"Account with code {account.AccountCode} already exists");
        }

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Account created: {AccountId}", account.Id);

        return account;
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        _logger.LogInformation("Updating account {AccountId}", account.Id);

        var existing = await GetAccountByIdAsync(account.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Account {account.Id} not found");
        }

        _context.Entry(existing).CurrentValues.SetValues(account);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Account updated: {AccountId}", account.Id);

        return existing;
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        _logger.LogInformation("Deleting account {AccountId}", id);

        var account = await GetAccountByIdAsync(id);
        if (account == null)
        {
            throw new InvalidOperationException($"Account {id} not found");
        }

        // Check if account has transactions
        var hasTransactions = await _context.GeneralLedgerEntries
            .AnyAsync(e => e.AccountId == id && !e.IsDeleted);

        if (hasTransactions)
        {
            throw new InvalidOperationException("Cannot delete account with transactions. Consider marking it inactive instead.");
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Account deleted: {AccountId}", id);
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        var account = await GetAccountByIdAsync(accountId);
        return account?.Balance ?? 0;
    }
}

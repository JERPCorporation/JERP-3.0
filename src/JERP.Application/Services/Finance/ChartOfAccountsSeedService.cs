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
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for seeding the cannabis-specific chart of accounts
/// </summary>
public class ChartOfAccountsSeedService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<ChartOfAccountsSeedService> _logger;

    public ChartOfAccountsSeedService(
        JerpDbContext context,
        ILogger<ChartOfAccountsSeedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the standard chart of accounts for a new company
    /// </summary>
    public async Task SeedChartOfAccountsAsync(Guid companyId)
    {
        // Check if accounts already exist
        var existingAccounts = await _context.Accounts
            .Where(a => a.CompanyId == companyId)
            .AnyAsync();

        if (existingAccounts)
        {
            _logger.LogInformation("Chart of accounts already exists for company {CompanyId}", companyId);
            return;
        }

        var accounts = new List<Account>
        {
            // Assets (1000-1999)
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "1000",
                AccountName = "Cash - Operating",
                Type = AccountType.Asset,
                SubType = AccountSubType.Cash,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "1010",
                AccountName = "Cash - Vault (Cannabis)",
                Type = AccountType.Asset,
                SubType = AccountSubType.Cash,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false,
                TaxCategory = "Cannabis Cash Holdings"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "1020",
                AccountName = "Bank - Checking",
                Type = AccountType.Asset,
                SubType = AccountSubType.BankAccount,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "1200",
                AccountName = "Accounts Receivable",
                Type = AccountType.Asset,
                SubType = AccountSubType.AccountsReceivable,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },

            // Liabilities (2000-2999)
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "2000",
                AccountName = "Accounts Payable",
                Type = AccountType.Liability,
                SubType = AccountSubType.AccountsPayable,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "2100",
                AccountName = "Payroll Tax Liability",
                Type = AccountType.Liability,
                SubType = AccountSubType.PayrollLiability,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "2110",
                AccountName = "Sales Tax Liability",
                Type = AccountType.Liability,
                SubType = AccountSubType.TaxPayable,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "2120",
                AccountName = "Excise Tax Liability (Cannabis)",
                Type = AccountType.Liability,
                SubType = AccountSubType.TaxPayable,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false,
                TaxCategory = "Cannabis Excise Tax"
            },

            // Equity (3000-3999)
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "3000",
                AccountName = "Owner's Equity",
                Type = AccountType.Equity,
                SubType = AccountSubType.OwnersEquity,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "3900",
                AccountName = "Retained Earnings",
                Type = AccountType.Equity,
                SubType = AccountSubType.RetainedEarnings,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false
            },

            // Revenue (4000-4999)
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "4000",
                AccountName = "Cannabis Sales - Flower",
                Type = AccountType.Revenue,
                SubType = AccountSubType.ProductSales,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false,
                TaxCategory = "Cannabis Revenue"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "4010",
                AccountName = "Cannabis Sales - Edibles",
                Type = AccountType.Revenue,
                SubType = AccountSubType.ProductSales,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false,
                TaxCategory = "Cannabis Revenue"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "4020",
                AccountName = "Cannabis Sales - Concentrates",
                Type = AccountType.Revenue,
                SubType = AccountSubType.ProductSales,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = false,
                TaxCategory = "Cannabis Revenue"
            },

            // COGS (5000-5099) - 280E Deductible
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5000",
                AccountName = "COGS - Cannabis Product",
                Type = AccountType.Expense,
                SubType = AccountSubType.COGS,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = true,
                IsNonDeductible = false,
                TaxCategory = "280E Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5010",
                AccountName = "COGS - Cultivation Labor",
                Type = AccountType.Expense,
                SubType = AccountSubType.COGS,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = true,
                IsNonDeductible = false,
                TaxCategory = "280E Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5020",
                AccountName = "COGS - Packaging",
                Type = AccountType.Expense,
                SubType = AccountSubType.COGS,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = true,
                IsNonDeductible = false,
                TaxCategory = "280E Deductible"
            },

            // Expenses (5100-5999) - 280E Non-Deductible
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5100",
                AccountName = "Payroll Expense - Budtenders",
                Type = AccountType.Expense,
                SubType = AccountSubType.PayrollExpense,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5110",
                AccountName = "Payroll Expense - Security",
                Type = AccountType.Expense,
                SubType = AccountSubType.PayrollExpense,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5120",
                AccountName = "Payroll Tax Expense",
                Type = AccountType.Expense,
                SubType = AccountSubType.PayrollTaxExpense,
                IsActive = true,
                IsSystemAccount = true,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5200",
                AccountName = "Rent Expense",
                Type = AccountType.Expense,
                SubType = AccountSubType.RentExpense,
                IsActive = true,
                IsSystemAccount = false,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5300",
                AccountName = "Utilities Expense",
                Type = AccountType.Expense,
                SubType = AccountSubType.UtilitiesExpense,
                IsActive = true,
                IsSystemAccount = false,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            },
            new Account
            {
                CompanyId = companyId,
                AccountNumber = "5400",
                AccountName = "Marketing & Advertising",
                Type = AccountType.Expense,
                SubType = AccountSubType.MarketingExpense,
                IsActive = true,
                IsSystemAccount = false,
                IsCOGS = false,
                IsNonDeductible = true,
                TaxCategory = "280E Non-Deductible"
            }
        };

        _context.Accounts.AddRange(accounts);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} accounts for company {CompanyId}", accounts.Count, companyId);
    }
}

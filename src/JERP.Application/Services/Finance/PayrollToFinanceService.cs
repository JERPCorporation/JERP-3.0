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
/// Service for posting payroll transactions to the general ledger
/// </summary>
public class PayrollToFinanceService : IPayrollToFinanceService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<PayrollToFinanceService> _logger;

    public PayrollToFinanceService(
        JerpDbContext context,
        ILogger<PayrollToFinanceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Posts a payroll record to the general ledger
    /// </summary>
    public async Task<Guid> PostPayrollToGLAsync(Guid payrollRecordId)
    {
        // Get payroll record with all details
        var payrollRecord = await _context.PayrollRecords
            .Include(pr => pr.Employee)
            .Include(pr => pr.PayPeriod)
            .FirstOrDefaultAsync(pr => pr.Id == payrollRecordId);

        if (payrollRecord == null)
        {
            throw new InvalidOperationException($"Payroll record {payrollRecordId} not found");
        }

        // Verify status is Approved
        if (payrollRecord.Status != PayrollStatus.Approved)
        {
            throw new InvalidOperationException($"Payroll record {payrollRecordId} must be approved before posting to GL. Current status: {payrollRecord.Status}");
        }

        var companyId = payrollRecord.Employee.CompanyId;

        // Get GL accounts
        var accounts = await GetGLAccountsAsync(companyId);

        // Create journal entry
        var journalEntry = new JournalEntry
        {
            CompanyId = companyId,
            JournalEntryNumber = await GenerateJournalEntryNumberAsync(companyId),
            TransactionDate = payrollRecord.PayPeriod.EndDate,
            Description = $"Payroll for {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName} - {payrollRecord.PayPeriod.StartDate:MM/dd/yyyy} to {payrollRecord.PayPeriod.EndDate:MM/dd/yyyy}",
            Status = JournalEntryStatus.Posted,
            Source = EntrySource.Payroll,
            PostedAt = DateTime.UtcNow
        };

        var ledgerEntries = new List<GeneralLedgerEntry>();

        // Calculate employer taxes (simplified - in real system would be calculated properly)
        var employerSocialSecurity = payrollRecord.SocialSecurityTax; // Employer matches employee
        var employerMedicare = payrollRecord.MedicareTax; // Employer matches employee
        var employerTaxes = employerSocialSecurity + employerMedicare;

        // Dr. Payroll Expense (gross wages)
        ledgerEntries.Add(new GeneralLedgerEntry
        {
            CompanyId = companyId,
            AccountId = accounts.PayrollExpenseAccount.Id,
            TransactionDate = journalEntry.TransactionDate,
            DebitAmount = payrollRecord.GrossPay,
            CreditAmount = 0,
            Description = $"Gross wages - {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName}",
            Source = EntrySource.Payroll,
            SourceEntityId = payrollRecordId,
            Is280EDeductible = accounts.PayrollExpenseAccount.IsCOGS
        });

        // Dr. Payroll Tax Expense (employer taxes)
        ledgerEntries.Add(new GeneralLedgerEntry
        {
            CompanyId = companyId,
            AccountId = accounts.PayrollTaxExpenseAccount.Id,
            TransactionDate = journalEntry.TransactionDate,
            DebitAmount = employerTaxes,
            CreditAmount = 0,
            Description = $"Employer payroll taxes - {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName}",
            Source = EntrySource.Payroll,
            SourceEntityId = payrollRecordId,
            Is280EDeductible = accounts.PayrollTaxExpenseAccount.IsCOGS
        });

        // Cr. Cash (net pay)
        ledgerEntries.Add(new GeneralLedgerEntry
        {
            CompanyId = companyId,
            AccountId = accounts.CashAccount.Id,
            TransactionDate = journalEntry.TransactionDate,
            DebitAmount = 0,
            CreditAmount = payrollRecord.NetPay,
            Description = $"Net pay - {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName}",
            Source = EntrySource.Payroll,
            SourceEntityId = payrollRecordId,
            Is280EDeductible = false
        });

        // Cr. Tax Liability (employee + employer taxes)
        var totalTaxLiability = payrollRecord.TotalTaxes + employerTaxes;
        ledgerEntries.Add(new GeneralLedgerEntry
        {
            CompanyId = companyId,
            AccountId = accounts.PayrollTaxLiabilityAccount.Id,
            TransactionDate = journalEntry.TransactionDate,
            DebitAmount = 0,
            CreditAmount = totalTaxLiability,
            Description = $"Payroll tax liability - {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName}",
            Source = EntrySource.Payroll,
            SourceEntityId = payrollRecordId,
            Is280EDeductible = false
        });

        // Cr. Benefits Liability (if there are deductions)
        if (payrollRecord.TotalDeductions > 0)
        {
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = accounts.PayrollLiabilityAccount.Id,
                TransactionDate = journalEntry.TransactionDate,
                DebitAmount = 0,
                CreditAmount = payrollRecord.TotalDeductions,
                Description = $"Benefits/deductions liability - {payrollRecord.Employee.FirstName} {payrollRecord.Employee.LastName}",
                Source = EntrySource.Payroll,
                SourceEntityId = payrollRecordId,
                Is280EDeductible = false
            });
        }

        // Calculate totals
        journalEntry.TotalDebit = ledgerEntries.Sum(e => e.DebitAmount);
        journalEntry.TotalCredit = ledgerEntries.Sum(e => e.CreditAmount);

        // Validate debits == credits
        if (journalEntry.TotalDebit != journalEntry.TotalCredit)
        {
            throw new InvalidOperationException($"Journal entry is not balanced. Debits: {journalEntry.TotalDebit}, Credits: {journalEntry.TotalCredit}");
        }

        // Save journal entry and GL entries
        _context.JournalEntries.Add(journalEntry);
        await _context.SaveChangesAsync();

        // Link ledger entries to journal entry
        foreach (var entry in ledgerEntries)
        {
            entry.JournalEntryId = journalEntry.Id;
        }

        _context.GeneralLedgerEntries.AddRange(ledgerEntries);
        await _context.SaveChangesAsync();

        // Update account balances
        await UpdateAccountBalancesAsync(ledgerEntries);

        _logger.LogInformation("Posted payroll record {PayrollRecordId} to GL as journal entry {JournalEntryId}",
            payrollRecordId, journalEntry.Id);

        return journalEntry.Id;
    }

    private async Task<GLAccounts> GetGLAccountsAsync(Guid companyId)
    {
        var accounts = new GLAccounts
        {
            CashAccount = await GetAccountByNumberAsync(companyId, "1000"),
            PayrollExpenseAccount = await GetAccountByNumberAsync(companyId, "5100"),
            PayrollTaxExpenseAccount = await GetAccountByNumberAsync(companyId, "5120"),
            PayrollTaxLiabilityAccount = await GetAccountByNumberAsync(companyId, "2100"),
            PayrollLiabilityAccount = await GetAccountByNumberAsync(companyId, "2000")
        };

        return accounts;
    }

    private async Task<Account> GetAccountByNumberAsync(Guid companyId, string accountNumber)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountNumber == accountNumber);

        if (account == null)
        {
            throw new InvalidOperationException($"Account {accountNumber} not found for company {companyId}. Please ensure the chart of accounts is properly configured.");
        }

        return account;
    }

    private async Task<string> GenerateJournalEntryNumberAsync(Guid companyId)
    {
        var lastEntry = await _context.JournalEntries
            .Where(j => j.CompanyId == companyId)
            .OrderByDescending(j => j.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastEntry == null)
        {
            return "JE-0001";
        }

        // Extract number from last entry (e.g., "JE-0001" -> 1)
        var lastNumber = int.Parse(lastEntry.JournalEntryNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"JE-{nextNumber:D4}";
    }

    private async Task UpdateAccountBalancesAsync(List<GeneralLedgerEntry> entries)
    {
        var accountIds = entries.Select(e => e.AccountId).Distinct();

        foreach (var accountId in accountIds)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) continue;

            var accountEntries = entries.Where(e => e.AccountId == accountId);
            var debitTotal = accountEntries.Sum(e => e.DebitAmount);
            var creditTotal = accountEntries.Sum(e => e.CreditAmount);

            // Update balance based on account type
            switch (account.Type)
            {
                case AccountType.Asset:
                case AccountType.Expense:
                    // Assets and Expenses increase with debits, decrease with credits
                    account.Balance += debitTotal - creditTotal;
                    break;
                case AccountType.Liability:
                case AccountType.Equity:
                case AccountType.Revenue:
                    // Liabilities, Equity, and Revenue increase with credits, decrease with debits
                    account.Balance += creditTotal - debitTotal;
                    break;
            }
        }

        await _context.SaveChangesAsync();
    }

    private class GLAccounts
    {
        public Account CashAccount { get; set; } = null!;
        public Account PayrollExpenseAccount { get; set; } = null!;
        public Account PayrollTaxExpenseAccount { get; set; } = null!;
        public Account PayrollTaxLiabilityAccount { get; set; } = null!;
        public Account PayrollLiabilityAccount { get; set; } = null!;
    }
}

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
using JERP.Application.Services.Security;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for accounts payable management
/// </summary>
public class APService : IAPService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<APService> _logger;

    public APService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<APService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get bill by ID with line items
    /// </summary>
    public async Task<BillDto?> GetBillByIdAsync(Guid id)
    {
        var bill = await _context.VendorBills
            .Include(b => b.Vendor)
            .Include(b => b.LineItems)
                .ThenInclude(li => li.Account)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null) return null;

        return new BillDto
        {
            Id = bill.Id,
            CompanyId = bill.CompanyId,
            VendorId = bill.VendorId,
            BillNumber = bill.BillNumber,
            VendorInvoiceNumber = bill.VendorInvoiceNumber,
            BillDate = bill.BillDate,
            DueDate = bill.DueDate,
            Subtotal = bill.Subtotal,
            TaxAmount = bill.TaxAmount,
            TotalAmount = bill.TotalAmount,
            AmountPaid = bill.AmountPaid,
            StatusEnum = bill.Status,
            Status = bill.Status.ToString(),
            IsPaid = bill.IsPaid,
            PaymentDate = bill.PaymentDate,
            JournalEntryId = bill.JournalEntryId,
            Notes = bill.Notes,
            VendorName = bill.Vendor.CompanyName,
            LineItems = bill.LineItems.Select(li => new BillLineItemDto
            {
                Id = li.Id,
                AccountId = li.AccountId,
                AccountName = li.Account.AccountName,
                AccountNumber = li.Account.AccountNumber,
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                Amount = li.Amount,
                IsCOGS = li.IsCOGS
            }).ToList(),
            CreatedAt = bill.CreatedAt,
            UpdatedAt = bill.UpdatedAt
        };
    }

    /// <summary>
    /// Get bills for a company
    /// </summary>
    public async Task<List<BillListDto>> GetBillsAsync(Guid companyId, BillStatus? status = null)
    {
        var query = _context.VendorBills
            .Include(b => b.Vendor)
            .Where(b => b.CompanyId == companyId);

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        return await query
            .OrderByDescending(b => b.BillDate)
            .Select(b => new BillListDto
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                VendorInvoiceNumber = b.VendorInvoiceNumber,
                VendorId = b.VendorId,
                VendorName = b.Vendor.CompanyName,
                BillDate = b.BillDate,
                DueDate = b.DueDate,
                TotalAmount = b.TotalAmount,
                AmountPaid = b.AmountPaid,
                StatusEnum = b.Status,
                Status = b.Status.ToString()
            })
            .ToListAsync();
    }

    /// <summary>
    /// Create a new bill
    /// </summary>
    public async Task<BillDto> CreateBillAsync(Guid companyId, CreateBillDto dto)
    {
        // Validate
        if (dto.DueDate <= dto.BillDate)
        {
            throw new InvalidOperationException("Due date must be after bill date");
        }

        if (!dto.LineItems.Any())
        {
            throw new InvalidOperationException("Bill must have at least one line item");
        }

        var billNumber = await GenerateBillNumberAsync(companyId);

        // Calculate subtotal
        var subtotal = dto.LineItems.Sum(li => li.Quantity * li.UnitPrice);

        var bill = new VendorBill
        {
            CompanyId = companyId,
            VendorId = dto.VendorId,
            BillNumber = billNumber,
            VendorInvoiceNumber = dto.VendorInvoiceNumber,
            BillDate = dto.BillDate,
            DueDate = dto.DueDate,
            Subtotal = subtotal,
            TaxAmount = dto.TaxAmount,
            TotalAmount = subtotal + dto.TaxAmount,
            AmountPaid = 0,
            Status = BillStatus.Draft,
            IsPaid = false,
            Notes = dto.Notes
        };

        _context.VendorBills.Add(bill);
        await _context.SaveChangesAsync();

        // Create line items
        foreach (var lineDto in dto.LineItems)
        {
            var account = await _context.Accounts.FindAsync(lineDto.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException($"Account {lineDto.AccountId} not found");
            }

            var lineItem = new BillLineItem
            {
                BillId = bill.Id,
                AccountId = lineDto.AccountId,
                Description = lineDto.Description,
                Quantity = lineDto.Quantity,
                UnitPrice = lineDto.UnitPrice,
                Amount = lineDto.Quantity * lineDto.UnitPrice,
                IsCOGS = account.IsCOGS
            };

            _context.BillLineItems.Add(lineItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Created bill {BillNumber} for vendor {VendorId}, amount ${TotalAmount}",
            bill.BillNumber, bill.VendorId, bill.TotalAmount);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "bill_created",
                $"Bill:{bill.Id}",
                $"Created bill {bill.BillNumber} for ${bill.TotalAmount:N2}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for bill creation");
        }

        return (await GetBillByIdAsync(bill.Id))!;
    }

    /// <summary>
    /// Update an existing bill (only if Draft)
    /// </summary>
    public async Task<BillDto> UpdateBillAsync(Guid id, UpdateBillDto dto)
    {
        var bill = await _context.VendorBills
            .Include(b => b.LineItems)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null)
        {
            throw new InvalidOperationException($"Bill {id} not found");
        }

        if (bill.Status != BillStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot update bill {bill.BillNumber} with status {bill.Status}. Only Draft bills can be updated.");
        }

        // Validate
        if (dto.DueDate <= dto.BillDate)
        {
            throw new InvalidOperationException("Due date must be after bill date");
        }

        // Update bill
        bill.VendorId = dto.VendorId;
        bill.VendorInvoiceNumber = dto.VendorInvoiceNumber;
        bill.BillDate = dto.BillDate;
        bill.DueDate = dto.DueDate;
        bill.TaxAmount = dto.TaxAmount;
        bill.Notes = dto.Notes;

        // Remove old line items
        _context.BillLineItems.RemoveRange(bill.LineItems);

        // Calculate subtotal and add new line items
        var subtotal = dto.LineItems.Sum(li => li.Quantity * li.UnitPrice);
        bill.Subtotal = subtotal;
        bill.TotalAmount = subtotal + dto.TaxAmount;

        foreach (var lineDto in dto.LineItems)
        {
            var account = await _context.Accounts.FindAsync(lineDto.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException($"Account {lineDto.AccountId} not found");
            }

            var lineItem = new BillLineItem
            {
                BillId = bill.Id,
                AccountId = lineDto.AccountId,
                Description = lineDto.Description,
                Quantity = lineDto.Quantity,
                UnitPrice = lineDto.UnitPrice,
                Amount = lineDto.Quantity * lineDto.UnitPrice,
                IsCOGS = account.IsCOGS
            };

            _context.BillLineItems.Add(lineItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated bill {BillNumber}", bill.BillNumber);

        try
        {
            await _auditLogService.LogAction(
                bill.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "bill_updated",
                $"Bill:{bill.Id}",
                $"Updated bill {bill.BillNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for bill update");
        }

        return (await GetBillByIdAsync(bill.Id))!;
    }

    /// <summary>
    /// Approve a bill and post to GL
    /// </summary>
    public async Task<BillDto> ApproveBillAsync(Guid id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var bill = await _context.VendorBills
                .Include(b => b.Vendor)
                .Include(b => b.LineItems)
                    .ThenInclude(li => li.Account)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                throw new InvalidOperationException($"Bill {id} not found");
            }

            if (bill.Status != BillStatus.Draft && bill.Status != BillStatus.Pending)
            {
                throw new InvalidOperationException($"Cannot approve bill {bill.BillNumber} with status {bill.Status}");
            }

            // Update bill status
            bill.Status = BillStatus.Approved;

            // Create journal entry
            var journalEntry = new JournalEntry
            {
                CompanyId = bill.CompanyId,
                JournalEntryNumber = await GenerateJournalEntryNumberAsync(bill.CompanyId),
                TransactionDate = bill.BillDate,
                Description = $"Bill {bill.BillNumber} - {bill.Vendor.CompanyName}",
                Status = JournalEntryStatus.Posted,
                Source = EntrySource.Bill,
                PostedAt = DateTime.UtcNow
            };

            var ledgerEntries = new List<GeneralLedgerEntry>();

            // Dr. Expense accounts (per line items)
            foreach (var lineItem in bill.LineItems)
            {
                ledgerEntries.Add(new GeneralLedgerEntry
                {
                    CompanyId = bill.CompanyId,
                    AccountId = lineItem.AccountId,
                    TransactionDate = bill.BillDate,
                    DebitAmount = lineItem.Amount,
                    CreditAmount = 0,
                    Description = $"{lineItem.Description} - Bill {bill.BillNumber}",
                    Source = EntrySource.Bill,
                    SourceEntityId = bill.Id,
                    Is280EDeductible = lineItem.Account.IsCOGS
                });
            }

            // Cr. Accounts Payable
            var apAccountId = bill.Vendor.AccountsPayableAccountId ?? await GetDefaultAPAccountIdAsync(bill.CompanyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = bill.CompanyId,
                AccountId = apAccountId,
                TransactionDate = bill.BillDate,
                DebitAmount = 0,
                CreditAmount = bill.TotalAmount,
                Description = $"Bill {bill.BillNumber} - {bill.Vendor.CompanyName}",
                Source = EntrySource.Bill,
                SourceEntityId = bill.Id,
                Is280EDeductible = false
            });

            // Calculate totals
            journalEntry.TotalDebit = ledgerEntries.Sum(e => e.DebitAmount);
            journalEntry.TotalCredit = ledgerEntries.Sum(e => e.CreditAmount);

            // Validate debits == credits
            if (journalEntry.TotalDebit != journalEntry.TotalCredit)
            {
                throw new InvalidOperationException($"Journal entry is not balanced. Debits: {journalEntry.TotalDebit}, Credits: {journalEntry.TotalCredit}");
            }

            // Save journal entry
            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            // Link ledger entries to journal entry
            foreach (var entry in ledgerEntries)
            {
                entry.JournalEntryId = journalEntry.Id;
            }

            _context.GeneralLedgerEntries.AddRange(ledgerEntries);
            bill.JournalEntryId = journalEntry.Id;

            // Update vendor balance
            bill.Vendor.Balance += bill.TotalAmount;

            // Update account balances
            await UpdateAccountBalancesAsync(ledgerEntries);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Approved bill {BillNumber} and posted to GL as journal entry {JournalEntryNumber}",
                bill.BillNumber, journalEntry.JournalEntryNumber);

            try
            {
                await _auditLogService.LogAction(
                    bill.CompanyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "bill_approved",
                    $"Bill:{bill.Id}",
                    $"Approved bill {bill.BillNumber} and posted to GL. Journal Entry: {journalEntry.JournalEntryNumber}",
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for bill approval");
            }

            return (await GetBillByIdAsync(bill.Id))!;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Create a payment for a bill
    /// </summary>
    public async Task<Guid> CreatePaymentAsync(Guid companyId, CreateBillPaymentDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var bill = await _context.VendorBills
                .Include(b => b.Vendor)
                .FirstOrDefaultAsync(b => b.Id == dto.BillId);

            if (bill == null)
            {
                throw new InvalidOperationException($"Bill {dto.BillId} not found");
            }

            if (bill.Status != BillStatus.Approved && bill.Status != BillStatus.Paid)
            {
                throw new InvalidOperationException($"Cannot pay bill {bill.BillNumber} with status {bill.Status}. Bill must be approved first.");
            }

            var amountRemaining = bill.TotalAmount - bill.AmountPaid;
            if (dto.Amount > amountRemaining)
            {
                throw new InvalidOperationException($"Payment amount ${dto.Amount} exceeds amount remaining ${amountRemaining}");
            }

            var paymentNumber = await GeneratePaymentNumberAsync(companyId);

            var payment = new BillPayment
            {
                CompanyId = companyId,
                BillId = dto.BillId,
                PaymentNumber = paymentNumber,
                PaymentDate = dto.PaymentDate,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                ReferenceNumber = dto.ReferenceNumber,
                Notes = dto.Notes
            };

            _context.BillPayments.Add(payment);
            await _context.SaveChangesAsync();

            // Create journal entry for payment
            var journalEntry = new JournalEntry
            {
                CompanyId = companyId,
                JournalEntryNumber = await GenerateJournalEntryNumberAsync(companyId),
                TransactionDate = dto.PaymentDate,
                Description = $"Payment {payment.PaymentNumber} for Bill {bill.BillNumber} - {bill.Vendor.CompanyName}",
                Status = JournalEntryStatus.Posted,
                Source = EntrySource.Payment,
                PostedAt = DateTime.UtcNow
            };

            var ledgerEntries = new List<GeneralLedgerEntry>();

            // Dr. Accounts Payable
            var apAccountId = bill.Vendor.AccountsPayableAccountId ?? await GetDefaultAPAccountIdAsync(companyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = apAccountId,
                TransactionDate = dto.PaymentDate,
                DebitAmount = dto.Amount,
                CreditAmount = 0,
                Description = $"Payment {payment.PaymentNumber} - Bill {bill.BillNumber}",
                Source = EntrySource.Payment,
                SourceEntityId = payment.Id,
                Is280EDeductible = false
            });

            // Cr. Cash
            var cashAccountId = await GetDefaultCashAccountIdAsync(companyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = cashAccountId,
                TransactionDate = dto.PaymentDate,
                DebitAmount = 0,
                CreditAmount = dto.Amount,
                Description = $"Payment {payment.PaymentNumber} - Bill {bill.BillNumber}",
                Source = EntrySource.Payment,
                SourceEntityId = payment.Id,
                Is280EDeductible = false
            });

            journalEntry.TotalDebit = ledgerEntries.Sum(e => e.DebitAmount);
            journalEntry.TotalCredit = ledgerEntries.Sum(e => e.CreditAmount);

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            foreach (var entry in ledgerEntries)
            {
                entry.JournalEntryId = journalEntry.Id;
            }

            _context.GeneralLedgerEntries.AddRange(ledgerEntries);
            payment.JournalEntryId = journalEntry.Id;

            // Update bill
            bill.AmountPaid += dto.Amount;
            if (bill.AmountPaid >= bill.TotalAmount)
            {
                bill.Status = BillStatus.Paid;
                bill.IsPaid = true;
                bill.PaymentDate = dto.PaymentDate;
            }

            // Update vendor balance
            bill.Vendor.Balance -= dto.Amount;

            // Update account balances
            await UpdateAccountBalancesAsync(ledgerEntries);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Created payment {PaymentNumber} for bill {BillNumber}, amount ${Amount}",
                payment.PaymentNumber, bill.BillNumber, dto.Amount);

            try
            {
                await _auditLogService.LogAction(
                    companyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "bill_payment_created",
                    $"Payment:{payment.Id}",
                    $"Created payment {payment.PaymentNumber} for bill {bill.BillNumber}. Amount: ${dto.Amount:N2}",
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for payment");
            }

            return payment.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Create batch payments for multiple bills
    /// </summary>
    public async Task<List<Guid>> CreateBatchPaymentsAsync(Guid companyId, List<CreateBillPaymentDto> payments)
    {
        var paymentIds = new List<Guid>();

        foreach (var payment in payments)
        {
            var paymentId = await CreatePaymentAsync(companyId, payment);
            paymentIds.Add(paymentId);
        }

        _logger.LogInformation("Created batch of {Count} payments for company {CompanyId}",
            payments.Count, companyId);

        return paymentIds;
    }

    /// <summary>
    /// Void a bill and reverse GL entries
    /// </summary>
    public async Task VoidBillAsync(Guid id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var bill = await _context.VendorBills
                .Include(b => b.Vendor)
                .Include(b => b.JournalEntry)
                    .ThenInclude(je => je!.LedgerEntries)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                throw new InvalidOperationException($"Bill {id} not found");
            }

            if (bill.Status == BillStatus.Void)
            {
                throw new InvalidOperationException($"Bill {bill.BillNumber} is already voided");
            }

            if (bill.AmountPaid > 0)
            {
                throw new InvalidOperationException($"Cannot void bill {bill.BillNumber} with payments. Amount paid: ${bill.AmountPaid}");
            }

            // Reverse GL entries if bill was approved
            if (bill.JournalEntry != null)
            {
                var reversalEntry = new JournalEntry
                {
                    CompanyId = bill.CompanyId,
                    JournalEntryNumber = await GenerateJournalEntryNumberAsync(bill.CompanyId),
                    TransactionDate = DateTime.UtcNow,
                    Description = $"VOID - Reversal of Bill {bill.BillNumber}",
                    Status = JournalEntryStatus.Posted,
                    Source = EntrySource.Bill,
                    PostedAt = DateTime.UtcNow
                };

                var reversalEntries = new List<GeneralLedgerEntry>();

                // Reverse each original entry (swap debit/credit)
                foreach (var originalEntry in bill.JournalEntry.LedgerEntries)
                {
                    reversalEntries.Add(new GeneralLedgerEntry
                    {
                        CompanyId = bill.CompanyId,
                        AccountId = originalEntry.AccountId,
                        TransactionDate = DateTime.UtcNow,
                        DebitAmount = originalEntry.CreditAmount,
                        CreditAmount = originalEntry.DebitAmount,
                        Description = $"VOID - Reversal: {originalEntry.Description}",
                        Source = EntrySource.Bill,
                        SourceEntityId = bill.Id,
                        Is280EDeductible = originalEntry.Is280EDeductible
                    });
                }

                reversalEntry.TotalDebit = reversalEntries.Sum(e => e.DebitAmount);
                reversalEntry.TotalCredit = reversalEntries.Sum(e => e.CreditAmount);

                _context.JournalEntries.Add(reversalEntry);
                await _context.SaveChangesAsync();

                foreach (var entry in reversalEntries)
                {
                    entry.JournalEntryId = reversalEntry.Id;
                }

                _context.GeneralLedgerEntries.AddRange(reversalEntries);

                // Update account balances
                await UpdateAccountBalancesAsync(reversalEntries);

                // Update vendor balance
                bill.Vendor.Balance -= bill.TotalAmount;
            }

            bill.Status = BillStatus.Void;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Voided bill {BillNumber}", bill.BillNumber);

            try
            {
                await _auditLogService.LogAction(
                    bill.CompanyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "bill_voided",
                    $"Bill:{bill.Id}",
                    $"Voided bill {bill.BillNumber}",
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for bill void");
            }
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Generate next bill number
    /// </summary>
    public async Task<string> GenerateBillNumberAsync(Guid companyId)
    {
        var lastBill = await _context.VendorBills
            .Where(b => b.CompanyId == companyId)
            .OrderByDescending(b => b.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastBill == null)
        {
            return "AP-0001";
        }

        var lastNumber = int.Parse(lastBill.BillNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"AP-{nextNumber:D4}";
    }

    /// <summary>
    /// Generate next payment number
    /// </summary>
    public async Task<string> GeneratePaymentNumberAsync(Guid companyId)
    {
        var lastPayment = await _context.BillPayments
            .Where(p => p.CompanyId == companyId)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastPayment == null)
        {
            return "PAY-0001";
        }

        var lastNumber = int.Parse(lastPayment.PaymentNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"PAY-{nextNumber:D4}";
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

        var lastNumber = int.Parse(lastEntry.JournalEntryNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"JE-{nextNumber:D4}";
    }

    private async Task<Guid> GetDefaultAPAccountIdAsync(Guid companyId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountNumber == "2000");

        if (account == null)
        {
            throw new InvalidOperationException("Default Accounts Payable account (2000) not found");
        }

        return account.Id;
    }

    private async Task<Guid> GetDefaultCashAccountIdAsync(Guid companyId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountNumber == "1000");

        if (account == null)
        {
            throw new InvalidOperationException("Default Cash account (1000) not found");
        }

        return account.Id;
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

            switch (account.Type)
            {
                case AccountType.Asset:
                case AccountType.Expense:
                    account.Balance += debitTotal - creditTotal;
                    break;
                case AccountType.Liability:
                case AccountType.Equity:
                case AccountType.Revenue:
                    account.Balance += creditTotal - debitTotal;
                    break;
            }
        }

        await _context.SaveChangesAsync();
    }
}

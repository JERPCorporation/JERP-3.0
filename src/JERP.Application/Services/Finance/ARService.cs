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
using JERP.Application.Services.Security;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for accounts receivable management
/// </summary>
public class ARService : IARService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<ARService> _logger;

    public ARService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<ARService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get invoice by ID with line items
    /// </summary>
    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id)
    {
        var invoice = await _context.CustomerInvoices
            .Include(i => i.Customer)
            .Include(i => i.LineItems)
                .ThenInclude(li => li.Account)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null) return null;

        return new InvoiceDto
        {
            Id = invoice.Id,
            CompanyId = invoice.CompanyId,
            CustomerId = invoice.CustomerId,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Subtotal = invoice.Subtotal,
            TaxAmount = invoice.TaxAmount,
            TotalAmount = invoice.TotalAmount,
            AmountPaid = invoice.AmountPaid,
            StatusEnum = invoice.Status,
            Status = invoice.Status.ToString(),
            IsPaid = invoice.IsPaid,
            PaymentDate = invoice.PaymentDate,
            JournalEntryId = invoice.JournalEntryId,
            Notes = invoice.Notes,
            CustomerName = invoice.Customer.CompanyName,
            LineItems = invoice.LineItems.Select(li => new InvoiceLineItemDto
            {
                Id = li.Id,
                AccountId = li.AccountId,
                AccountName = li.Account.AccountName,
                AccountNumber = li.Account.AccountNumber,
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                Amount = li.Amount
            }).ToList(),
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };
    }

    /// <summary>
    /// Get invoices for a company
    /// </summary>
    public async Task<List<InvoiceListDto>> GetInvoicesAsync(Guid companyId, InvoiceStatus? status = null)
    {
        var query = _context.CustomerInvoices
            .Include(i => i.Customer)
            .Where(i => i.CompanyId == companyId);

        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        return await query
            .OrderByDescending(i => i.InvoiceDate)
            .Select(i => new InvoiceListDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer.CompanyName,
                InvoiceDate = i.InvoiceDate,
                DueDate = i.DueDate,
                TotalAmount = i.TotalAmount,
                AmountPaid = i.AmountPaid,
                StatusEnum = i.Status,
                Status = i.Status.ToString()
            })
            .ToListAsync();
    }

    /// <summary>
    /// Create a new invoice and post to GL
    /// </summary>
    public async Task<InvoiceDto> CreateInvoiceAsync(Guid companyId, CreateInvoiceDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Validate
            if (dto.DueDate <= dto.InvoiceDate)
            {
                throw new InvalidOperationException("Due date must be after invoice date");
            }

            if (!dto.LineItems.Any())
            {
                throw new InvalidOperationException("Invoice must have at least one line item");
            }

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer {dto.CustomerId} not found");
            }

            // Calculate subtotal
            var subtotal = dto.LineItems.Sum(li => li.Quantity * li.UnitPrice);
            var totalAmount = subtotal + dto.TaxAmount;

            // Check credit limit
            var availableCredit = customer.CreditLimit - customer.Balance;
            if (totalAmount > availableCredit)
            {
                throw new InvalidOperationException($"Invoice amount ${totalAmount:N2} exceeds available credit ${availableCredit:N2}. Customer credit limit: ${customer.CreditLimit:N2}, current balance: ${customer.Balance:N2}");
            }

            var invoiceNumber = await GenerateInvoiceNumberAsync(companyId);

            var invoice = new CustomerInvoice
            {
                CompanyId = companyId,
                CustomerId = dto.CustomerId,
                InvoiceNumber = invoiceNumber,
                InvoiceDate = dto.InvoiceDate,
                DueDate = dto.DueDate,
                Subtotal = subtotal,
                TaxAmount = dto.TaxAmount,
                TotalAmount = totalAmount,
                AmountPaid = 0,
                Status = InvoiceStatus.Draft,
                IsPaid = false,
                Notes = dto.Notes
            };

            _context.CustomerInvoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Create line items
            foreach (var lineDto in dto.LineItems)
            {
                var account = await _context.Accounts.FindAsync(lineDto.AccountId);
                if (account == null)
                {
                    throw new InvalidOperationException($"Account {lineDto.AccountId} not found");
                }

                var lineItem = new InvoiceLineItem
                {
                    InvoiceId = invoice.Id,
                    AccountId = lineDto.AccountId,
                    Description = lineDto.Description,
                    Quantity = lineDto.Quantity,
                    UnitPrice = lineDto.UnitPrice,
                    Amount = lineDto.Quantity * lineDto.UnitPrice
                };

                _context.InvoiceLineItems.Add(lineItem);
            }

            await _context.SaveChangesAsync();

            // Post to GL automatically
            var journalEntry = new JournalEntry
            {
                CompanyId = companyId,
                JournalEntryNumber = await GenerateJournalEntryNumberAsync(companyId),
                TransactionDate = dto.InvoiceDate,
                Description = $"Invoice {invoice.InvoiceNumber} - {customer.CompanyName}",
                Status = JournalEntryStatus.Posted,
                Source = EntrySource.Invoice,
                PostedAt = DateTime.UtcNow
            };

            var ledgerEntries = new List<GeneralLedgerEntry>();

            // Dr. Accounts Receivable
            var arAccountId = customer.AccountsReceivableAccountId ?? await GetDefaultARAccountIdAsync(companyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = arAccountId,
                TransactionDate = dto.InvoiceDate,
                DebitAmount = invoice.TotalAmount,
                CreditAmount = 0,
                Description = $"Invoice {invoice.InvoiceNumber} - {customer.CompanyName}",
                Source = EntrySource.Invoice,
                SourceEntityId = invoice.Id,
                Is280EDeductible = false
            });

            // Cr. Revenue accounts (per line items)
            var lineItems = await _context.InvoiceLineItems
                .Include(li => li.Account)
                .Where(li => li.InvoiceId == invoice.Id)
                .ToListAsync();

            foreach (var lineItem in lineItems)
            {
                ledgerEntries.Add(new GeneralLedgerEntry
                {
                    CompanyId = companyId,
                    AccountId = lineItem.AccountId,
                    TransactionDate = dto.InvoiceDate,
                    DebitAmount = 0,
                    CreditAmount = lineItem.Amount,
                    Description = $"{lineItem.Description} - Invoice {invoice.InvoiceNumber}",
                    Source = EntrySource.Invoice,
                    SourceEntityId = invoice.Id,
                    Is280EDeductible = false
                });
            }

            journalEntry.TotalDebit = ledgerEntries.Sum(e => e.DebitAmount);
            journalEntry.TotalCredit = ledgerEntries.Sum(e => e.CreditAmount);

            // Validate debits == credits
            if (journalEntry.TotalDebit != journalEntry.TotalCredit)
            {
                throw new InvalidOperationException($"Journal entry is not balanced. Debits: {journalEntry.TotalDebit}, Credits: {journalEntry.TotalCredit}");
            }

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            foreach (var entry in ledgerEntries)
            {
                entry.JournalEntryId = journalEntry.Id;
            }

            _context.GeneralLedgerEntries.AddRange(ledgerEntries);
            invoice.JournalEntryId = journalEntry.Id;

            // Update customer balance
            customer.Balance += invoice.TotalAmount;

            // Update account balances
            await UpdateAccountBalancesAsync(ledgerEntries);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Created invoice {InvoiceNumber} for customer {CustomerId}, amount ${TotalAmount}, posted to GL as {JournalEntryNumber}",
                invoice.InvoiceNumber, invoice.CustomerId, invoice.TotalAmount, journalEntry.JournalEntryNumber);

            try
            {
                await _auditLogService.LogAction(
                    companyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "invoice_created",
                    $"Invoice:{invoice.Id}",
                    $"Created invoice {invoice.InvoiceNumber} for ${invoice.TotalAmount:N2} and posted to GL",
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for invoice creation");
            }

            return (await GetInvoiceByIdAsync(invoice.Id))!;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Update an existing invoice (only if Draft)
    /// </summary>
    public async Task<InvoiceDto> UpdateInvoiceAsync(Guid id, UpdateInvoiceDto dto)
    {
        var invoice = await _context.CustomerInvoices
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
        {
            throw new InvalidOperationException($"Invoice {id} not found");
        }

        if (invoice.Status != InvoiceStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot update invoice {invoice.InvoiceNumber} with status {invoice.Status}. Only Draft invoices can be updated.");
        }

        // Validate
        if (dto.DueDate <= dto.InvoiceDate)
        {
            throw new InvalidOperationException("Due date must be after invoice date");
        }

        // Update invoice
        invoice.CustomerId = dto.CustomerId;
        invoice.InvoiceDate = dto.InvoiceDate;
        invoice.DueDate = dto.DueDate;
        invoice.TaxAmount = dto.TaxAmount;
        invoice.Notes = dto.Notes;

        // Remove old line items
        _context.InvoiceLineItems.RemoveRange(invoice.LineItems);

        // Calculate subtotal and add new line items
        var subtotal = dto.LineItems.Sum(li => li.Quantity * li.UnitPrice);
        invoice.Subtotal = subtotal;
        invoice.TotalAmount = subtotal + dto.TaxAmount;

        foreach (var lineDto in dto.LineItems)
        {
            var account = await _context.Accounts.FindAsync(lineDto.AccountId);
            if (account == null)
            {
                throw new InvalidOperationException($"Account {lineDto.AccountId} not found");
            }

            var lineItem = new InvoiceLineItem
            {
                InvoiceId = invoice.Id,
                AccountId = lineDto.AccountId,
                Description = lineDto.Description,
                Quantity = lineDto.Quantity,
                UnitPrice = lineDto.UnitPrice,
                Amount = lineDto.Quantity * lineDto.UnitPrice
            };

            _context.InvoiceLineItems.Add(lineItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated invoice {InvoiceNumber}", invoice.InvoiceNumber);

        try
        {
            await _auditLogService.LogAction(
                invoice.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "invoice_updated",
                $"Invoice:{invoice.Id}",
                $"Updated invoice {invoice.InvoiceNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for invoice update");
        }

        return (await GetInvoiceByIdAsync(invoice.Id))!;
    }

    /// <summary>
    /// Send invoice to customer (change status to Sent)
    /// </summary>
    public async Task<InvoiceDto> SendInvoiceAsync(Guid id)
    {
        var invoice = await _context.CustomerInvoices.FindAsync(id);
        if (invoice == null)
        {
            throw new InvalidOperationException($"Invoice {id} not found");
        }

        if (invoice.Status != InvoiceStatus.Draft)
        {
            throw new InvalidOperationException($"Cannot send invoice {invoice.InvoiceNumber} with status {invoice.Status}");
        }

        invoice.Status = InvoiceStatus.Sent;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Sent invoice {InvoiceNumber}", invoice.InvoiceNumber);

        try
        {
            await _auditLogService.LogAction(
                invoice.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "invoice_sent",
                $"Invoice:{invoice.Id}",
                $"Sent invoice {invoice.InvoiceNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for invoice send");
        }

        return (await GetInvoiceByIdAsync(invoice.Id))!;
    }

    /// <summary>
    /// Record a payment for an invoice
    /// </summary>
    public async Task<Guid> RecordPaymentAsync(Guid companyId, CreateInvoicePaymentDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var invoice = await _context.CustomerInvoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId);

            if (invoice == null)
            {
                throw new InvalidOperationException($"Invoice {dto.InvoiceId} not found");
            }

            if (invoice.Status == InvoiceStatus.Draft)
            {
                throw new InvalidOperationException($"Cannot record payment for draft invoice {invoice.InvoiceNumber}");
            }

            if (invoice.Status == InvoiceStatus.Void)
            {
                throw new InvalidOperationException($"Cannot record payment for voided invoice {invoice.InvoiceNumber}");
            }

            var amountRemaining = invoice.TotalAmount - invoice.AmountPaid;
            if (dto.Amount > amountRemaining)
            {
                throw new InvalidOperationException($"Payment amount ${dto.Amount} exceeds amount remaining ${amountRemaining}");
            }

            var receiptNumber = await GenerateReceiptNumberAsync(companyId);

            var payment = new InvoicePayment
            {
                CompanyId = companyId,
                InvoiceId = dto.InvoiceId,
                ReceiptNumber = receiptNumber,
                PaymentDate = dto.PaymentDate,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                ReferenceNumber = dto.ReferenceNumber,
                Notes = dto.Notes
            };

            _context.InvoicePayments.Add(payment);
            await _context.SaveChangesAsync();

            // Create journal entry for payment
            var journalEntry = new JournalEntry
            {
                CompanyId = companyId,
                JournalEntryNumber = await GenerateJournalEntryNumberAsync(companyId),
                TransactionDate = dto.PaymentDate,
                Description = $"Receipt {payment.ReceiptNumber} for Invoice {invoice.InvoiceNumber} - {invoice.Customer.CompanyName}",
                Status = JournalEntryStatus.Posted,
                Source = EntrySource.Payment,
                PostedAt = DateTime.UtcNow
            };

            var ledgerEntries = new List<GeneralLedgerEntry>();

            // Dr. Cash
            var cashAccountId = await GetDefaultCashAccountIdAsync(companyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = cashAccountId,
                TransactionDate = dto.PaymentDate,
                DebitAmount = dto.Amount,
                CreditAmount = 0,
                Description = $"Receipt {payment.ReceiptNumber} - Invoice {invoice.InvoiceNumber}",
                Source = EntrySource.Payment,
                SourceEntityId = payment.Id,
                Is280EDeductible = false
            });

            // Cr. Accounts Receivable
            var arAccountId = invoice.Customer.AccountsReceivableAccountId ?? await GetDefaultARAccountIdAsync(companyId);
            ledgerEntries.Add(new GeneralLedgerEntry
            {
                CompanyId = companyId,
                AccountId = arAccountId,
                TransactionDate = dto.PaymentDate,
                DebitAmount = 0,
                CreditAmount = dto.Amount,
                Description = $"Receipt {payment.ReceiptNumber} - Invoice {invoice.InvoiceNumber}",
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

            // Update invoice
            invoice.AmountPaid += dto.Amount;
            if (invoice.AmountPaid >= invoice.TotalAmount)
            {
                invoice.Status = InvoiceStatus.Paid;
                invoice.IsPaid = true;
                invoice.PaymentDate = dto.PaymentDate;
            }

            // Update customer balance
            invoice.Customer.Balance -= dto.Amount;

            // Update account balances
            await UpdateAccountBalancesAsync(ledgerEntries);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Recorded payment {ReceiptNumber} for invoice {InvoiceNumber}, amount ${Amount}",
                payment.ReceiptNumber, invoice.InvoiceNumber, dto.Amount);

            try
            {
                await _auditLogService.LogAction(
                    companyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "invoice_payment_recorded",
                    $"Payment:{payment.Id}",
                    $"Recorded payment {payment.ReceiptNumber} for invoice {invoice.InvoiceNumber}. Amount: ${dto.Amount:N2}",
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
    /// Void an invoice and reverse GL entries
    /// </summary>
    public async Task VoidInvoiceAsync(Guid id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var invoice = await _context.CustomerInvoices
                .Include(i => i.Customer)
                .Include(i => i.JournalEntry)
                    .ThenInclude(je => je!.LedgerEntries)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                throw new InvalidOperationException($"Invoice {id} not found");
            }

            if (invoice.Status == InvoiceStatus.Void)
            {
                throw new InvalidOperationException($"Invoice {invoice.InvoiceNumber} is already voided");
            }

            if (invoice.AmountPaid > 0)
            {
                throw new InvalidOperationException($"Cannot void invoice {invoice.InvoiceNumber} with payments. Amount paid: ${invoice.AmountPaid}");
            }

            // Reverse GL entries
            if (invoice.JournalEntry != null)
            {
                var reversalEntry = new JournalEntry
                {
                    CompanyId = invoice.CompanyId,
                    JournalEntryNumber = await GenerateJournalEntryNumberAsync(invoice.CompanyId),
                    TransactionDate = DateTime.UtcNow,
                    Description = $"VOID - Reversal of Invoice {invoice.InvoiceNumber}",
                    Status = JournalEntryStatus.Posted,
                    Source = EntrySource.Invoice,
                    PostedAt = DateTime.UtcNow
                };

                var reversalEntries = new List<GeneralLedgerEntry>();

                // Reverse each original entry (swap debit/credit)
                foreach (var originalEntry in invoice.JournalEntry.LedgerEntries)
                {
                    reversalEntries.Add(new GeneralLedgerEntry
                    {
                        CompanyId = invoice.CompanyId,
                        AccountId = originalEntry.AccountId,
                        TransactionDate = DateTime.UtcNow,
                        DebitAmount = originalEntry.CreditAmount,
                        CreditAmount = originalEntry.DebitAmount,
                        Description = $"VOID - Reversal: {originalEntry.Description}",
                        Source = EntrySource.Invoice,
                        SourceEntityId = invoice.Id,
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

                // Update customer balance
                invoice.Customer.Balance -= invoice.TotalAmount;
            }

            invoice.Status = InvoiceStatus.Void;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Voided invoice {InvoiceNumber}", invoice.InvoiceNumber);

            try
            {
                await _auditLogService.LogAction(
                    invoice.CompanyId,
                    Guid.Empty,
                    "system@jerp.io",
                    "System",
                    "invoice_voided",
                    $"Invoice:{invoice.Id}",
                    $"Voided invoice {invoice.InvoiceNumber}",
                    null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for invoice void");
            }
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Generate next invoice number
    /// </summary>
    public async Task<string> GenerateInvoiceNumberAsync(Guid companyId)
    {
        var lastInvoice = await _context.CustomerInvoices
            .Where(i => i.CompanyId == companyId)
            .OrderByDescending(i => i.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastInvoice == null)
        {
            return "INV-0001";
        }

        var lastNumber = int.Parse(lastInvoice.InvoiceNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"INV-{nextNumber:D4}";
    }

    /// <summary>
    /// Generate next receipt number
    /// </summary>
    public async Task<string> GenerateReceiptNumberAsync(Guid companyId)
    {
        var lastPayment = await _context.InvoicePayments
            .Where(p => p.CompanyId == companyId)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastPayment == null)
        {
            return "RCPT-0001";
        }

        var lastNumber = int.Parse(lastPayment.ReceiptNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"RCPT-{nextNumber:D4}";
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

    private async Task<Guid> GetDefaultARAccountIdAsync(Guid companyId)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.CompanyId == companyId && a.AccountNumber == "1200");

        if (account == null)
        {
            throw new InvalidOperationException("Default Accounts Receivable account (1200) not found");
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

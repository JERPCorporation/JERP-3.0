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

using JERP.Application.DTOs.PurchaseOrders;
using JERP.Application.Services.Security;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service for vendor bill management
/// </summary>
public class VendorBillService : IVendorBillService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<VendorBillService> _logger;

    public VendorBillService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<VendorBillService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<VendorBillDto?> GetByIdAsync(Guid id)
    {
        return await _context.VendorBills
            .Include(b => b.Vendor)
            .Include(b => b.LineItems)
                .ThenInclude(l => l.Account)
            .Where(b => b.Id == id)
            .Select(b => new VendorBillDto
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                VendorId = b.VendorId,
                VendorName = b.Vendor.CompanyName,
                PurchaseOrderId = null,
                PONumber = null,
                VendorInvoiceNumber = b.VendorInvoiceNumber ?? string.Empty,
                InvoiceDate = b.BillDate,
                DueDate = b.DueDate,
                Status = b.Status.ToString(),
                LineItems = b.LineItems.Select(l => new VendorBillLineDto
                {
                    Id = l.Id,
                    ProductId = null,
                    Description = l.Description,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    LineTotal = l.Amount,
                    AccountId = l.AccountId,
                    AccountName = l.Account.AccountName
                }).ToList(),
                SubTotal = b.Subtotal,
                TaxAmount = b.TaxAmount,
                TotalAmount = b.TotalAmount,
                AmountPaid = b.AmountPaid,
                AmountDue = b.TotalAmount - b.AmountPaid,
                IsPaid = b.IsPaid,
                PaymentDate = b.PaymentDate,
                JournalEntryId = b.JournalEntryId,
                Notes = b.Notes,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<VendorBillDto>> GetByVendorAsync(Guid vendorId)
    {
        return await _context.VendorBills
            .Include(b => b.Vendor)
            .Where(b => b.VendorId == vendorId)
            .Select(b => new VendorBillDto
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                VendorId = b.VendorId,
                VendorName = b.Vendor.CompanyName,
                PurchaseOrderId = null,
                PONumber = null,
                VendorInvoiceNumber = b.VendorInvoiceNumber ?? string.Empty,
                InvoiceDate = b.BillDate,
                DueDate = b.DueDate,
                Status = b.Status.ToString(),
                LineItems = new List<VendorBillLineDto>(),
                SubTotal = b.Subtotal,
                TaxAmount = b.TaxAmount,
                TotalAmount = b.TotalAmount,
                AmountPaid = b.AmountPaid,
                AmountDue = b.TotalAmount - b.AmountPaid,
                IsPaid = b.IsPaid,
                PaymentDate = b.PaymentDate,
                JournalEntryId = b.JournalEntryId,
                Notes = b.Notes,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<VendorBillDto> CreateAsync(Guid companyId, CreateVendorBillRequest request)
    {
        var billNumber = await GenerateBillNumberAsync(companyId);

        // Calculate totals
        var subtotal = request.LineItems.Sum(l => l.Quantity * l.UnitPrice);
        var totalAmount = subtotal + request.TaxAmount;

        var bill = new VendorBill
        {
            CompanyId = companyId,
            VendorId = request.VendorId,
            BillNumber = billNumber,
            VendorInvoiceNumber = request.VendorInvoiceNumber,
            BillDate = request.InvoiceDate,
            DueDate = request.DueDate,
            Subtotal = subtotal,
            TaxAmount = request.TaxAmount,
            TotalAmount = totalAmount,
            AmountPaid = 0,
            Status = BillStatus.Draft,
            IsPaid = false,
            Notes = request.Notes
        };

        _context.VendorBills.Add(bill);
        await _context.SaveChangesAsync();

        // Add line items
        foreach (var lineRequest in request.LineItems)
        {
            // Get default expense account if not provided
            var accountId = lineRequest.AccountId;
            if (accountId == null)
            {
                var vendor = await _context.Vendors.FindAsync(request.VendorId);
                accountId = vendor?.AccountsPayableAccountId;
                
                if (accountId == null)
                {
                    // Find a default expense account
                    var defaultAccount = await _context.Accounts
                        .Where(a => a.CompanyId == companyId && a.Type == AccountType.Expense)
                        .FirstOrDefaultAsync();
                    
                    if (defaultAccount == null)
                    {
                        throw new InvalidOperationException("No expense account found. Please specify an account.");
                    }
                    
                    accountId = defaultAccount.Id;
                }
            }

            var lineItem = new BillLineItem
            {
                BillId = bill.Id,
                AccountId = accountId.Value,
                Description = lineRequest.Description,
                Quantity = lineRequest.Quantity,
                UnitPrice = lineRequest.UnitPrice,
                Amount = lineRequest.Quantity * lineRequest.UnitPrice,
                IsCOGS = false
            };

            _context.BillLineItems.Add(lineItem);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Created vendor bill {BillNumber} for company {CompanyId}", billNumber, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "vendor_bill_created",
                $"VendorBill:{bill.Id}",
                $"Created vendor bill {billNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for bill creation");
        }

        return (await GetByIdAsync(bill.Id))!;
    }

    public async Task<string> GenerateBillNumberAsync(Guid companyId)
    {
        var lastBill = await _context.VendorBills
            .Where(b => b.CompanyId == companyId)
            .OrderByDescending(b => b.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastBill == null)
        {
            return "BILL-0001";
        }

        // Parse with validation
        var parts = lastBill.BillNumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            // If format is corrupted, find the maximum number
            var maxNumber = await _context.VendorBills
                .Where(b => b.CompanyId == companyId && b.BillNumber.StartsWith("BILL-"))
                .Select(b => b.BillNumber)
                .ToListAsync()
                .ContinueWith(task => task.Result
                    .Select(n => n.Split('-'))
                    .Where(p => p.Length == 2 && int.TryParse(p[1], out _))
                    .Select(p => int.Parse(p[1]))
                    .DefaultIfEmpty(0)
                    .Max());
            
            lastNumber = maxNumber;
        }

        var nextNumber = lastNumber + 1;
        return $"BILL-{nextNumber:D4}";
    }
}

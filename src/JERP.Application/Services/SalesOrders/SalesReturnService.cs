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

using JERP.Application.DTOs.SalesOrders;
using JERP.Application.Services.Security;
using JERP.Core.Entities.SalesOrders;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.SalesOrders;

/// <summary>
/// Service for sales return (RMA) management
/// </summary>
public class SalesReturnService : ISalesReturnService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<SalesReturnService> _logger;

    public SalesReturnService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<SalesReturnService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<SalesReturnDto?> GetByIdAsync(Guid id)
    {
        return await _context.SalesReturns
            .Where(sr => sr.Id == id)
            .Select(sr => new SalesReturnDto
            {
                Id = sr.Id,
                RMANumber = sr.RMANumber,
                CustomerId = sr.CustomerId,
                CustomerName = sr.Customer.CompanyName,
                SalesOrderId = sr.SalesOrderId,
                SONumber = sr.SalesOrder != null ? sr.SalesOrder.SONumber : null,
                InvoiceId = sr.InvoiceId,
                InvoiceNumber = sr.Invoice != null ? sr.Invoice.InvoiceNumber : null,
                ReturnDate = sr.ReturnDate,
                Status = sr.Status.ToString(),
                Reason = sr.Reason,
                ReturnType = sr.ReturnType,
                LineItems = sr.LineItems.Select(line => new SalesReturnLineDto
                {
                    Id = line.Id,
                    SalesReturnId = line.SalesReturnId,
                    SalesOrderLineId = line.SalesOrderLineId,
                    InventoryItemId = line.InventoryItemId,
                    ItemCode = line.InventoryItem.SKU,
                    ItemName = line.InventoryItem.Name,
                    Description = line.Description,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice,
                    TaxAmount = line.TaxAmount,
                    LineTotal = line.LineTotal,
                    RestockingFee = line.RestockingFee,
                    Notes = line.Notes
                }).ToList(),
                SubTotal = sr.SubTotal,
                TaxAmount = sr.TaxAmount,
                TotalAmount = sr.TotalAmount,
                ApprovedBy = sr.ApprovedBy,
                ApprovedAt = sr.ApprovedAt,
                ReceivedBy = sr.ReceivedBy,
                ReceivedAt = sr.ReceivedAt,
                Notes = sr.Notes,
                CreatedAt = sr.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<SalesReturnDto>> GetAllAsync(Guid companyId)
    {
        return await _context.SalesReturns
            .Where(sr => sr.CompanyId == companyId)
            .OrderByDescending(sr => sr.CreatedAt)
            .Select(sr => new SalesReturnDto
            {
                Id = sr.Id,
                RMANumber = sr.RMANumber,
                CustomerId = sr.CustomerId,
                CustomerName = sr.Customer.CompanyName,
                ReturnDate = sr.ReturnDate,
                Status = sr.Status.ToString(),
                Reason = sr.Reason,
                TotalAmount = sr.TotalAmount,
                CreatedAt = sr.CreatedAt,
                LineItems = new List<SalesReturnLineDto>()
            })
            .ToListAsync();
    }

    public async Task<List<SalesReturnDto>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.SalesReturns
            .Where(sr => sr.CustomerId == customerId)
            .OrderByDescending(sr => sr.CreatedAt)
            .Select(sr => new SalesReturnDto
            {
                Id = sr.Id,
                RMANumber = sr.RMANumber,
                CustomerId = sr.CustomerId,
                CustomerName = sr.Customer.CompanyName,
                ReturnDate = sr.ReturnDate,
                Status = sr.Status.ToString(),
                TotalAmount = sr.TotalAmount,
                CreatedAt = sr.CreatedAt,
                LineItems = new List<SalesReturnLineDto>()
            })
            .ToListAsync();
    }

    public async Task<SalesReturnDto> RequestAsync(Guid companyId, Guid customerId, Guid? salesOrderId, string reason)
    {
        var rmaNumber = await GenerateRMANumberAsync(companyId);

        var salesReturn = new SalesReturn
        {
            CompanyId = companyId,
            RMANumber = rmaNumber,
            CustomerId = customerId,
            SalesOrderId = salesOrderId,
            ReturnDate = DateTime.UtcNow,
            Status = SalesReturnStatus.Requested,
            Reason = reason,
            SubTotal = 0,
            TaxAmount = 0,
            TotalAmount = 0
        };

        _context.SalesReturns.Add(salesReturn);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created sales return {RMANumber} for customer {CustomerId}", rmaNumber, customerId);

        return (await GetByIdAsync(salesReturn.Id))!;
    }

    public async Task<SalesReturnDto> ApproveAsync(Guid id, string approvedBy)
    {
        var salesReturn = await _context.SalesReturns.FindAsync(id);
        if (salesReturn == null)
        {
            throw new InvalidOperationException($"Sales return {id} not found");
        }

        if (salesReturn.Status != SalesReturnStatus.Requested)
        {
            throw new InvalidOperationException("Can only approve requested returns");
        }

        salesReturn.Status = SalesReturnStatus.Approved;
        salesReturn.ApprovedBy = approvedBy;
        salesReturn.ApprovedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Approved sales return {RMANumber} by {ApprovedBy}", salesReturn.RMANumber, approvedBy);

        return (await GetByIdAsync(salesReturn.Id))!;
    }

    public async Task<SalesReturnDto> ReceiveAsync(Guid id, string receivedBy)
    {
        var salesReturn = await _context.SalesReturns
            .Include(sr => sr.LineItems)
            .FirstOrDefaultAsync(sr => sr.Id == id);

        if (salesReturn == null)
        {
            throw new InvalidOperationException($"Sales return {id} not found");
        }

        if (salesReturn.Status != SalesReturnStatus.Approved)
        {
            throw new InvalidOperationException("Can only receive approved returns");
        }

        // TODO: Add inventory back to stock

        salesReturn.Status = SalesReturnStatus.Received;
        salesReturn.ReceivedBy = receivedBy;
        salesReturn.ReceivedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Received sales return {RMANumber} by {ReceivedBy}", salesReturn.RMANumber, receivedBy);

        return (await GetByIdAsync(salesReturn.Id))!;
    }

    public async Task<SalesReturnDto> RefundAsync(Guid id)
    {
        var salesReturn = await _context.SalesReturns.FindAsync(id);
        if (salesReturn == null)
        {
            throw new InvalidOperationException($"Sales return {id} not found");
        }

        if (salesReturn.Status != SalesReturnStatus.Received)
        {
            throw new InvalidOperationException("Can only refund received returns");
        }

        // TODO: Process refund to customer

        salesReturn.Status = SalesReturnStatus.Refunded;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Refunded sales return {RMANumber}", salesReturn.RMANumber);

        return (await GetByIdAsync(salesReturn.Id))!;
    }

    public async Task<SalesReturnDto> CloseAsync(Guid id)
    {
        var salesReturn = await _context.SalesReturns.FindAsync(id);
        if (salesReturn == null)
        {
            throw new InvalidOperationException($"Sales return {id} not found");
        }

        salesReturn.Status = SalesReturnStatus.Closed;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Closed sales return {RMANumber}", salesReturn.RMANumber);

        return (await GetByIdAsync(salesReturn.Id))!;
    }

    private async Task<string> GenerateRMANumberAsync(Guid companyId)
    {
        var lastReturn = await _context.SalesReturns
            .Where(sr => sr.CompanyId == companyId)
            .OrderByDescending(sr => sr.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastReturn == null)
        {
            return "RMA-0001";
        }

        var parts = lastReturn.RMANumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            _logger.LogWarning("Invalid RMA number format: {RMANumber}, generating from scratch", lastReturn.RMANumber);
            return "RMA-0001";
        }

        var nextNumber = lastNumber + 1;
        return $"RMA-{nextNumber:D4}";
    }
}

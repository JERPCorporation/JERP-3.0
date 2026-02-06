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
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service for purchase order receipt management
/// </summary>
public class POReceiptService : IPOReceiptService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<POReceiptService> _logger;

    public POReceiptService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<POReceiptService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<POReceiptDto?> GetByIdAsync(Guid id)
    {
        return await _context.StockReceipts
            .Include(r => r.PurchaseOrder)
                .ThenInclude(po => po.Vendor)
            .Include(r => r.Warehouse)
            .Include(r => r.Lines)
                .ThenInclude(l => l.Product)
            .Include(r => r.Lines)
                .ThenInclude(l => l.POLine)
            .Where(r => r.Id == id)
            .Select(r => new POReceiptDto
            {
                Id = r.Id,
                ReceiptNumber = r.ReceiptNumber,
                PurchaseOrderId = r.PurchaseOrderId,
                PONumber = r.PurchaseOrder.PONumber,
                VendorId = r.PurchaseOrder.VendorId,
                VendorName = r.PurchaseOrder.Vendor.CompanyName,
                ReceiptDate = r.ReceiptDate,
                WarehouseId = r.WarehouseId,
                WarehouseName = r.Warehouse.Name,
                IsComplete = r.IsComplete,
                QCPassed = r.QCPassed,
                QCNotes = r.QCNotes,
                LineItems = r.Lines.Select(l => new POReceiptLineDto
                {
                    Id = l.Id,
                    PurchaseOrderLineId = l.POLineId,
                    ProductId = l.ProductId,
                    ItemCode = l.Product.SKU,
                    ItemName = l.Product.Name,
                    QuantityOrdered = l.POLine.QuantityOrdered,
                    QuantityReceived = l.QuantityReceived,
                    QuantityPreviouslyReceived = l.POLine.QuantityReceived - l.QuantityReceived,
                    UnitOfMeasure = l.Product.UnitOfMeasure,
                    UnitCost = l.UnitCost,
                    BatchId = null,
                    BatchNumber = l.BatchNumber,
                    ExpirationDate = l.ExpirationDate,
                    Notes = l.Notes
                }).ToList(),
                ReceivedBy = null,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<POReceiptDto>> GetByPurchaseOrderAsync(Guid purchaseOrderId)
    {
        return await _context.StockReceipts
            .Include(r => r.PurchaseOrder)
                .ThenInclude(po => po.Vendor)
            .Include(r => r.Warehouse)
            .Include(r => r.Lines)
            .Where(r => r.PurchaseOrderId == purchaseOrderId)
            .Select(r => new POReceiptDto
            {
                Id = r.Id,
                ReceiptNumber = r.ReceiptNumber,
                PurchaseOrderId = r.PurchaseOrderId,
                PONumber = r.PurchaseOrder.PONumber,
                VendorId = r.PurchaseOrder.VendorId,
                VendorName = r.PurchaseOrder.Vendor.CompanyName,
                ReceiptDate = r.ReceiptDate,
                WarehouseId = r.WarehouseId,
                WarehouseName = r.Warehouse.Name,
                IsComplete = r.IsComplete,
                QCPassed = r.QCPassed,
                QCNotes = r.QCNotes,
                LineItems = new List<POReceiptLineDto>(),
                ReceivedBy = null,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<POReceiptDto> CreateAsync(Guid companyId, CreatePOReceiptRequest request, string? userId = null)
    {
        var receiptNumber = await GenerateReceiptNumberAsync(companyId);

        var po = await _context.PurchaseOrders
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == request.PurchaseOrderId);

        if (po == null)
        {
            throw new InvalidOperationException($"Purchase order {request.PurchaseOrderId} not found");
        }

        if (po.Status != PurchaseOrderStatus.Approved && po.Status != PurchaseOrderStatus.Ordered && po.Status != PurchaseOrderStatus.PartiallyReceived)
        {
            throw new InvalidOperationException("Purchase order must be approved or ordered to receive items");
        }

        var receipt = new StockReceipt
        {
            CompanyId = companyId,
            PurchaseOrderId = request.PurchaseOrderId,
            WarehouseId = po.WarehouseId,
            ReceiptNumber = receiptNumber,
            ReceiptDate = request.ReceiptDate,
            ReceivedByUserId = userId != null && Guid.TryParse(userId, out var userGuid) ? userGuid : Guid.Empty,
            IsComplete = false,
            QCPassed = request.QCPassed,
            QCNotes = request.QCNotes,
            Notes = request.Notes
        };

        _context.StockReceipts.Add(receipt);
        await _context.SaveChangesAsync();

        // Add line items and update PO line quantities
        foreach (var lineRequest in request.LineItems)
        {
            var poLine = await _context.PurchaseOrderLines.FindAsync(lineRequest.PurchaseOrderLineId);
            if (poLine == null)
            {
                throw new InvalidOperationException($"Purchase order line {lineRequest.PurchaseOrderLineId} not found");
            }

            var receiptLine = new StockReceiptLine
            {
                ReceiptId = receipt.Id,
                POLineId = lineRequest.PurchaseOrderLineId,
                ProductId = poLine.ProductId,
                QuantityReceived = (int)lineRequest.QuantityReceived,
                UnitCost = poLine.UnitCost,
                BatchNumber = lineRequest.BatchNumber ?? $"BATCH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8]}",
                ExpirationDate = lineRequest.ExpirationDate,
                Notes = lineRequest.Notes
            };

            _context.StockReceiptLines.Add(receiptLine);

            // Update PO line quantity received
            poLine.QuantityReceived += (int)lineRequest.QuantityReceived;
        }

        // Update PO status
        var allLinesFullyReceived = po.Lines.All(l => l.QuantityReceived >= l.QuantityOrdered);
        var anyLinesPartiallyReceived = po.Lines.Any(l => l.QuantityReceived > 0 && l.QuantityReceived < l.QuantityOrdered);

        if (allLinesFullyReceived)
        {
            po.Status = PurchaseOrderStatus.Received;
            po.ReceivedDate = DateTime.UtcNow;
            receipt.IsComplete = true;
        }
        else if (anyLinesPartiallyReceived || po.Lines.Any(l => l.QuantityReceived > 0))
        {
            po.Status = PurchaseOrderStatus.PartiallyReceived;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Created receipt {ReceiptNumber} for PO {PONumber}", receiptNumber, po.PONumber);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                userId != null ? Guid.Parse(userId) : Guid.Empty,
                "system@jerp.io",
                "System",
                "receipt_created",
                $"StockReceipt:{receipt.Id}",
                $"Created receipt {receiptNumber} for PO {po.PONumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for receipt creation");
        }

        return (await GetByIdAsync(receipt.Id))!;
    }

    public async Task<string> GenerateReceiptNumberAsync(Guid companyId)
    {
        var lastReceipt = await _context.StockReceipts
            .Where(r => r.CompanyId == companyId)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastReceipt == null)
        {
            return "RCV-0001";
        }

        // Parse with validation
        var parts = lastReceipt.ReceiptNumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            // If format is corrupted, find the maximum number
            var maxNumber = await _context.StockReceipts
                .Where(r => r.CompanyId == companyId && r.ReceiptNumber.StartsWith("RCV-"))
                .Select(r => r.ReceiptNumber)
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
        return $"RCV-{nextNumber:D4}";
    }
}

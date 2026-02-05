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

using JERP.Application.DTOs.PurchaseOrders;
using JERP.Application.Services.Security;
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.PurchaseOrders;

/// <summary>
/// Service for purchase order management
/// </summary>
public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<PurchaseOrderService> _logger;

    public PurchaseOrderService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<PurchaseOrderService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.Warehouse)
            .Include(po => po.Lines)
                .ThenInclude(l => l.Product)
            .Where(po => po.Id == id)
            .Select(po => new PurchaseOrderDto
            {
                Id = po.Id,
                PONumber = po.PONumber,
                VendorId = po.VendorId,
                VendorName = po.Vendor.CompanyName,
                Status = po.Status.ToString(),
                OrderDate = po.OrderDate,
                ExpectedDeliveryDate = po.ExpectedDate,
                WarehouseId = po.WarehouseId,
                WarehouseName = po.Warehouse.Name,
                ShippingMethod = null,
                ShippingTerms = null,
                PaymentTerms = po.Vendor.PaymentTerms.ToString(),
                LineItems = po.Lines.Select(l => new PurchaseOrderLineDto
                {
                    Id = l.Id,
                    LineNumber = l.LineNumber,
                    ProductId = l.ProductId,
                    ItemCode = l.Product.SKU,
                    ItemName = l.Product.Name,
                    Description = l.Notes,
                    Quantity = l.QuantityOrdered,
                    UnitOfMeasure = l.Product.UnitOfMeasure,
                    UnitPrice = l.UnitCost,
                    LineTotal = l.QuantityOrdered * l.UnitCost,
                    QuantityReceived = l.QuantityReceived,
                    QuantityRemaining = l.QuantityOrdered - l.QuantityReceived,
                    IsFullyReceived = l.QuantityReceived >= l.QuantityOrdered,
                    Notes = l.Notes
                }).ToList(),
                SubTotal = po.Subtotal,
                TaxAmount = po.TaxAmount,
                ShippingAmount = po.ShippingAmount,
                TotalAmount = po.TotalAmount,
                TotalReceived = po.Lines.Sum(l => l.QuantityReceived * l.UnitCost),
                IsFullyReceived = po.Status == PurchaseOrderStatus.Received,
                ApprovedBy = null,
                ApprovedAt = po.ApprovedDate,
                VendorPONumber = po.VendorPONumber,
                Notes = po.Notes,
                CreatedAt = po.CreatedAt,
                UpdatedAt = po.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<PurchaseOrderListDto>> GetAllAsync(Guid companyId, string? status = null)
    {
        var query = _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Where(po => po.CompanyId == companyId);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<PurchaseOrderStatus>(status, true, out var statusEnum))
        {
            query = query.Where(po => po.Status == statusEnum);
        }

        return await query
            .OrderByDescending(po => po.CreatedAt)
            .Select(po => new PurchaseOrderListDto
            {
                Id = po.Id,
                PONumber = po.PONumber,
                VendorId = po.VendorId,
                VendorName = po.Vendor.CompanyName,
                Status = po.Status.ToString(),
                OrderDate = po.OrderDate,
                ExpectedDeliveryDate = po.ExpectedDate,
                TotalAmount = po.TotalAmount,
                IsFullyReceived = po.Status == PurchaseOrderStatus.Received
            })
            .ToListAsync();
    }

    public async Task<PurchaseOrderDto> CreateAsync(Guid companyId, CreatePurchaseOrderRequest request, string? userId = null)
    {
        var poNumber = await GeneratePONumberAsync(companyId);

        // Calculate totals
        var subtotal = request.LineItems.Sum(l => l.Quantity * l.UnitPrice);
        // TODO: Implement proper tax calculation based on vendor location and tax rules
        var taxAmount = subtotal * 0; 
        var totalAmount = subtotal + taxAmount + request.ShippingAmount;

        var po = new PurchaseOrder
        {
            CompanyId = companyId,
            VendorId = request.VendorId,
            WarehouseId = request.WarehouseId,
            PONumber = poNumber,
            OrderDate = request.OrderDate,
            ExpectedDate = request.ExpectedDeliveryDate,
            Subtotal = subtotal,
            TaxAmount = taxAmount,
            ShippingAmount = request.ShippingAmount,
            TotalAmount = totalAmount,
            Status = PurchaseOrderStatus.Draft,
            VendorPONumber = request.VendorPONumber,
            Notes = request.Notes
        };

        _context.PurchaseOrders.Add(po);
        await _context.SaveChangesAsync();

        // Add line items
        int lineNumber = 1;
        foreach (var lineRequest in request.LineItems)
        {
            var line = new PurchaseOrderLine
            {
                PurchaseOrderId = po.Id,
                ProductId = lineRequest.ProductId,
                LineNumber = lineNumber++,
                QuantityOrdered = (int)lineRequest.Quantity,
                QuantityReceived = 0,
                UnitCost = lineRequest.UnitPrice,
                Notes = lineRequest.Notes
            };
            _context.PurchaseOrderLines.Add(line);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Created purchase order {PONumber} for company {CompanyId}", poNumber, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                userId != null ? Guid.Parse(userId) : Guid.Empty,
                "system@jerp.io",
                "System",
                "purchase_order_created",
                $"PurchaseOrder:{po.Id}",
                $"Created purchase order {poNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for PO creation");
        }

        return (await GetByIdAsync(po.Id))!;
    }

    public async Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderRequest request)
    {
        var po = await _context.PurchaseOrders.FindAsync(id);
        if (po == null)
        {
            throw new InvalidOperationException($"Purchase order {id} not found");
        }

        if (po.Status != PurchaseOrderStatus.Draft)
        {
            throw new InvalidOperationException("Only draft purchase orders can be updated");
        }

        po.ExpectedDate = request.ExpectedDeliveryDate;
        po.ShippingAmount = request.ShippingAmount;
        po.VendorPONumber = request.VendorPONumber;
        po.Notes = request.Notes;
        
        // Recalculate total
        po.TotalAmount = po.Subtotal + po.TaxAmount + po.ShippingAmount;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated purchase order {PONumber}", po.PONumber);

        return (await GetByIdAsync(po.Id))!;
    }

    public async Task<PurchaseOrderDto> ApproveAsync(Guid id, string? userId = null)
    {
        var po = await _context.PurchaseOrders.FindAsync(id);
        if (po == null)
        {
            throw new InvalidOperationException($"Purchase order {id} not found");
        }

        po.Status = PurchaseOrderStatus.Approved;
        po.ApprovedDate = DateTime.UtcNow;
        if (userId != null && Guid.TryParse(userId, out var userGuid))
        {
            po.ApprovedByUserId = userGuid;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Approved purchase order {PONumber}", po.PONumber);

        try
        {
            await _auditLogService.LogAction(
                po.CompanyId,
                userId != null ? Guid.Parse(userId) : Guid.Empty,
                "system@jerp.io",
                "System",
                "purchase_order_approved",
                $"PurchaseOrder:{po.Id}",
                $"Approved purchase order {po.PONumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for PO approval");
        }

        return (await GetByIdAsync(po.Id))!;
    }

    public async Task<PurchaseOrderDto> CancelAsync(Guid id)
    {
        var po = await _context.PurchaseOrders.FindAsync(id);
        if (po == null)
        {
            throw new InvalidOperationException($"Purchase order {id} not found");
        }

        if (po.Status == PurchaseOrderStatus.Received || po.Status == PurchaseOrderStatus.Closed)
        {
            throw new InvalidOperationException("Cannot cancel a received or closed purchase order");
        }

        po.Status = PurchaseOrderStatus.Cancelled;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Cancelled purchase order {PONumber}", po.PONumber);

        return (await GetByIdAsync(po.Id))!;
    }

    public async Task<PurchaseOrderDto> CloseAsync(Guid id)
    {
        var po = await _context.PurchaseOrders.FindAsync(id);
        if (po == null)
        {
            throw new InvalidOperationException($"Purchase order {id} not found");
        }

        po.Status = PurchaseOrderStatus.Closed;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Closed purchase order {PONumber}", po.PONumber);

        return (await GetByIdAsync(po.Id))!;
    }

    public async Task<string> GeneratePONumberAsync(Guid companyId)
    {
        var lastPO = await _context.PurchaseOrders
            .Where(po => po.CompanyId == companyId)
            .OrderByDescending(po => po.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastPO == null)
        {
            return "PO-0001";
        }

        // Parse with validation
        var parts = lastPO.PONumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            // If format is corrupted, find the maximum number from all POs
            var maxNumber = await _context.PurchaseOrders
                .Where(po => po.CompanyId == companyId && po.PONumber.StartsWith("PO-"))
                .Select(po => po.PONumber)
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
        return $"PO-{nextNumber:D4}";
    }
}

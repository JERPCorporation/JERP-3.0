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
/// Service for sales order management
/// </summary>
public class SalesOrderService : ISalesOrderService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<SalesOrderService> _logger;

    public SalesOrderService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<SalesOrderService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<SalesOrderDto?> GetByIdAsync(Guid id)
    {
        return await _context.SalesOrders
            .Where(so => so.Id == id)
            .Select(so => new SalesOrderDto
            {
                Id = so.Id,
                SONumber = so.SONumber,
                CustomerId = so.CustomerId,
                CustomerName = so.Customer.CompanyName,
                Status = so.Status.ToString(),
                OrderDate = so.OrderDate,
                RequestedShipDate = so.RequestedShipDate,
                PromisedShipDate = so.PromisedShipDate,
                WarehouseId = so.WarehouseId,
                WarehouseName = so.Warehouse != null ? so.Warehouse.Name : null,
                ShippingMethod = so.ShippingMethod,
                ShippingTerms = so.ShippingTerms,
                PaymentTerms = so.PaymentTerms,
                ShipToAddressLine1 = so.ShipToAddressLine1,
                ShipToAddressLine2 = so.ShipToAddressLine2,
                ShipToCity = so.ShipToCity,
                ShipToState = so.ShipToState,
                ShipToPostalCode = so.ShipToPostalCode,
                ShipToCountry = so.ShipToCountry,
                LineItems = so.LineItems.Select(line => new SalesOrderLineDto
                {
                    Id = line.Id,
                    LineNumber = line.LineNumber,
                    InventoryItemId = line.InventoryItemId,
                    ItemCode = line.InventoryItem.SKU,
                    ItemName = line.InventoryItem.Name,
                    Description = line.Description,
                    Quantity = line.Quantity,
                    UnitOfMeasure = line.InventoryItem.UnitOfMeasure,
                    UnitPrice = line.UnitPrice,
                    DiscountPercent = line.DiscountPercent,
                    DiscountAmount = line.DiscountAmount,
                    TaxPercent = line.TaxPercent,
                    TaxAmount = line.TaxAmount,
                    LineTotal = line.LineTotal,
                    QuantityShipped = line.QuantityShipped,
                    QuantityInvoiced = line.QuantityInvoiced,
                    QuantityRemaining = line.Quantity - line.QuantityShipped,
                    IsFullyShipped = line.QuantityShipped >= line.Quantity,
                    IsFullyInvoiced = line.QuantityInvoiced >= line.Quantity,
                    RevenueAccountId = line.RevenueAccountId,
                    Notes = line.Notes
                }).ToList(),
                SubTotal = so.SubTotal,
                TaxAmount = so.TaxAmount,
                ShippingAmount = so.ShippingAmount,
                DiscountAmount = so.DiscountAmount,
                TotalAmount = so.TotalAmount,
                TotalShipped = so.TotalShipped,
                TotalInvoiced = so.TotalInvoiced,
                IsFullyShipped = so.IsFullyShipped,
                IsFullyInvoiced = so.IsFullyInvoiced,
                ApprovedBy = so.ApprovedBy,
                ApprovedAt = so.ApprovedAt,
                SalesRepId = so.SalesRepId,
                SalesRepName = null, // TODO: Add Employee navigation
                SalesQuoteId = so.SalesQuoteId,
                SalesQuoteNumber = so.SalesQuoteNumber,
                CustomerPONumber = so.CustomerPONumber,
                Notes = so.Notes,
                InternalNotes = so.InternalNotes,
                RequiresMetrcTracking = so.RequiresMetrcTracking,
                MetrcManifestNumber = so.MetrcManifestNumber,
                CreatedAt = so.CreatedAt,
                UpdatedAt = so.UpdatedAt,
                CreatedBy = so.CreatedBy
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<SalesOrderDto>> GetAllAsync(Guid companyId, SalesOrderStatus? status = null)
    {
        var query = _context.SalesOrders
            .Where(so => so.CompanyId == companyId);

        if (status.HasValue)
        {
            query = query.Where(so => so.Status == status.Value);
        }

        return await query
            .OrderByDescending(so => so.CreatedAt)
            .Select(so => new SalesOrderDto
            {
                Id = so.Id,
                SONumber = so.SONumber,
                CustomerId = so.CustomerId,
                CustomerName = so.Customer.CompanyName,
                Status = so.Status.ToString(),
                OrderDate = so.OrderDate,
                RequestedShipDate = so.RequestedShipDate,
                PromisedShipDate = so.PromisedShipDate,
                SubTotal = so.SubTotal,
                TaxAmount = so.TaxAmount,
                ShippingAmount = so.ShippingAmount,
                DiscountAmount = so.DiscountAmount,
                TotalAmount = so.TotalAmount,
                TotalShipped = so.TotalShipped,
                TotalInvoiced = so.TotalInvoiced,
                IsFullyShipped = so.IsFullyShipped,
                IsFullyInvoiced = so.IsFullyInvoiced,
                CreatedAt = so.CreatedAt,
                UpdatedAt = so.UpdatedAt,
                LineItems = new List<SalesOrderLineDto>()
            })
            .ToListAsync();
    }

    public async Task<List<SalesOrderDto>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.SalesOrders
            .Where(so => so.CustomerId == customerId)
            .OrderByDescending(so => so.CreatedAt)
            .Select(so => new SalesOrderDto
            {
                Id = so.Id,
                SONumber = so.SONumber,
                CustomerId = so.CustomerId,
                CustomerName = so.Customer.CompanyName,
                Status = so.Status.ToString(),
                OrderDate = so.OrderDate,
                TotalAmount = so.TotalAmount,
                IsFullyShipped = so.IsFullyShipped,
                IsFullyInvoiced = so.IsFullyInvoiced,
                CreatedAt = so.CreatedAt,
                LineItems = new List<SalesOrderLineDto>()
            })
            .ToListAsync();
    }

    public async Task<SalesOrderDto> CreateAsync(Guid companyId, CreateSalesOrderRequest request)
    {
        // Validate customer
        var customer = await _context.Customers.FindAsync(request.CustomerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer {request.CustomerId} not found");
        }

        // Check credit limit
        var orderTotal = CalculateOrderTotal(request);
        var hasCredit = await CheckCreditLimitAsync(request.CustomerId, orderTotal);
        if (!hasCredit)
        {
            throw new InvalidOperationException("Customer has insufficient credit limit for this order");
        }

        // Generate SO number
        var soNumber = await GenerateSONumberAsync(companyId);

        // Calculate line totals
        var lineNumber = 1;
        var salesOrder = new SalesOrder
        {
            CompanyId = companyId,
            CustomerId = request.CustomerId,
            SONumber = soNumber,
            Status = SalesOrderStatus.Draft,
            OrderDate = request.OrderDate,
            RequestedShipDate = request.RequestedShipDate,
            WarehouseId = request.WarehouseId,
            ShippingMethod = request.ShippingMethod,
            ShippingTerms = request.ShippingTerms,
            PaymentTerms = request.PaymentTerms,
            ShipToAddressLine1 = request.ShipToAddressLine1,
            ShipToAddressLine2 = request.ShipToAddressLine2,
            ShipToCity = request.ShipToCity,
            ShipToState = request.ShipToState,
            ShipToPostalCode = request.ShipToPostalCode,
            ShipToCountry = request.ShipToCountry,
            ShippingAmount = request.ShippingAmount,
            DiscountAmount = request.DiscountAmount,
            SalesRepId = request.SalesRepId,
            SalesQuoteId = request.SalesQuoteId,
            CustomerPONumber = request.CustomerPONumber,
            Notes = request.Notes,
            InternalNotes = request.InternalNotes,
            RequiresMetrcTracking = customer.IsCannabisCustomer,
            CreatedBy = "system" // TODO: Get from current user
        };

        // Add line items
        foreach (var lineRequest in request.LineItems)
        {
            var product = await _context.Products.FindAsync(lineRequest.InventoryItemId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product {lineRequest.InventoryItemId} not found");
            }

            var discountAmount = lineRequest.Quantity * lineRequest.UnitPrice * (lineRequest.DiscountPercent / 100);
            var lineSubtotal = lineRequest.Quantity * lineRequest.UnitPrice - discountAmount;
            var taxAmount = lineSubtotal * (lineRequest.TaxPercent / 100);
            var lineTotal = lineSubtotal + taxAmount;

            var line = new SalesOrderLine
            {
                LineNumber = lineNumber++,
                InventoryItemId = lineRequest.InventoryItemId,
                Description = lineRequest.Description,
                Quantity = lineRequest.Quantity,
                UnitPrice = lineRequest.UnitPrice,
                DiscountPercent = lineRequest.DiscountPercent,
                DiscountAmount = discountAmount,
                TaxPercent = lineRequest.TaxPercent,
                TaxAmount = taxAmount,
                LineTotal = lineTotal,
                QuantityShipped = 0,
                QuantityInvoiced = 0,
                RevenueAccountId = lineRequest.RevenueAccountId,
                Notes = lineRequest.Notes
            };

            salesOrder.LineItems.Add(line);
        }

        // Calculate totals
        salesOrder.SubTotal = salesOrder.LineItems.Sum(l => l.Quantity * l.UnitPrice - l.DiscountAmount);
        salesOrder.TaxAmount = salesOrder.LineItems.Sum(l => l.TaxAmount);
        salesOrder.TotalAmount = salesOrder.SubTotal + salesOrder.TaxAmount + salesOrder.ShippingAmount - salesOrder.DiscountAmount;

        _context.SalesOrders.Add(salesOrder);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created sales order {SONumber} for customer {CustomerId}", soNumber, request.CustomerId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "sales_order_created",
                $"SalesOrder:{salesOrder.Id}",
                $"Created sales order {soNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for sales order creation");
        }

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<SalesOrderDto> UpdateAsync(Guid id, CreateSalesOrderRequest request)
    {
        var salesOrder = await _context.SalesOrders
            .Include(so => so.LineItems)
            .FirstOrDefaultAsync(so => so.Id == id);

        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {id} not found");
        }

        if (salesOrder.Status != SalesOrderStatus.Draft)
        {
            throw new InvalidOperationException("Can only update draft sales orders");
        }

        // Update header
        salesOrder.CustomerId = request.CustomerId;
        salesOrder.OrderDate = request.OrderDate;
        salesOrder.RequestedShipDate = request.RequestedShipDate;
        salesOrder.WarehouseId = request.WarehouseId;
        salesOrder.ShippingMethod = request.ShippingMethod;
        salesOrder.ShippingTerms = request.ShippingTerms;
        salesOrder.PaymentTerms = request.PaymentTerms;
        salesOrder.ShipToAddressLine1 = request.ShipToAddressLine1;
        salesOrder.ShipToCity = request.ShipToCity;
        salesOrder.ShipToState = request.ShipToState;
        salesOrder.ShipToPostalCode = request.ShipToPostalCode;
        salesOrder.ShippingAmount = request.ShippingAmount;
        salesOrder.DiscountAmount = request.DiscountAmount;
        salesOrder.Notes = request.Notes;
        salesOrder.InternalNotes = request.InternalNotes;

        // Remove old lines and add new ones
        _context.SalesOrderLines.RemoveRange(salesOrder.LineItems);
        
        var lineNumber = 1;
        foreach (var lineRequest in request.LineItems)
        {
            var discountAmount = lineRequest.Quantity * lineRequest.UnitPrice * (lineRequest.DiscountPercent / 100);
            var lineSubtotal = lineRequest.Quantity * lineRequest.UnitPrice - discountAmount;
            var taxAmount = lineSubtotal * (lineRequest.TaxPercent / 100);
            var lineTotal = lineSubtotal + taxAmount;

            var line = new SalesOrderLine
            {
                SalesOrderId = salesOrder.Id,
                LineNumber = lineNumber++,
                InventoryItemId = lineRequest.InventoryItemId,
                Description = lineRequest.Description,
                Quantity = lineRequest.Quantity,
                UnitPrice = lineRequest.UnitPrice,
                DiscountPercent = lineRequest.DiscountPercent,
                DiscountAmount = discountAmount,
                TaxPercent = lineRequest.TaxPercent,
                TaxAmount = taxAmount,
                LineTotal = lineTotal,
                QuantityShipped = 0,
                QuantityInvoiced = 0,
                RevenueAccountId = lineRequest.RevenueAccountId,
                Notes = lineRequest.Notes
            };

            salesOrder.LineItems.Add(line);
        }

        // Recalculate totals
        salesOrder.SubTotal = salesOrder.LineItems.Sum(l => l.Quantity * l.UnitPrice - l.DiscountAmount);
        salesOrder.TaxAmount = salesOrder.LineItems.Sum(l => l.TaxAmount);
        salesOrder.TotalAmount = salesOrder.SubTotal + salesOrder.TaxAmount + salesOrder.ShippingAmount - salesOrder.DiscountAmount;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated sales order {SONumber}", salesOrder.SONumber);

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<SalesOrderDto> SubmitAsync(Guid id)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {id} not found");
        }

        if (salesOrder.Status != SalesOrderStatus.Draft)
        {
            throw new InvalidOperationException("Can only submit draft sales orders");
        }

        salesOrder.Status = SalesOrderStatus.Submitted;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Submitted sales order {SONumber}", salesOrder.SONumber);

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<SalesOrderDto> ApproveAsync(Guid id, string approvedBy)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {id} not found");
        }

        if (salesOrder.Status != SalesOrderStatus.Submitted)
        {
            throw new InvalidOperationException("Can only approve submitted sales orders");
        }

        // Check credit limit
        var hasCredit = await CheckCreditLimitAsync(salesOrder.CustomerId, salesOrder.TotalAmount);
        if (!hasCredit)
        {
            throw new InvalidOperationException("Customer has insufficient credit limit");
        }

        salesOrder.Status = SalesOrderStatus.Approved;
        salesOrder.ApprovedBy = approvedBy;
        salesOrder.ApprovedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Approved sales order {SONumber} by {ApprovedBy}", salesOrder.SONumber, approvedBy);

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<SalesOrderDto> CancelAsync(Guid id, string reason)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {id} not found");
        }

        if (salesOrder.Status == SalesOrderStatus.Shipped || salesOrder.Status == SalesOrderStatus.Closed)
        {
            throw new InvalidOperationException("Cannot cancel shipped or closed orders");
        }

        salesOrder.Status = SalesOrderStatus.Cancelled;
        salesOrder.InternalNotes = $"{salesOrder.InternalNotes}\nCancelled: {reason}";
        await _context.SaveChangesAsync();

        _logger.LogInformation("Cancelled sales order {SONumber}: {Reason}", salesOrder.SONumber, reason);

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<SalesOrderDto> CloseAsync(Guid id)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {id} not found");
        }

        salesOrder.Status = SalesOrderStatus.Closed;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Closed sales order {SONumber}", salesOrder.SONumber);

        return (await GetByIdAsync(salesOrder.Id))!;
    }

    public async Task<bool> CheckCreditLimitAsync(Guid customerId, decimal orderAmount)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            return false;
        }

        var availableCredit = customer.CreditLimit - customer.Balance;
        return availableCredit >= orderAmount;
    }

    private async Task<string> GenerateSONumberAsync(Guid companyId)
    {
        var lastOrder = await _context.SalesOrders
            .Where(so => so.CompanyId == companyId)
            .OrderByDescending(so => so.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastOrder == null)
        {
            return "SO-0001";
        }

        var parts = lastOrder.SONumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            _logger.LogWarning("Invalid SO number format: {SONumber}, generating from scratch", lastOrder.SONumber);
            return "SO-0001";
        }

        var nextNumber = lastNumber + 1;
        return $"SO-{nextNumber:D4}";
    }

    private decimal CalculateOrderTotal(CreateSalesOrderRequest request)
    {
        decimal subtotal = 0;
        decimal totalTax = 0;

        foreach (var line in request.LineItems)
        {
            var discountAmount = line.Quantity * line.UnitPrice * (line.DiscountPercent / 100);
            var lineSubtotal = line.Quantity * line.UnitPrice - discountAmount;
            var taxAmount = lineSubtotal * (line.TaxPercent / 100);

            subtotal += lineSubtotal;
            totalTax += taxAmount;
        }

        return subtotal + totalTax + request.ShippingAmount - request.DiscountAmount;
    }
}

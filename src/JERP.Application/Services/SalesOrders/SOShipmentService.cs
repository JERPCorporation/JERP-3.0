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

using JERP.Application.DTOs.SalesOrders;
using JERP.Application.Services.Security;
using JERP.Core.Entities.SalesOrders;
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.SalesOrders;

/// <summary>
/// Service for SO shipment management
/// </summary>
public class SOShipmentService : ISOShipmentService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<SOShipmentService> _logger;

    public SOShipmentService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<SOShipmentService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<SOShipmentDto?> GetByIdAsync(Guid id)
    {
        return await _context.SOShipments
            .Where(sh => sh.Id == id)
            .Select(sh => new SOShipmentDto
            {
                Id = sh.Id,
                ShipmentNumber = sh.ShipmentNumber,
                SalesOrderId = sh.SalesOrderId,
                SONumber = sh.SalesOrder.SONumber,
                CustomerId = sh.CustomerId,
                CustomerName = sh.Customer.CompanyName,
                ShipDate = sh.ShipDate,
                WarehouseId = sh.WarehouseId,
                WarehouseName = sh.Warehouse != null ? sh.Warehouse.Name : null,
                ShippingMethod = sh.ShippingMethod,
                TrackingNumber = sh.TrackingNumber,
                Carrier = sh.Carrier,
                ShippingCost = sh.ShippingCost,
                Status = sh.Status.ToString(),
                LineItems = sh.LineItems.Select(line => new SOShipmentLineDto
                {
                    Id = line.Id,
                    SalesOrderLineId = line.SalesOrderLineId,
                    InventoryItemId = line.InventoryItemId,
                    ItemCode = line.InventoryItem.SKU,
                    ItemName = line.InventoryItem.Name,
                    QuantityOrdered = line.SalesOrderLine.Quantity,
                    QuantityShipped = line.QuantityShipped,
                    QuantityPreviouslyShipped = line.SalesOrderLine.QuantityShipped - line.QuantityShipped,
                    UnitOfMeasure = line.InventoryItem.UnitOfMeasure,
                    BatchLotId = line.BatchLotId,
                    BatchNumber = line.BatchLot != null ? line.BatchLot.BatchNumber : null,
                    SerialNumber = line.SerialNumber,
                    BinLocation = line.BinLocation,
                    Notes = line.Notes
                }).ToList(),
                MetrcManifestNumber = sh.MetrcManifestNumber,
                MetrcManifestDate = sh.MetrcManifestDate,
                PackedBy = sh.PackedBy,
                ShippedBy = sh.ShippedBy,
                Notes = sh.Notes,
                CreatedAt = sh.CreatedAt,
                ShippedAt = sh.ShippedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<SOShipmentDto>> GetBySalesOrderAsync(Guid salesOrderId)
    {
        return await _context.SOShipments
            .Where(sh => sh.SalesOrderId == salesOrderId)
            .OrderByDescending(sh => sh.CreatedAt)
            .Select(sh => new SOShipmentDto
            {
                Id = sh.Id,
                ShipmentNumber = sh.ShipmentNumber,
                SalesOrderId = sh.SalesOrderId,
                SONumber = sh.SalesOrder.SONumber,
                ShipDate = sh.ShipDate,
                Status = sh.Status.ToString(),
                TrackingNumber = sh.TrackingNumber,
                CreatedAt = sh.CreatedAt,
                ShippedAt = sh.ShippedAt,
                LineItems = new List<SOShipmentLineDto>()
            })
            .ToListAsync();
    }

    public async Task<List<SOShipmentDto>> GetAllAsync(Guid companyId)
    {
        return await _context.SOShipments
            .Where(sh => sh.CompanyId == companyId)
            .OrderByDescending(sh => sh.CreatedAt)
            .Select(sh => new SOShipmentDto
            {
                Id = sh.Id,
                ShipmentNumber = sh.ShipmentNumber,
                SalesOrderId = sh.SalesOrderId,
                SONumber = sh.SalesOrder.SONumber,
                CustomerName = sh.Customer.CompanyName,
                ShipDate = sh.ShipDate,
                Status = sh.Status.ToString(),
                TrackingNumber = sh.TrackingNumber,
                CreatedAt = sh.CreatedAt,
                LineItems = new List<SOShipmentLineDto>()
            })
            .ToListAsync();
    }

    public async Task<SOShipmentDto> CreateAsync(Guid companyId, CreateSOShipmentRequest request)
    {
        // Validate sales order
        var salesOrder = await _context.SalesOrders
            .Include(so => so.LineItems)
            .FirstOrDefaultAsync(so => so.Id == request.SalesOrderId);

        if (salesOrder == null)
        {
            throw new InvalidOperationException($"Sales order {request.SalesOrderId} not found");
        }

        if (salesOrder.Status != SalesOrderStatus.Approved && salesOrder.Status != SalesOrderStatus.Processing)
        {
            throw new InvalidOperationException("Sales order must be approved to create shipments");
        }

        // Generate shipment number
        var shipmentNumber = await GenerateShipmentNumberAsync(companyId);

        var shipment = new SOShipment
        {
            CompanyId = companyId,
            ShipmentNumber = shipmentNumber,
            SalesOrderId = request.SalesOrderId,
            CustomerId = salesOrder.CustomerId,
            ShipDate = request.ShipDate,
            WarehouseId = request.WarehouseId,
            ShippingMethod = request.ShippingMethod,
            TrackingNumber = request.TrackingNumber,
            Carrier = request.Carrier,
            ShippingCost = request.ShippingCost,
            Status = ShipmentStatus.Draft,
            MetrcManifestNumber = request.MetrcManifestNumber,
            Notes = request.Notes
        };

        // Add line items and update SO line quantities
        foreach (var lineRequest in request.LineItems)
        {
            var soLine = salesOrder.LineItems.FirstOrDefault(l => l.Id == lineRequest.SalesOrderLineId);
            if (soLine == null)
            {
                throw new InvalidOperationException($"Sales order line {lineRequest.SalesOrderLineId} not found");
            }

            var remainingQty = soLine.Quantity - soLine.QuantityShipped;
            if (lineRequest.QuantityShipped > remainingQty)
            {
                throw new InvalidOperationException($"Cannot ship more than remaining quantity for line {soLine.LineNumber}");
            }

            var shipmentLine = new SOShipmentLine
            {
                SalesOrderLineId = lineRequest.SalesOrderLineId,
                InventoryItemId = soLine.InventoryItemId,
                QuantityShipped = lineRequest.QuantityShipped,
                BatchLotId = lineRequest.BatchLotId,
                SerialNumber = lineRequest.SerialNumber,
                BinLocation = lineRequest.BinLocation,
                Notes = lineRequest.Notes
            };

            shipment.LineItems.Add(shipmentLine);

            // Update SO line quantities
            soLine.QuantityShipped += lineRequest.QuantityShipped;
        }

        // Update SO status
        salesOrder.Status = SalesOrderStatus.Processing;
        salesOrder.TotalShipped = salesOrder.LineItems.Sum(l => l.QuantityShipped * l.UnitPrice);

        // Check if fully shipped
        var allLinesFullyShipped = salesOrder.LineItems.All(l => l.QuantityShipped >= l.Quantity);
        if (allLinesFullyShipped)
        {
            salesOrder.Status = SalesOrderStatus.Shipped;
            salesOrder.IsFullyShipped = true;
        }

        _context.SOShipments.Add(shipment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created shipment {ShipmentNumber} for SO {SONumber}", shipmentNumber, salesOrder.SONumber);

        return (await GetByIdAsync(shipment.Id))!;
    }

    public async Task<SOShipmentDto> ShipAsync(Guid id, string shippedBy)
    {
        var shipment = await _context.SOShipments
            .Include(sh => sh.LineItems)
            .FirstOrDefaultAsync(sh => sh.Id == id);

        if (shipment == null)
        {
            throw new InvalidOperationException($"Shipment {id} not found");
        }

        if (shipment.Status != ShipmentStatus.Draft && shipment.Status != ShipmentStatus.Picked && shipment.Status != ShipmentStatus.Packed)
        {
            throw new InvalidOperationException("Shipment is already shipped");
        }

        // Deduct inventory
        foreach (var line in shipment.LineItems)
        {
            var inventoryLevel = await _context.InventoryLevels
                .FirstOrDefaultAsync(il => il.ProductId == line.InventoryItemId && il.WarehouseId == shipment.WarehouseId);

            if (inventoryLevel != null)
            {
                // Convert decimal quantity to int for inventory tracking
                // Note: QuantityOnHand is stored as int in InventoryLevel entity
                // Rounding is used to handle minor decimal differences
                var qtyChange = (int)Math.Round(line.QuantityShipped);
                inventoryLevel.QuantityOnHand -= qtyChange;

                // Create inventory transaction
                var transaction = new InventoryTransaction
                {
                    CompanyId = shipment.CompanyId,
                    ProductId = line.InventoryItemId,
                    WarehouseId = shipment.WarehouseId ?? Guid.Empty,
                    Type = InventoryTransactionType.Sale,
                    QuantityChange = -qtyChange,
                    UnitCost = 0, // TODO: Get from inventory
                    QuantityAfter = inventoryLevel.QuantityOnHand,
                    SourceType = "SOShipment",
                    SourceId = shipment.Id,
                    SourceNumber = shipment.ShipmentNumber,
                    TransactionDate = DateTime.UtcNow,
                    TransactedByUserId = Guid.Empty, // TODO: Get from current user
                    Notes = $"Shipped on {shipment.ShipmentNumber}"
                };

                _context.InventoryTransactions.Add(transaction);
            }
        }

        shipment.Status = ShipmentStatus.Shipped;
        shipment.ShippedBy = shippedBy;
        shipment.ShippedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Shipped {ShipmentNumber} by {ShippedBy}", shipment.ShipmentNumber, shippedBy);

        return (await GetByIdAsync(shipment.Id))!;
    }

    public async Task CancelAsync(Guid id)
    {
        var shipment = await _context.SOShipments
            .Include(sh => sh.LineItems)
            .ThenInclude(line => line.SalesOrderLine)
            .FirstOrDefaultAsync(sh => sh.Id == id);

        if (shipment == null)
        {
            throw new InvalidOperationException($"Shipment {id} not found");
        }

        if (shipment.Status == ShipmentStatus.Shipped)
        {
            throw new InvalidOperationException("Cannot cancel shipped shipments");
        }

        // Reverse SO line quantities
        foreach (var line in shipment.LineItems)
        {
            line.SalesOrderLine.QuantityShipped -= line.QuantityShipped;
        }

        _context.SOShipments.Remove(shipment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Cancelled shipment {ShipmentNumber}", shipment.ShipmentNumber);
    }

    private async Task<string> GenerateShipmentNumberAsync(Guid companyId)
    {
        var lastShipment = await _context.SOShipments
            .Where(sh => sh.CompanyId == companyId)
            .OrderByDescending(sh => sh.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastShipment == null)
        {
            return "SHIP-0001";
        }

        var parts = lastShipment.ShipmentNumber.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var lastNumber))
        {
            _logger.LogWarning("Invalid shipment number format: {ShipmentNumber}, generating from scratch", lastShipment.ShipmentNumber);
            return "SHIP-0001";
        }

        var nextNumber = lastNumber + 1;
        return $"SHIP-{nextNumber:D4}";
    }
}

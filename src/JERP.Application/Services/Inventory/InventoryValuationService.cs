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

using JERP.Application.DTOs.Inventory;
using JERP.Application.Services.Security;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Inventory;

/// <summary>
/// Service for inventory valuation
/// </summary>
public class InventoryValuationService : IInventoryValuationService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<InventoryValuationService> _logger;

    public InventoryValuationService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<InventoryValuationService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get inventory valuation for a company
    /// </summary>
    public async Task<IEnumerable<InventoryValuationDto>> GetValuationByCompanyAsync(Guid companyId, DateTime? asOfDate = null)
    {
        var valuationDate = asOfDate ?? DateTime.UtcNow;

        var inventoryLevels = await _context.InventoryLevels
            .Include(l => l.Product)
            .Include(l => l.Warehouse)
            .Where(l => l.Product.CompanyId == companyId && l.QuantityOnHand > 0)
            .ToListAsync();

        var valuations = inventoryLevels
            .GroupBy(l => new { l.ProductId, l.Product.SKU, l.Product.Name, l.Product.UnitOfMeasure })
            .Select(g => new InventoryValuationDto
            {
                InventoryItemId = g.Key.ProductId,
                ItemCode = g.Key.SKU,
                ItemName = g.Key.Name,
                QuantityOnHand = g.Sum(l => l.QuantityOnHand),
                AverageCost = g.Sum(l => l.TotalValue) / Math.Max(g.Sum(l => l.QuantityOnHand), 1),
                TotalValue = g.Sum(l => l.TotalValue),
                UnitOfMeasure = g.Key.UnitOfMeasure,
                WarehouseId = null,
                WarehouseName = null,
                ValuationDate = valuationDate
            })
            .OrderBy(v => v.ItemCode)
            .ToList();

        _logger.LogInformation("Retrieved inventory valuation for company {CompanyId} - {Count} items",
            companyId, valuations.Count);

        return valuations;
    }

    /// <summary>
    /// Get inventory valuation for a specific warehouse
    /// </summary>
    public async Task<IEnumerable<InventoryValuationDto>> GetValuationByWarehouseAsync(Guid warehouseId, DateTime? asOfDate = null)
    {
        var valuationDate = asOfDate ?? DateTime.UtcNow;

        var inventoryLevels = await _context.InventoryLevels
            .Include(l => l.Product)
            .Include(l => l.Warehouse)
            .Where(l => l.WarehouseId == warehouseId && l.QuantityOnHand > 0)
            .ToListAsync();

        var valuations = inventoryLevels
            .Select(l => new InventoryValuationDto
            {
                InventoryItemId = l.ProductId,
                ItemCode = l.Product.SKU,
                ItemName = l.Product.Name,
                QuantityOnHand = l.QuantityOnHand,
                AverageCost = l.AverageCost,
                TotalValue = l.TotalValue,
                UnitOfMeasure = l.Product.UnitOfMeasure,
                WarehouseId = l.WarehouseId,
                WarehouseName = l.Warehouse?.Name,
                ValuationDate = valuationDate
            })
            .OrderBy(v => v.ItemCode)
            .ToList();

        _logger.LogInformation("Retrieved inventory valuation for warehouse {WarehouseId} - {Count} items",
            warehouseId, valuations.Count);

        return valuations;
    }

    /// <summary>
    /// Get inventory valuation for a specific item
    /// </summary>
    public async Task<InventoryValuationDto> GetValuationByItemAsync(Guid itemId, DateTime? asOfDate = null)
    {
        var valuationDate = asOfDate ?? DateTime.UtcNow;

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == itemId);

        if (product == null)
        {
            throw new InvalidOperationException($"Product {itemId} not found");
        }

        var inventoryLevels = await _context.InventoryLevels
            .Include(l => l.Warehouse)
            .Where(l => l.ProductId == itemId && l.QuantityOnHand > 0)
            .ToListAsync();

        var totalQuantity = inventoryLevels.Sum(l => l.QuantityOnHand);
        var totalValue = inventoryLevels.Sum(l => l.TotalValue);
        var averageCost = totalQuantity > 0 ? totalValue / totalQuantity : 0;

        var valuation = new InventoryValuationDto
        {
            InventoryItemId = itemId,
            ItemCode = product.SKU,
            ItemName = product.Name,
            QuantityOnHand = totalQuantity,
            AverageCost = averageCost,
            TotalValue = totalValue,
            UnitOfMeasure = product.UnitOfMeasure,
            WarehouseId = null,
            WarehouseName = null,
            ValuationDate = valuationDate
        };

        _logger.LogInformation("Retrieved inventory valuation for item {ItemId} - Qty: {Quantity}, Value: {Value}",
            itemId, totalQuantity, totalValue);

        return valuation;
    }

    /// <summary>
    /// Get total inventory value for a company
    /// </summary>
    public async Task<decimal> GetTotalInventoryValueAsync(Guid companyId, DateTime? asOfDate = null)
    {
        var totalValue = await _context.InventoryLevels
            .Include(l => l.Product)
            .Where(l => l.Product.CompanyId == companyId && l.QuantityOnHand > 0)
            .SumAsync(l => l.TotalValue);

        _logger.LogInformation("Retrieved total inventory value for company {CompanyId}: {TotalValue}",
            companyId, totalValue);

        return totalValue;
    }
}

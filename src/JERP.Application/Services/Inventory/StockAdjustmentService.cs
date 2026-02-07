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

using JERP.Application.DTOs.Inventory;
using JERP.Application.Services.Security;
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Inventory;

/// <summary>
/// Service for stock adjustment management
/// </summary>
public class StockAdjustmentService : IStockAdjustmentService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<StockAdjustmentService> _logger;

    public StockAdjustmentService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<StockAdjustmentService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get stock adjustment by ID
    /// </summary>
    public async Task<StockAdjustmentDto> GetByIdAsync(Guid id)
    {
        var adjustment = await _context.InventoryAdjustments
            .Include(a => a.Warehouse)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Product)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Batch)
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (adjustment == null)
        {
            throw new InvalidOperationException($"Stock adjustment {id} not found");
        }

        return MapToDto(adjustment);
    }

    /// <summary>
    /// Get all stock adjustments for a company
    /// </summary>
    public async Task<IEnumerable<StockAdjustmentDto>> GetAllAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.InventoryAdjustments
            .Include(a => a.Warehouse)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Product)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Batch)
            .Where(a => a.CompanyId == companyId);

        if (startDate.HasValue)
        {
            query = query.Where(a => a.AdjustmentDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.AdjustmentDate <= endDate.Value);
        }

        var adjustments = await query
            .OrderByDescending(a => a.AdjustmentDate)
            .ToListAsync();

        return adjustments.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get stock adjustments by status
    /// </summary>
    public async Task<IEnumerable<StockAdjustmentDto>> GetByStatusAsync(Guid companyId, string status)
    {
        if (!Enum.TryParse<AdjustmentStatus>(status, true, out var adjustmentStatus))
        {
            throw new ArgumentException($"Invalid adjustment status: {status}");
        }

        var adjustments = await _context.InventoryAdjustments
            .Include(a => a.Warehouse)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Product)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Batch)
            .Where(a => a.CompanyId == companyId && a.Status == adjustmentStatus)
            .OrderByDescending(a => a.AdjustmentDate)
            .ToListAsync();

        return adjustments.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get stock adjustments by item ID
    /// </summary>
    public async Task<IEnumerable<StockAdjustmentDto>> GetByItemIdAsync(Guid itemId)
    {
        var adjustments = await _context.InventoryAdjustments
            .Include(a => a.Warehouse)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Product)
            .Include(a => a.Lines)
            .ThenInclude(l => l.Batch)
            .Where(a => a.Lines.Any(l => l.ProductId == itemId))
            .OrderByDescending(a => a.AdjustmentDate)
            .ToListAsync();

        return adjustments.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Create a new stock adjustment
    /// </summary>
    public async Task<StockAdjustmentDto> CreateAsync(Guid companyId, CreateStockAdjustmentRequest request, string userId)
    {
        var product = await _context.Products
            .Include(p => p.InventoryLevels)
            .FirstOrDefaultAsync(p => p.Id == request.InventoryItemId);

        if (product == null)
        {
            throw new InvalidOperationException($"Product {request.InventoryItemId} not found");
        }

        var warehouseId = request.WarehouseId ?? product.DefaultWarehouseId ?? Guid.Empty;
        var adjustmentNumber = await GenerateAdjustmentNumberAsync(companyId);

        var inventoryLevel = await _context.InventoryLevels
            .FirstOrDefaultAsync(l => l.ProductId == request.InventoryItemId && l.WarehouseId == warehouseId);

        var quantityBefore = inventoryLevel?.QuantityOnHand ?? 0;

        if (!Enum.TryParse<AdjustmentType>(request.AdjustmentType, true, out var adjustmentType))
        {
            adjustmentType = request.AdjustmentQuantity >= 0 ? AdjustmentType.Increase : AdjustmentType.Decrease;
        }

        var adjustment = new InventoryAdjustment
        {
            CompanyId = companyId,
            WarehouseId = warehouseId,
            AdjustmentNumber = adjustmentNumber,
            AdjustmentDate = request.AdjustmentDate ?? DateTime.UtcNow,
            AdjustedByUserId = Guid.Parse(userId),
            Type = adjustmentType,
            Reason = ParseAdjustmentReason(request.Reason),
            TotalValue = Math.Abs(request.AdjustmentQuantity * product.StandardCost),
            Notes = request.Notes,
            Status = AdjustmentStatus.Draft
        };

        _context.InventoryAdjustments.Add(adjustment);
        await _context.SaveChangesAsync();

        var adjustmentLine = new InventoryAdjustmentLine
        {
            AdjustmentId = adjustment.Id,
            ProductId = request.InventoryItemId,
            BatchId = request.BatchLotId,
            QuantityBefore = quantityBefore,
            QuantityAdjustment = (int)request.AdjustmentQuantity,
            UnitCost = product.StandardCost,
            Notes = request.Notes
        };

        _context.InventoryAdjustmentLines.Add(adjustmentLine);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created stock adjustment {AdjustmentNumber} for company {CompanyId}",
            adjustment.AdjustmentNumber, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "adjustment_created",
                $"InventoryAdjustment:{adjustment.Id}",
                $"Created stock adjustment {adjustment.AdjustmentNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for stock adjustment creation");
        }

        return await GetByIdAsync(adjustment.Id);
    }

    /// <summary>
    /// Approve a stock adjustment
    /// </summary>
    public async Task<StockAdjustmentDto> ApproveAsync(Guid id, string userId)
    {
        var adjustment = await _context.InventoryAdjustments
            .Include(a => a.Lines)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
        {
            throw new InvalidOperationException($"Stock adjustment {id} not found");
        }

        if (adjustment.Status != AdjustmentStatus.Draft)
        {
            throw new InvalidOperationException($"Only draft adjustments can be approved");
        }

        adjustment.Status = AdjustmentStatus.Approved;
        adjustment.ApprovedDate = DateTime.UtcNow;
        adjustment.ApprovedByUserId = Guid.Parse(userId);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Approved stock adjustment {AdjustmentNumber}",
            adjustment.AdjustmentNumber);

        try
        {
            await _auditLogService.LogAction(
                adjustment.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "adjustment_approved",
                $"InventoryAdjustment:{adjustment.Id}",
                $"Approved stock adjustment {adjustment.AdjustmentNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for stock adjustment approval");
        }

        return await GetByIdAsync(adjustment.Id);
    }

    /// <summary>
    /// Post a stock adjustment to inventory
    /// </summary>
    public async Task<StockAdjustmentDto> PostAsync(Guid id, string userId)
    {
        var adjustment = await _context.InventoryAdjustments
            .Include(a => a.Lines)
            .ThenInclude(l => l.Product)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
        {
            throw new InvalidOperationException($"Stock adjustment {id} not found");
        }

        if (adjustment.Status != AdjustmentStatus.Approved)
        {
            throw new InvalidOperationException($"Only approved adjustments can be posted");
        }

        foreach (var line in adjustment.Lines)
        {
            var transaction = new InventoryTransaction
            {
                CompanyId = adjustment.CompanyId,
                ProductId = line.ProductId,
                WarehouseId = adjustment.WarehouseId,
                BatchId = line.BatchId,
                TransactionDate = adjustment.AdjustmentDate,
                Type = InventoryTransactionType.Adjustment,
                QuantityChange = line.QuantityAdjustment,
                UnitCost = line.UnitCost,
                QuantityAfter = line.QuantityAfter,
                SourceType = "Adjustment",
                SourceId = adjustment.Id,
                SourceNumber = adjustment.AdjustmentNumber,
                Notes = adjustment.Notes,
                TransactedByUserId = adjustment.AdjustedByUserId
            };

            _context.InventoryTransactions.Add(transaction);

            var level = await _context.InventoryLevels
                .FirstOrDefaultAsync(l => l.ProductId == line.ProductId && l.WarehouseId == adjustment.WarehouseId);

            if (level == null)
            {
                level = new InventoryLevel
                {
                    ProductId = line.ProductId,
                    WarehouseId = adjustment.WarehouseId,
                    QuantityOnHand = 0,
                    QuantityReserved = 0,
                    QuantityOnOrder = 0,
                    TotalValue = 0,
                    AverageCost = 0,
                    LastStockDate = DateTime.UtcNow,
                    LastCountDate = DateTime.UtcNow
                };
                _context.InventoryLevels.Add(level);
            }

            level.QuantityOnHand += line.QuantityAdjustment;
            level.LastStockDate = DateTime.UtcNow;

            if (level.QuantityOnHand > 0)
            {
                var currentValue = level.TotalValue;
                var addedValue = line.QuantityAdjustment * line.UnitCost;
                level.TotalValue = currentValue + addedValue;
                level.AverageCost = level.TotalValue / level.QuantityOnHand;
            }
            else
            {
                level.TotalValue = 0;
                level.AverageCost = 0;
            }
        }

        adjustment.Status = AdjustmentStatus.Posted;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Posted stock adjustment {AdjustmentNumber}",
            adjustment.AdjustmentNumber);

        try
        {
            await _auditLogService.LogAction(
                adjustment.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "adjustment_posted",
                $"InventoryAdjustment:{adjustment.Id}",
                $"Posted stock adjustment {adjustment.AdjustmentNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for stock adjustment posting");
        }

        return await GetByIdAsync(adjustment.Id);
    }

    /// <summary>
    /// Reject a stock adjustment
    /// </summary>
    public async Task<bool> RejectAsync(Guid id, string reason, string userId)
    {
        var adjustment = await _context.InventoryAdjustments.FindAsync(id);
        if (adjustment == null)
        {
            return false;
        }

        if (adjustment.Status != AdjustmentStatus.Draft)
        {
            throw new InvalidOperationException($"Only draft adjustments can be rejected");
        }

        adjustment.Notes = $"REJECTED: {reason}\n{adjustment.Notes}";
        await _context.SaveChangesAsync();

        _logger.LogWarning("Rejected stock adjustment {AdjustmentNumber} - Reason: {Reason}",
            adjustment.AdjustmentNumber, reason);

        try
        {
            await _auditLogService.LogAction(
                adjustment.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "adjustment_rejected",
                $"InventoryAdjustment:{adjustment.Id}",
                $"Rejected stock adjustment {adjustment.AdjustmentNumber} - {reason}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for stock adjustment rejection");
        }

        return true;
    }

    private async Task<string> GenerateAdjustmentNumberAsync(Guid companyId)
    {
        var lastAdjustment = await _context.InventoryAdjustments
            .Where(a => a.CompanyId == companyId)
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastAdjustment == null)
        {
            return "ADJ-0001";
        }

        var parts = lastAdjustment.AdjustmentNumber.Split('-');
        if (parts.Length >= 2 && int.TryParse(parts[1], out var lastNumber))
        {
            var nextNumber = lastNumber + 1;
            return $"ADJ-{nextNumber:D4}";
        }

        return "ADJ-0001";
    }

    private static AdjustmentReason ParseAdjustmentReason(string reason)
    {
        if (Enum.TryParse<AdjustmentReason>(reason, true, out var parsedReason))
        {
            return parsedReason;
        }

        return reason.ToLower() switch
        {
            var r when r.Contains("damage") => AdjustmentReason.Damage,
            var r when r.Contains("expire") => AdjustmentReason.Expired,
            var r when r.Contains("shrink") => AdjustmentReason.Shrinkage,
            var r when r.Contains("theft") => AdjustmentReason.Theft,
            var r when r.Contains("found") => AdjustmentReason.Found,
            var r when r.Contains("count") => AdjustmentReason.PhysicalCountVariance,
            var r when r.Contains("write") => AdjustmentReason.WriteOff,
            var r when r.Contains("return") => AdjustmentReason.Returned,
            _ => AdjustmentReason.Other
        };
    }

    private static StockAdjustmentDto MapToDto(InventoryAdjustment adjustment)
    {
        var firstLine = adjustment.Lines.FirstOrDefault();

        return new StockAdjustmentDto
        {
            Id = adjustment.Id,
            AdjustmentNumber = adjustment.AdjustmentNumber,
            InventoryItemId = firstLine?.ProductId ?? Guid.Empty,
            ItemName = firstLine?.Product?.Name ?? string.Empty,
            BatchLotId = firstLine?.BatchId,
            BatchNumber = firstLine?.Batch?.BatchNumber,
            QuantityBefore = firstLine?.QuantityBefore ?? 0,
            AdjustmentQuantity = firstLine?.QuantityAdjustment ?? 0,
            QuantityAfter = firstLine?.QuantityAfter ?? 0,
            AdjustmentType = adjustment.Type.ToString(),
            Reason = adjustment.Reason.ToString(),
            AdjustmentDate = adjustment.AdjustmentDate,
            WarehouseId = adjustment.WarehouseId,
            WarehouseName = adjustment.Warehouse?.Name,
            BinLocation = null,
            Notes = adjustment.Notes,
            ApprovedBy = null,
            ApprovedAt = adjustment.ApprovedDate,
            Status = adjustment.Status.ToString(),
            CreatedBy = null,
            CreatedAt = adjustment.CreatedAt
        };
    }
}

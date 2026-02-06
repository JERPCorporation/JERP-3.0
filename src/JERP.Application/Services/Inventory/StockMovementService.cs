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

using JERP.Application.DTOs.Inventory;
using JERP.Application.Services.Security;
using JERP.Core.Entities.Inventory;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Inventory;

/// <summary>
/// Service for stock movement management
/// </summary>
public class StockMovementService : IStockMovementService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<StockMovementService> _logger;

    public StockMovementService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<StockMovementService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get stock movement by ID
    /// </summary>
    public async Task<StockMovementDto> GetByIdAsync(Guid id)
    {
        var movement = await _context.InventoryTransactions
            .Include(t => t.Product)
            .Include(t => t.Warehouse)
            .Include(t => t.Batch)
            .Where(t => t.Id == id)
            .Select(t => MapToDto(t))
            .FirstOrDefaultAsync();

        if (movement == null)
        {
            throw new InvalidOperationException($"Stock movement {id} not found");
        }

        return movement;
    }

    /// <summary>
    /// Get stock movements by item ID
    /// </summary>
    public async Task<IEnumerable<StockMovementDto>> GetByItemIdAsync(Guid itemId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.InventoryTransactions
            .Include(t => t.Product)
            .Include(t => t.Warehouse)
            .Include(t => t.Batch)
            .Where(t => t.ProductId == itemId);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= endDate.Value);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    /// <summary>
    /// Get stock movements by warehouse ID
    /// </summary>
    public async Task<IEnumerable<StockMovementDto>> GetByWarehouseIdAsync(Guid warehouseId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.InventoryTransactions
            .Include(t => t.Product)
            .Include(t => t.Warehouse)
            .Include(t => t.Batch)
            .Where(t => t.WarehouseId == warehouseId);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= endDate.Value);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    /// <summary>
    /// Get stock movements by type
    /// </summary>
    public async Task<IEnumerable<StockMovementDto>> GetByTypeAsync(Guid companyId, string movementType, DateTime? startDate = null, DateTime? endDate = null)
    {
        if (!Enum.TryParse<InventoryTransactionType>(movementType, true, out var transactionType))
        {
            throw new ArgumentException($"Invalid movement type: {movementType}");
        }

        var query = _context.InventoryTransactions
            .Include(t => t.Product)
            .Include(t => t.Warehouse)
            .Include(t => t.Batch)
            .Where(t => t.CompanyId == companyId && t.Type == transactionType);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= endDate.Value);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => MapToDto(t))
            .ToListAsync();
    }

    /// <summary>
    /// Create a receipt movement
    /// </summary>
    public async Task<StockMovementDto> CreateReceiptAsync(CreateStockMovementRequest request, string userId)
    {
        var transaction = await CreateMovementAsync(request, InventoryTransactionType.Receipt, userId);
        
        await UpdateInventoryLevel(transaction.ProductId, transaction.WarehouseId, transaction.QuantityChange, transaction.UnitCost);
        
        return await GetByIdAsync(transaction.Id);
    }

    /// <summary>
    /// Create an issue movement
    /// </summary>
    public async Task<StockMovementDto> CreateIssueAsync(CreateStockMovementRequest request, string userId)
    {
        var transaction = await CreateMovementAsync(request, InventoryTransactionType.Sale, userId);
        
        await UpdateInventoryLevel(transaction.ProductId, transaction.WarehouseId, -transaction.QuantityChange, transaction.UnitCost);
        
        return await GetByIdAsync(transaction.Id);
    }

    /// <summary>
    /// Create a transfer movement
    /// </summary>
    public async Task<StockMovementDto> CreateTransferAsync(CreateStockMovementRequest request, string userId)
    {
        if (!request.FromWarehouseId.HasValue || !request.ToWarehouseId.HasValue)
        {
            throw new InvalidOperationException("Transfer requires both FromWarehouseId and ToWarehouseId");
        }

        var transferOut = await CreateMovementAsync(request, InventoryTransactionType.TransferOut, userId, request.FromWarehouseId.Value);
        await UpdateInventoryLevel(transferOut.ProductId, request.FromWarehouseId.Value, -(int)request.Quantity, transferOut.UnitCost);

        var transferIn = await CreateMovementAsync(request, InventoryTransactionType.TransferIn, userId, request.ToWarehouseId.Value);
        await UpdateInventoryLevel(transferIn.ProductId, request.ToWarehouseId.Value, (int)request.Quantity, transferIn.UnitCost);

        _logger.LogInformation("Created transfer from warehouse {FromWarehouse} to {ToWarehouse}",
            request.FromWarehouseId.Value, request.ToWarehouseId.Value);

        return await GetByIdAsync(transferOut.Id);
    }

    /// <summary>
    /// Create a return movement
    /// </summary>
    public async Task<StockMovementDto> CreateReturnAsync(CreateStockMovementRequest request, string userId)
    {
        var transaction = await CreateMovementAsync(request, InventoryTransactionType.Return, userId);
        
        await UpdateInventoryLevel(transaction.ProductId, transaction.WarehouseId, transaction.QuantityChange, transaction.UnitCost);
        
        return await GetByIdAsync(transaction.Id);
    }

    /// <summary>
    /// Reverse a stock movement
    /// </summary>
    public async Task<bool> ReverseMovementAsync(Guid movementId, string reason, string userId)
    {
        var originalTransaction = await _context.InventoryTransactions
            .Include(t => t.Product)
            .FirstOrDefaultAsync(t => t.Id == movementId);

        if (originalTransaction == null)
        {
            return false;
        }

        var reversalTransaction = new InventoryTransaction
        {
            CompanyId = originalTransaction.CompanyId,
            ProductId = originalTransaction.ProductId,
            WarehouseId = originalTransaction.WarehouseId,
            BatchId = originalTransaction.BatchId,
            TransactionDate = DateTime.UtcNow,
            Type = originalTransaction.Type,
            QuantityChange = -originalTransaction.QuantityChange,
            UnitCost = originalTransaction.UnitCost,
            QuantityAfter = originalTransaction.QuantityAfter - originalTransaction.QuantityChange,
            SourceType = "Reversal",
            SourceId = originalTransaction.Id,
            SourceNumber = originalTransaction.SourceNumber,
            Notes = $"Reversal of {originalTransaction.SourceNumber}: {reason}",
            TransactedByUserId = Guid.Parse(userId)
        };

        _context.InventoryTransactions.Add(reversalTransaction);
        await UpdateInventoryLevel(reversalTransaction.ProductId, reversalTransaction.WarehouseId, reversalTransaction.QuantityChange, reversalTransaction.UnitCost);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Reversed movement {MovementId} - Reason: {Reason}",
            movementId, reason);

        try
        {
            await _auditLogService.LogAction(
                originalTransaction.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "movement_reversed",
                $"InventoryTransaction:{reversalTransaction.Id}",
                $"Reversed movement for {originalTransaction.Product.Name} - {reason}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for movement reversal");
        }

        return true;
    }

    private async Task<InventoryTransaction> CreateMovementAsync(
        CreateStockMovementRequest request, 
        InventoryTransactionType transactionType, 
        string userId,
        Guid? specificWarehouseId = null)
    {
        var product = await _context.Products.FindAsync(request.InventoryItemId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product {request.InventoryItemId} not found");
        }

        var warehouseId = specificWarehouseId ?? request.ToWarehouseId ?? product.DefaultWarehouseId ?? Guid.Empty;

        var currentLevel = await _context.InventoryLevels
            .FirstOrDefaultAsync(l => l.ProductId == request.InventoryItemId && l.WarehouseId == warehouseId);

        var currentQuantity = currentLevel?.QuantityOnHand ?? 0;
        var quantityChange = transactionType == InventoryTransactionType.TransferOut || transactionType == InventoryTransactionType.Sale 
            ? -(int)request.Quantity 
            : (int)request.Quantity;

        var transaction = new InventoryTransaction
        {
            CompanyId = product.CompanyId,
            ProductId = request.InventoryItemId,
            WarehouseId = warehouseId,
            BatchId = request.BatchLotId,
            TransactionDate = request.TransactionDate ?? DateTime.UtcNow,
            Type = transactionType,
            QuantityChange = quantityChange,
            UnitCost = request.UnitCost ?? product.StandardCost,
            QuantityAfter = currentQuantity + quantityChange,
            SourceType = request.ReferenceType,
            SourceId = request.ReferenceId,
            SourceNumber = request.ReferenceNumber,
            Notes = request.Notes,
            TransactedByUserId = Guid.Parse(userId)
        };

        _context.InventoryTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {TransactionType} movement for product {ProductId} - Quantity: {Quantity}",
            transactionType, request.InventoryItemId, request.Quantity);

        try
        {
            await _auditLogService.LogAction(
                product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "stock_movement_created",
                $"InventoryTransaction:{transaction.Id}",
                $"Created {transactionType} movement for {product.Name} - Qty: {request.Quantity}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for stock movement");
        }

        return transaction;
    }

    private async Task UpdateInventoryLevel(Guid productId, Guid warehouseId, int quantityChange, decimal unitCost)
    {
        var level = await _context.InventoryLevels
            .FirstOrDefaultAsync(l => l.ProductId == productId && l.WarehouseId == warehouseId);

        if (level == null)
        {
            level = new InventoryLevel
            {
                ProductId = productId,
                WarehouseId = warehouseId,
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

        level.QuantityOnHand += quantityChange;
        level.LastStockDate = DateTime.UtcNow;

        if (level.QuantityOnHand > 0)
        {
            var currentValue = level.TotalValue;
            var addedValue = quantityChange * unitCost;
            level.TotalValue = currentValue + addedValue;
            level.AverageCost = level.TotalValue / level.QuantityOnHand;
        }
        else
        {
            level.TotalValue = 0;
            level.AverageCost = 0;
        }

        await _context.SaveChangesAsync();
    }

    private static StockMovementDto MapToDto(InventoryTransaction transaction)
    {
        return new StockMovementDto
        {
            Id = transaction.Id,
            InventoryItemId = transaction.ProductId,
            ItemName = transaction.Product?.Name ?? string.Empty,
            MovementType = transaction.Type.ToString(),
            Quantity = Math.Abs(transaction.QuantityChange),
            UnitOfMeasure = transaction.Product?.UnitOfMeasure ?? "Each",
            UnitCost = transaction.UnitCost,
            BatchLotId = transaction.BatchId,
            BatchNumber = transaction.Batch?.BatchNumber,
            FromWarehouseId = transaction.Type == InventoryTransactionType.TransferOut ? transaction.WarehouseId : null,
            FromWarehouseName = transaction.Type == InventoryTransactionType.TransferOut ? transaction.Warehouse?.Name : null,
            FromBinLocation = null,
            ToWarehouseId = transaction.Type != InventoryTransactionType.TransferOut ? transaction.WarehouseId : null,
            ToWarehouseName = transaction.Type != InventoryTransactionType.TransferOut ? transaction.Warehouse?.Name : null,
            ToBinLocation = null,
            ReferenceType = transaction.SourceType,
            ReferenceId = transaction.SourceId,
            ReferenceNumber = transaction.SourceNumber,
            TransactionDate = transaction.TransactionDate,
            Reason = null,
            Notes = transaction.Notes,
            CreatedBy = null,
            CreatedAt = transaction.CreatedAt
        };
    }
}

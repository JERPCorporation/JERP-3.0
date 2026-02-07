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
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Inventory;

/// <summary>
/// Service for batch/lot management
/// </summary>
public class BatchLotService : IBatchLotService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<BatchLotService> _logger;

    public BatchLotService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<BatchLotService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get batch by ID
    /// </summary>
    public async Task<BatchLotDto> GetByIdAsync(Guid id)
    {
        var batch = await _context.ProductBatches
            .Include(b => b.Product)
            .Include(b => b.Warehouse)
            .Where(b => b.Id == id)
            .Select(b => MapToDto(b))
            .FirstOrDefaultAsync();

        if (batch == null)
        {
            throw new InvalidOperationException($"Batch {id} not found");
        }

        return batch;
    }

    /// <summary>
    /// Get batches by item ID
    /// </summary>
    public async Task<IEnumerable<BatchLotDto>> GetByItemIdAsync(Guid itemId)
    {
        return await _context.ProductBatches
            .Include(b => b.Product)
            .Include(b => b.Warehouse)
            .Where(b => b.ProductId == itemId)
            .OrderByDescending(b => b.ReceivedDate)
            .Select(b => MapToDto(b))
            .ToListAsync();
    }

    /// <summary>
    /// Get active batches by item ID
    /// </summary>
    public async Task<IEnumerable<BatchLotDto>> GetActiveByItemIdAsync(Guid itemId)
    {
        return await _context.ProductBatches
            .Include(b => b.Product)
            .Include(b => b.Warehouse)
            .Where(b => b.ProductId == itemId && b.IsActive && !b.IsQuarantined && b.RemainingQuantity > 0)
            .OrderBy(b => b.ExpirationDate)
            .Select(b => MapToDto(b))
            .ToListAsync();
    }

    /// <summary>
    /// Get expiring batches
    /// </summary>
    public async Task<IEnumerable<BatchLotDto>> GetExpiringBatchesAsync(Guid companyId, int daysAhead)
    {
        var expirationThreshold = DateTime.UtcNow.AddDays(daysAhead);

        return await _context.ProductBatches
            .Include(b => b.Product)
            .Include(b => b.Warehouse)
            .Where(b => b.Product.CompanyId == companyId &&
                b.IsActive &&
                b.RemainingQuantity > 0 &&
                b.ExpirationDate.HasValue &&
                b.ExpirationDate.Value <= expirationThreshold)
            .OrderBy(b => b.ExpirationDate)
            .Select(b => MapToDto(b))
            .ToListAsync();
    }

    /// <summary>
    /// Get quarantined batches
    /// </summary>
    public async Task<IEnumerable<BatchLotDto>> GetQuarantinedBatchesAsync(Guid companyId)
    {
        return await _context.ProductBatches
            .Include(b => b.Product)
            .Include(b => b.Warehouse)
            .Where(b => b.Product.CompanyId == companyId && b.IsQuarantined)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => MapToDto(b))
            .ToListAsync();
    }

    /// <summary>
    /// Create a new batch/lot
    /// </summary>
    public async Task<BatchLotDto> CreateAsync(CreateBatchLotRequest request, string userId)
    {
        var product = await _context.Products.FindAsync(request.InventoryItemId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product {request.InventoryItemId} not found");
        }

        var batch = new ProductBatch
        {
            ProductId = request.InventoryItemId,
            WarehouseId = product.DefaultWarehouseId ?? Guid.Empty,
            BatchNumber = request.BatchNumber,
            ReceivedDate = request.ManufactureDate ?? DateTime.UtcNow,
            ExpirationDate = request.ExpirationDate,
            TestingDate = request.LabTestDate,
            Quantity = (int)request.Quantity,
            RemainingQuantity = (int)request.Quantity,
            UnitCost = 0,
            ActualTHC = request.TestedTHC,
            ActualCBD = request.TestedCBD,
            TestingPassed = request.LabTestDate.HasValue,
            TestCertificateUrl = request.LabTestCertificate,
            MetrcUID = request.MetrcTag,
            IsActive = true,
            IsQuarantined = false
        };

        _context.ProductBatches.Add(batch);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created batch {BatchNumber} for product {ProductId}",
            batch.BatchNumber, batch.ProductId);

        try
        {
            await _auditLogService.LogAction(
                product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "batch_created",
                $"ProductBatch:{batch.Id}",
                $"Created batch {batch.BatchNumber} for {product.Name}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for batch creation");
        }

        return await GetByIdAsync(batch.Id);
    }

    /// <summary>
    /// Update an existing batch/lot
    /// </summary>
    public async Task<BatchLotDto> UpdateAsync(Guid id, CreateBatchLotRequest request, string userId)
    {
        var batch = await _context.ProductBatches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
        {
            throw new InvalidOperationException($"Batch {id} not found");
        }

        batch.BatchNumber = request.BatchNumber;
        batch.ExpirationDate = request.ExpirationDate;
        batch.TestingDate = request.LabTestDate;
        batch.ActualTHC = request.TestedTHC;
        batch.ActualCBD = request.TestedCBD;
        batch.TestingPassed = request.LabTestDate.HasValue;
        batch.TestCertificateUrl = request.LabTestCertificate;
        batch.MetrcUID = request.MetrcTag;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated batch {BatchNumber}", batch.BatchNumber);

        try
        {
            await _auditLogService.LogAction(
                batch.Product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "batch_updated",
                $"ProductBatch:{batch.Id}",
                $"Updated batch {batch.BatchNumber}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for batch update");
        }

        return await GetByIdAsync(batch.Id);
    }

    /// <summary>
    /// Quarantine a batch
    /// </summary>
    public async Task<bool> QuarantineAsync(Guid id, string reason, string userId)
    {
        var batch = await _context.ProductBatches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
        {
            return false;
        }

        batch.IsQuarantined = true;
        batch.QuarantineReason = reason;
        await _context.SaveChangesAsync();

        _logger.LogWarning("Quarantined batch {BatchNumber} - Reason: {Reason}",
            batch.BatchNumber, reason);

        try
        {
            await _auditLogService.LogAction(
                batch.Product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "batch_quarantined",
                $"ProductBatch:{batch.Id}",
                $"Quarantined batch {batch.BatchNumber} - {reason}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for batch quarantine");
        }

        return true;
    }

    /// <summary>
    /// Release batch from quarantine
    /// </summary>
    public async Task<bool> ReleaseFromQuarantineAsync(Guid id, string userId)
    {
        var batch = await _context.ProductBatches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
        {
            return false;
        }

        batch.IsQuarantined = false;
        batch.QuarantineReason = null;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Released batch {BatchNumber} from quarantine",
            batch.BatchNumber);

        try
        {
            await _auditLogService.LogAction(
                batch.Product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "batch_released",
                $"ProductBatch:{batch.Id}",
                $"Released batch {batch.BatchNumber} from quarantine",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for batch release");
        }

        return true;
    }

    /// <summary>
    /// Deactivate a batch
    /// </summary>
    public async Task<bool> DeactivateAsync(Guid id)
    {
        var batch = await _context.ProductBatches.FindAsync(id);
        if (batch == null)
        {
            return false;
        }

        batch.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deactivated batch {BatchNumber}", batch.BatchNumber);

        return true;
    }

    private static BatchLotDto MapToDto(ProductBatch batch)
    {
        return new BatchLotDto
        {
            Id = batch.Id,
            InventoryItemId = batch.ProductId,
            ItemName = batch.Product?.Name ?? string.Empty,
            BatchNumber = batch.BatchNumber,
            LotNumber = null,
            Quantity = batch.Quantity,
            QuantityAvailable = batch.RemainingQuantity,
            ManufactureDate = batch.ReceivedDate,
            ExpirationDate = batch.ExpirationDate,
            VendorId = null,
            VendorName = null,
            VendorBatchNumber = null,
            IsActive = batch.IsActive,
            MetrcTag = batch.MetrcUID,
            LabTestCertificate = batch.TestCertificateUrl,
            LabTestDate = batch.TestingDate,
            TestedTHC = batch.ActualTHC,
            TestedCBD = batch.ActualCBD,
            IsQuarantined = batch.IsQuarantined,
            QuarantineReason = batch.QuarantineReason,
            CreatedAt = batch.CreatedAt,
            Notes = null
        };
    }
}

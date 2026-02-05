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
using JERP.Core.Entities.Inventory;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Inventory;

/// <summary>
/// Service for inventory item management
/// </summary>
public class InventoryItemService : IInventoryItemService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<InventoryItemService> _logger;

    public InventoryItemService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<InventoryItemService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get inventory item by ID
    /// </summary>
    public async Task<InventoryItemDto> GetByIdAsync(Guid id)
    {
        var item = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.Id == id)
            .Select(p => MapToDto(p))
            .FirstOrDefaultAsync();

        if (item == null)
        {
            throw new InvalidOperationException($"Inventory item {id} not found");
        }

        return item;
    }

    /// <summary>
    /// Get all inventory items for a company
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> GetAllAsync(Guid companyId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .OrderBy(p => p.SKU)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    /// <summary>
    /// Search inventory items
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> SearchAsync(Guid companyId, string searchTerm)
    {
        var normalizedSearch = searchTerm.ToLower();
        
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.CompanyId == companyId && p.IsActive &&
                (p.Name.ToLower().Contains(normalizedSearch) ||
                 p.SKU.ToLower().Contains(normalizedSearch) ||
                 (p.Barcode != null && p.Barcode.ToLower().Contains(normalizedSearch))))
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    /// <summary>
    /// Get inventory items by type
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> GetByTypeAsync(Guid companyId, string itemType)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    /// <summary>
    /// Get inventory items by category
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> GetByCategoryAsync(Guid companyId, string category)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.CompanyId == companyId && 
                p.IsActive && 
                p.Category.Name == category)
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    /// <summary>
    /// Get low stock items
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync(Guid companyId)
    {
        var items = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Include(p => p.InventoryLevels)
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .ToListAsync();

        var lowStockItems = items
            .Where(p => p.InventoryLevels.Sum(l => l.QuantityOnHand) <= p.ReorderPoint)
            .Select(p => MapToDto(p))
            .ToList();

        return lowStockItems;
    }

    /// <summary>
    /// Get cannabis products
    /// </summary>
    public async Task<IEnumerable<InventoryItemDto>> GetCannabisProductsAsync(Guid companyId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.DefaultWarehouse)
            .Where(p => p.CompanyId == companyId && p.IsActive && p.IsCannabisProduct)
            .OrderBy(p => p.Name)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    /// <summary>
    /// Create a new inventory item
    /// </summary>
    public async Task<InventoryItemDto> CreateAsync(Guid companyId, CreateInventoryItemRequest request, string userId)
    {
        var product = new Product
        {
            CompanyId = companyId,
            SKU = request.ItemCode,
            Name = request.ItemName,
            Description = request.Description,
            CategoryId = Guid.Empty,
            UnitOfMeasure = request.UnitOfMeasure,
            Barcode = request.Barcode,
            IsCannabisProduct = request.IsCannabisProduct,
            THCPercentage = request.THCPercentage,
            CBDPercentage = request.CBDPercentage,
            StrainType = request.StrainName,
            RequiresBatchTracking = request.IsBatchTracked,
            StandardCost = request.UnitCost,
            RetailPrice = request.UnitPrice,
            ReorderPoint = (int)request.ReorderLevel,
            ReorderQuantity = (int)request.ReorderQuantity,
            DefaultWarehouseId = request.WarehouseId,
            InventoryAssetAccountId = request.InventoryAccountId ?? Guid.Empty,
            COGSAccountId = request.COGSAccountId ?? Guid.Empty,
            RevenueAccountId = request.SalesAccountId ?? Guid.Empty,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created inventory item {ItemCode} - {ItemName} for company {CompanyId}",
            product.SKU, product.Name, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "inventory_item_created",
                $"Product:{product.Id}",
                $"Created inventory item {product.SKU} - {product.Name}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for inventory item creation");
        }

        return await GetByIdAsync(product.Id);
    }

    /// <summary>
    /// Update an existing inventory item
    /// </summary>
    public async Task<InventoryItemDto> UpdateAsync(Guid id, UpdateInventoryItemRequest request, string userId)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException($"Inventory item {id} not found");
        }

        if (request.ItemName != null) product.Name = request.ItemName;
        if (request.Description != null) product.Description = request.Description;
        if (request.SKU != null) product.SKU = request.SKU;
        if (request.Barcode != null) product.Barcode = request.Barcode;
        if (request.UnitCost.HasValue) product.StandardCost = request.UnitCost.Value;
        if (request.UnitPrice.HasValue) product.RetailPrice = request.UnitPrice.Value;
        if (request.ReorderLevel.HasValue) product.ReorderPoint = (int)request.ReorderLevel.Value;
        if (request.ReorderQuantity.HasValue) product.ReorderQuantity = (int)request.ReorderQuantity.Value;
        if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;
        if (request.THCPercentage.HasValue) product.THCPercentage = request.THCPercentage;
        if (request.CBDPercentage.HasValue) product.CBDPercentage = request.CBDPercentage;
        if (request.StrainName != null) product.StrainType = request.StrainName;
        if (request.WarehouseId.HasValue) product.DefaultWarehouseId = request.WarehouseId;
        if (request.InventoryAccountId.HasValue) product.InventoryAssetAccountId = request.InventoryAccountId.Value;
        if (request.COGSAccountId.HasValue) product.COGSAccountId = request.COGSAccountId.Value;
        if (request.SalesAccountId.HasValue) product.RevenueAccountId = request.SalesAccountId.Value;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated inventory item {ItemCode} - {ItemName}",
            product.SKU, product.Name);

        try
        {
            await _auditLogService.LogAction(
                product.CompanyId,
                Guid.Parse(userId),
                "system@jerp.io",
                "System",
                "inventory_item_updated",
                $"Product:{product.Id}",
                $"Updated inventory item {product.SKU} - {product.Name}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for inventory item update");
        }

        return await GetByIdAsync(product.Id);
    }

    /// <summary>
    /// Delete an inventory item
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        product.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted (soft) inventory item {ItemCode} - {ItemName}",
            product.SKU, product.Name);

        return true;
    }

    /// <summary>
    /// Activate an inventory item
    /// </summary>
    public async Task<bool> ActivateAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        product.IsActive = true;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Activated inventory item {ItemCode} - {ItemName}",
            product.SKU, product.Name);

        return true;
    }

    /// <summary>
    /// Deactivate an inventory item
    /// </summary>
    public async Task<bool> DeactivateAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        product.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deactivated inventory item {ItemCode} - {ItemName}",
            product.SKU, product.Name);

        return true;
    }

    /// <summary>
    /// Get available quantity for an item
    /// </summary>
    public async Task<decimal> GetAvailableQuantityAsync(Guid itemId)
    {
        var levels = await _context.InventoryLevels
            .Where(l => l.ProductId == itemId)
            .ToListAsync();

        return levels.Sum(l => l.QuantityAvailable);
    }

    /// <summary>
    /// Get total inventory value for a company
    /// </summary>
    public async Task<decimal> GetTotalValueAsync(Guid companyId)
    {
        var products = await _context.Products
            .Include(p => p.InventoryLevels)
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .ToListAsync();

        return products.Sum(p => p.InventoryLevels.Sum(l => l.TotalValue));
    }

    private static InventoryItemDto MapToDto(Product product)
    {
        var totalQuantity = product.InventoryLevels?.Sum(l => l.QuantityOnHand) ?? 0;
        var totalReserved = product.InventoryLevels?.Sum(l => l.QuantityReserved) ?? 0;

        return new InventoryItemDto
        {
            Id = product.Id,
            ItemCode = product.SKU,
            ItemName = product.Name,
            Description = product.Description,
            ItemType = "Product",
            SKU = product.SKU,
            Barcode = product.Barcode,
            Category = product.Category?.Name,
            UnitCost = product.StandardCost,
            UnitPrice = product.RetailPrice,
            QuantityOnHand = totalQuantity,
            QuantityAvailable = totalQuantity - totalReserved,
            QuantityReserved = totalReserved,
            ReorderLevel = product.ReorderPoint,
            ReorderQuantity = product.ReorderQuantity,
            UnitOfMeasure = product.UnitOfMeasure,
            IsActive = product.IsActive,
            IsSerialized = false,
            IsBatchTracked = product.RequiresBatchTracking,
            IsCannabisProduct = product.IsCannabisProduct,
            THCPercentage = product.THCPercentage,
            CBDPercentage = product.CBDPercentage,
            StrainName = product.StrainType,
            ProductForm = null,
            WarehouseId = product.DefaultWarehouseId,
            WarehouseName = product.DefaultWarehouse?.Name,
            DefaultBinLocation = null,
            InventoryAccountId = product.InventoryAssetAccountId,
            COGSAccountId = product.COGSAccountId,
            SalesAccountId = product.RevenueAccountId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CreatedBy = null,
            Notes = null
        };
    }
}

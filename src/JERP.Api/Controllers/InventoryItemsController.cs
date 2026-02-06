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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Inventory;
using JERP.Application.DTOs.Inventory;

namespace JERP.Api.Controllers;

/// <summary>
/// Inventory items management endpoints
/// </summary>
[Route("api/v1/inventory/items")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class InventoryItemsController : ControllerBase
{
    private readonly IInventoryItemService _inventoryItemService;
    private readonly ILogger<InventoryItemsController> _logger;

    public InventoryItemsController(
        IInventoryItemService inventoryItemService,
        ILogger<InventoryItemsController> logger)
    {
        _inventoryItemService = inventoryItemService;
        _logger = logger;
    }

    /// <summary>
    /// Get all inventory items for a company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid companyId)
    {
        var items = await _inventoryItemService.GetAllAsync(companyId);
        return Ok(items);
    }

    /// <summary>
    /// Get inventory item by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InventoryItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _inventoryItemService.GetByIdAsync(id);
        
        if (item == null)
        {
            return NotFound(new { success = false, error = $"Inventory item with ID {id} not found" });
        }

        return Ok(item);
    }

    /// <summary>
    /// Search inventory items by search term
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] Guid companyId, [FromQuery] string searchTerm)
    {
        var items = await _inventoryItemService.SearchAsync(companyId, searchTerm);
        return Ok(items);
    }

    /// <summary>
    /// Get inventory items by type
    /// </summary>
    [HttpGet("by-type/{itemType}")]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType([FromQuery] Guid companyId, string itemType)
    {
        var items = await _inventoryItemService.GetByTypeAsync(companyId, itemType);
        return Ok(items);
    }

    /// <summary>
    /// Get low stock inventory items
    /// </summary>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLowStock([FromQuery] Guid companyId)
    {
        var items = await _inventoryItemService.GetLowStockItemsAsync(companyId);
        return Ok(items);
    }

    /// <summary>
    /// Get cannabis product inventory items
    /// </summary>
    [HttpGet("cannabis")]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCannabisProducts([FromQuery] Guid companyId)
    {
        var items = await _inventoryItemService.GetCannabisProductsAsync(companyId);
        return Ok(items);
    }

    /// <summary>
    /// Get available quantity for an inventory item
    /// </summary>
    [HttpGet("{id}/available-quantity")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableQuantity(Guid id)
    {
        var quantity = await _inventoryItemService.GetAvailableQuantityAsync(id);
        return Ok(new { availableQuantity = quantity });
    }

    /// <summary>
    /// Create a new inventory item
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InventoryItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromQuery] Guid companyId, [FromBody] CreateInventoryItemRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var item = await _inventoryItemService.CreateAsync(companyId, request, userId);
            
            _logger.LogInformation("Created inventory item {ItemName} for company {CompanyId}", 
                item.ItemName, companyId);
            
            return StatusCode(201, new { success = true, data = item });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing inventory item
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(InventoryItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInventoryItemRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var item = await _inventoryItemService.UpdateAsync(id, request, userId);
            
            if (item == null)
            {
                return NotFound(new { success = false, error = $"Inventory item with ID {id} not found" });
            }
            
            _logger.LogInformation("Updated inventory item {ItemId}", id);
            
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item {ItemId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an inventory item
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _inventoryItemService.DeleteAsync(id);
        
        if (!result)
        {
            return NotFound(new { success = false, error = $"Inventory item with ID {id} not found" });
        }
        
        _logger.LogInformation("Deleted inventory item {ItemId}", id);
        
        return NoContent();
    }
}

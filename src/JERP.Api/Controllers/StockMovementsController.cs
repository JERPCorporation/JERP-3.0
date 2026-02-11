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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.Services.Inventory;
using JERP.Application.DTOs.Inventory;

namespace JERP.Api.Controllers;

/// <summary>
/// Stock movement endpoints for receipt, issue, transfer, and return operations.
/// 
/// FIX: Changed from ControllerBase to BaseApiController for consistent response format.
/// Also removed try-catch blocks - ExceptionHandlingMiddleware handles exceptions globally.
/// </summary>
[Route("api/v1/inventory/movements")]
[Authorize]
public class StockMovementsController : BaseApiController
{
    private readonly IStockMovementService _stockMovementService;
    private readonly ILogger<StockMovementsController> _logger;

    public StockMovementsController(
        IStockMovementService stockMovementService,
        ILogger<StockMovementsController> logger)
    {
        _stockMovementService = stockMovementService;
        _logger = logger;
    }

    /// <summary>
    /// Get stock movement by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movement = await _stockMovementService.GetByIdAsync(id);
        
        if (movement == null)
            return NotFound($"Stock movement with ID {id} not found");

        return Ok(movement);
    }

    /// <summary>
    /// Get stock movements for an inventory item
    /// </summary>
    [HttpGet("item/{itemId}")]
    [ProducesResponseType(typeof(IEnumerable<StockMovementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByItemId(Guid itemId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var movements = await _stockMovementService.GetByItemIdAsync(itemId, startDate, endDate);
        return Ok(movements);
    }

    /// <summary>
    /// Get stock movements for a warehouse
    /// </summary>
    [HttpGet("warehouse/{warehouseId}")]
    [ProducesResponseType(typeof(IEnumerable<StockMovementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByWarehouseId(Guid warehouseId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var movements = await _stockMovementService.GetByWarehouseIdAsync(warehouseId, startDate, endDate);
        return Ok(movements);
    }

    /// <summary>
    /// Create a stock receipt
    /// </summary>
    [HttpPost("receipt")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReceipt([FromBody] CreateStockMovementRequest request)
    {
        var userId = GetCurrentUserId() ?? "system";
        var movement = await _stockMovementService.CreateReceiptAsync(request, userId);
        
        _logger.LogInformation("Created stock receipt for item {ItemId}, quantity {Quantity}", 
            request.InventoryItemId, request.Quantity);
        
        return Created(movement);
    }

    /// <summary>
    /// Create a stock issue
    /// </summary>
    [HttpPost("issue")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIssue([FromBody] CreateStockMovementRequest request)
    {
        var userId = GetCurrentUserId() ?? "system";
        var movement = await _stockMovementService.CreateIssueAsync(request, userId);
        
        _logger.LogInformation("Created stock issue for item {ItemId}, quantity {Quantity}", 
            request.InventoryItemId, request.Quantity);
        
        return Created(movement);
    }

    /// <summary>
    /// Create a stock transfer
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransfer([FromBody] CreateStockMovementRequest request)
    {
        var userId = GetCurrentUserId() ?? "system";
        var movement = await _stockMovementService.CreateTransferAsync(request, userId);
        
        _logger.LogInformation("Created stock transfer for item {ItemId}, quantity {Quantity}", 
            request.InventoryItemId, request.Quantity);
        
        return Created(movement);
    }

    /// <summary>
    /// Create a stock return
    /// </summary>
    [HttpPost("return")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReturn([FromBody] CreateStockMovementRequest request)
    {
        var userId = GetCurrentUserId() ?? "system";
        var movement = await _stockMovementService.CreateReturnAsync(request, userId);
        
        _logger.LogInformation("Created stock return for item {ItemId}, quantity {Quantity}", 
            request.InventoryItemId, request.Quantity);
        
        return Created(movement);
    }

    /// <summary>
    /// Reverse a stock movement
    /// </summary>
    [HttpPost("{id}/reverse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReverseMovement(Guid id, [FromBody] ReversalRequest request)
    {
        var userId = GetCurrentUserId() ?? "system";
        var result = await _stockMovementService.ReverseMovementAsync(id, request.Reason, userId);
        
        if (!result)
            return NotFound($"Stock movement with ID {id} not found");
        
        _logger.LogInformation("Reversed stock movement {MovementId} - Reason: {Reason}", id, request.Reason);
        return Ok(new { success = true, message = "Stock movement reversed successfully" });
    }
}

public class ReversalRequest
{
    public string Reason { get; set; } = string.Empty;
}

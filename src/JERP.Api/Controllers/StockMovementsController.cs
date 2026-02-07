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
/// Stock movement endpoints for receipt, issue, transfer, and return operations
/// </summary>
[Route("api/v1/inventory/movements")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class StockMovementsController : ControllerBase
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
        {
            return NotFound(new { success = false, error = $"Stock movement with ID {id} not found" });
        }

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
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var movement = await _stockMovementService.CreateReceiptAsync(request, userId);
            
            _logger.LogInformation("Created stock receipt for item {ItemId}, quantity {Quantity}", 
                request.InventoryItemId, request.Quantity);
            
            return StatusCode(201, new { success = true, data = movement });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock receipt");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Create a stock issue
    /// </summary>
    [HttpPost("issue")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIssue([FromBody] CreateStockMovementRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var movement = await _stockMovementService.CreateIssueAsync(request, userId);
            
            _logger.LogInformation("Created stock issue for item {ItemId}, quantity {Quantity}", 
                request.InventoryItemId, request.Quantity);
            
            return StatusCode(201, new { success = true, data = movement });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock issue");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Create a stock transfer
    /// </summary>
    [HttpPost("transfer")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransfer([FromBody] CreateStockMovementRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var movement = await _stockMovementService.CreateTransferAsync(request, userId);
            
            _logger.LogInformation("Created stock transfer for item {ItemId}, quantity {Quantity}", 
                request.InventoryItemId, request.Quantity);
            
            return StatusCode(201, new { success = true, data = movement });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock transfer");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Create a stock return
    /// </summary>
    [HttpPost("return")]
    [ProducesResponseType(typeof(StockMovementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReturn([FromBody] CreateStockMovementRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var movement = await _stockMovementService.CreateReturnAsync(request, userId);
            
            _logger.LogInformation("Created stock return for item {ItemId}, quantity {Quantity}", 
                request.InventoryItemId, request.Quantity);
            
            return StatusCode(201, new { success = true, data = movement });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock return");
            return BadRequest(new { success = false, error = ex.Message });
        }
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
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var result = await _stockMovementService.ReverseMovementAsync(id, request.Reason, userId);
            
            if (!result)
            {
                return NotFound(new { success = false, error = $"Stock movement with ID {id} not found" });
            }
            
            _logger.LogInformation("Reversed stock movement {MovementId} - Reason: {Reason}", id, request.Reason);
            
            return Ok(new { success = true, message = "Stock movement reversed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reversing stock movement {MovementId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}

public class ReversalRequest
{
    public string Reason { get; set; } = string.Empty;
}

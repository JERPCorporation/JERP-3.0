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
/// Stock adjustment endpoints with approval workflow and ledger posting
/// </summary>
[Route("api/v1/inventory/adjustments")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class StockAdjustmentsController : ControllerBase
{
    private readonly IStockAdjustmentService _stockAdjustmentService;
    private readonly ILogger<StockAdjustmentsController> _logger;

    public StockAdjustmentsController(
        IStockAdjustmentService stockAdjustmentService,
        ILogger<StockAdjustmentsController> logger)
    {
        _stockAdjustmentService = stockAdjustmentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all stock adjustments for a company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StockAdjustmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid companyId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var adjustments = await _stockAdjustmentService.GetAllAsync(companyId, startDate, endDate);
        return Ok(adjustments);
    }

    /// <summary>
    /// Get stock adjustment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(StockAdjustmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var adjustment = await _stockAdjustmentService.GetByIdAsync(id);
        
        if (adjustment == null)
        {
            return NotFound(new { success = false, error = $"Stock adjustment with ID {id} not found" });
        }

        return Ok(adjustment);
    }

    /// <summary>
    /// Get stock adjustments for an inventory item
    /// </summary>
    [HttpGet("item/{itemId}")]
    [ProducesResponseType(typeof(IEnumerable<StockAdjustmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByItemId(Guid itemId)
    {
        var adjustments = await _stockAdjustmentService.GetByItemIdAsync(itemId);
        return Ok(adjustments);
    }

    /// <summary>
    /// Create a new stock adjustment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(StockAdjustmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromQuery] Guid companyId, [FromBody] CreateStockAdjustmentRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var adjustment = await _stockAdjustmentService.CreateAsync(companyId, request, userId);
            
            _logger.LogInformation("Created stock adjustment for item {ItemId}, adjustment quantity {Quantity}", 
                request.InventoryItemId, request.AdjustmentQuantity);
            
            return StatusCode(201, new { success = true, data = adjustment });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock adjustment");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Approve a stock adjustment
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(StockAdjustmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var adjustment = await _stockAdjustmentService.ApproveAsync(id, userId);
            
            if (adjustment == null)
            {
                return NotFound(new { success = false, error = $"Stock adjustment with ID {id} not found" });
            }
            
            _logger.LogInformation("Approved stock adjustment {AdjustmentId}", id);
            
            return Ok(adjustment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving stock adjustment {AdjustmentId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Post a stock adjustment to ledger
    /// </summary>
    [HttpPost("{id}/post")]
    [ProducesResponseType(typeof(StockAdjustmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var adjustment = await _stockAdjustmentService.PostAsync(id, userId);
            
            if (adjustment == null)
            {
                return NotFound(new { success = false, error = $"Stock adjustment with ID {id} not found" });
            }
            
            _logger.LogInformation("Posted stock adjustment {AdjustmentId} to ledger", id);
            
            return Ok(adjustment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error posting stock adjustment {AdjustmentId} to ledger", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Reject a stock adjustment
    /// </summary>
    [HttpPost("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectionRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var result = await _stockAdjustmentService.RejectAsync(id, request.Reason, userId);
            
            if (!result)
            {
                return NotFound(new { success = false, error = $"Stock adjustment with ID {id} not found" });
            }
            
            _logger.LogInformation("Rejected stock adjustment {AdjustmentId} - Reason: {Reason}", id, request.Reason);
            
            return Ok(new { success = true, message = "Stock adjustment rejected successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting stock adjustment {AdjustmentId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}

public class RejectionRequest
{
    public string Reason { get; set; } = string.Empty;
}

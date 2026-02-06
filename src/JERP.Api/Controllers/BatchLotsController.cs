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
/// Batch and lot management endpoints with quarantine controls
/// </summary>
[Route("api/v1/inventory/batches")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class BatchLotsController : ControllerBase
{
    private readonly IBatchLotService _batchLotService;
    private readonly ILogger<BatchLotsController> _logger;

    public BatchLotsController(
        IBatchLotService batchLotService,
        ILogger<BatchLotsController> logger)
    {
        _batchLotService = batchLotService;
        _logger = logger;
    }

    /// <summary>
    /// Get batch/lot by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BatchLotDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var batch = await _batchLotService.GetByIdAsync(id);
        
        if (batch == null)
        {
            return NotFound(new { success = false, error = $"Batch/lot with ID {id} not found" });
        }

        return Ok(batch);
    }

    /// <summary>
    /// Get all batches/lots for an inventory item
    /// </summary>
    [HttpGet("item/{itemId}")]
    [ProducesResponseType(typeof(IEnumerable<BatchLotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByItemId(Guid itemId)
    {
        var batches = await _batchLotService.GetByItemIdAsync(itemId);
        return Ok(batches);
    }

    /// <summary>
    /// Get expiring batches/lots within specified days
    /// </summary>
    [HttpGet("expiring")]
    [ProducesResponseType(typeof(IEnumerable<BatchLotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpiring([FromQuery] Guid companyId, [FromQuery] int daysAhead = 30)
    {
        var batches = await _batchLotService.GetExpiringBatchesAsync(companyId, daysAhead);
        return Ok(batches);
    }

    /// <summary>
    /// Get all quarantined batches/lots
    /// </summary>
    [HttpGet("quarantined")]
    [ProducesResponseType(typeof(IEnumerable<BatchLotDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuarantined([FromQuery] Guid companyId)
    {
        var batches = await _batchLotService.GetQuarantinedBatchesAsync(companyId);
        return Ok(batches);
    }

    /// <summary>
    /// Create a new batch/lot
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BatchLotDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBatchLotRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var batch = await _batchLotService.CreateAsync(request, userId);
            
            _logger.LogInformation("Created batch/lot {BatchNumber} for item {ItemId}", 
                batch.BatchNumber, request.InventoryItemId);
            
            return StatusCode(201, new { success = true, data = batch });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating batch/lot");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing batch/lot
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BatchLotDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateBatchLotRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var batch = await _batchLotService.UpdateAsync(id, request, userId);
            
            if (batch == null)
            {
                return NotFound(new { success = false, error = $"Batch/lot with ID {id} not found" });
            }
            
            _logger.LogInformation("Updated batch/lot {BatchId}", id);
            
            return Ok(batch);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating batch/lot {BatchId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Quarantine a batch/lot
    /// </summary>
    [HttpPost("{id}/quarantine")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Quarantine(Guid id, [FromBody] QuarantineRequest request)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var result = await _batchLotService.QuarantineAsync(id, request.Reason, userId);
            
            if (!result)
            {
                return NotFound(new { success = false, error = $"Batch/lot with ID {id} not found" });
            }
            
            _logger.LogInformation("Quarantined batch/lot {BatchId} - Reason: {Reason}", id, request.Reason);
            
            return Ok(new { success = true, message = "Batch/lot quarantined successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error quarantining batch/lot {BatchId}", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Release a batch/lot from quarantine
    /// </summary>
    [HttpPost("{id}/release")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReleaseFromQuarantine(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value ?? "system";
        
        try
        {
            var result = await _batchLotService.ReleaseFromQuarantineAsync(id, userId);
            
            if (!result)
            {
                return NotFound(new { success = false, error = $"Batch/lot with ID {id} not found" });
            }
            
            _logger.LogInformation("Released batch/lot {BatchId} from quarantine", id);
            
            return Ok(new { success = true, message = "Batch/lot released from quarantine successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing batch/lot {BatchId} from quarantine", id);
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a batch/lot
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _batchLotService.DeactivateAsync(id);
        
        if (!result)
        {
            return NotFound(new { success = false, error = $"Batch/lot with ID {id} not found" });
        }
        
        _logger.LogInformation("Deleted batch/lot {BatchId}", id);
        
        return NoContent();
    }
}

public class QuarantineRequest
{
    public string Reason { get; set; } = string.Empty;
}

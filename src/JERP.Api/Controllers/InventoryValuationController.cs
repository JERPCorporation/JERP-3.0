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
/// Inventory valuation report endpoints
/// </summary>
[Route("api/v1/inventory/valuation")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class InventoryValuationController : ControllerBase
{
    private readonly IInventoryValuationService _valuationService;
    private readonly ILogger<InventoryValuationController> _logger;

    public InventoryValuationController(
        IInventoryValuationService valuationService,
        ILogger<InventoryValuationController> logger)
    {
        _valuationService = valuationService;
        _logger = logger;
    }

    /// <summary>
    /// Get inventory valuation for a company
    /// </summary>
    [HttpGet("company/{companyId}")]
    [ProducesResponseType(typeof(IEnumerable<InventoryValuationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCompany(Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        var valuations = await _valuationService.GetValuationByCompanyAsync(companyId, asOfDate);
        return Ok(valuations);
    }

    /// <summary>
    /// Get inventory valuation for a warehouse
    /// </summary>
    [HttpGet("warehouse/{warehouseId}")]
    [ProducesResponseType(typeof(IEnumerable<InventoryValuationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByWarehouse(Guid warehouseId, [FromQuery] DateTime? asOfDate = null)
    {
        var valuations = await _valuationService.GetValuationByWarehouseAsync(warehouseId, asOfDate);
        return Ok(valuations);
    }

    /// <summary>
    /// Get inventory valuation for a specific item
    /// </summary>
    [HttpGet("item/{itemId}")]
    [ProducesResponseType(typeof(InventoryValuationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByItem(Guid itemId, [FromQuery] DateTime? asOfDate = null)
    {
        var valuation = await _valuationService.GetValuationByItemAsync(itemId, asOfDate);
        
        if (valuation == null)
        {
            return NotFound(new { success = false, error = $"Valuation for item with ID {itemId} not found" });
        }

        return Ok(valuation);
    }

    /// <summary>
    /// Get total inventory value for a company
    /// </summary>
    [HttpGet("company/{companyId}/total")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalValue(Guid companyId, [FromQuery] DateTime? asOfDate = null)
    {
        var totalValue = await _valuationService.GetTotalInventoryValueAsync(companyId, asOfDate);
        return Ok(new { companyId, totalValue, asOfDate = asOfDate ?? DateTime.UtcNow });
    }
}

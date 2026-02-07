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
using JERP.Application.DTOs.SalesOrders;
using JERP.Application.Services.SalesOrders;

namespace JERP.Api.Controllers;

/// <summary>
/// Sales order shipment management endpoints
/// </summary>
[Route("api/v1/sales-orders/shipments")]
[Authorize]
public class SOShipmentsController : BaseApiController
{
    private readonly ISOShipmentService _shipmentService;
    private readonly ILogger<SOShipmentsController> _logger;

    public SOShipmentsController(
        ISOShipmentService shipmentService,
        ILogger<SOShipmentsController> logger)
    {
        _shipmentService = shipmentService;
        _logger = logger;
    }

    /// <summary>
    /// Get a shipment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SOShipmentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment == null)
            {
                return NotFound($"Shipment {id} not found");
            }

            return Ok(shipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shipment {Id}", id);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Get shipments by sales order
    /// </summary>
    [HttpGet("sales-order/{salesOrderId}")]
    [ProducesResponseType(typeof(List<SOShipmentDto>), 200)]
    public async Task<IActionResult> GetBySalesOrder(Guid salesOrderId)
    {
        try
        {
            var shipments = await _shipmentService.GetBySalesOrderAsync(salesOrderId);
            return Ok(shipments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shipments for sales order {SalesOrderId}", salesOrderId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// List all shipments for a company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SOShipmentDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] Guid companyId)
    {
        try
        {
            var shipments = await _shipmentService.GetAllAsync(companyId);
            return Ok(shipments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing shipments for company {CompanyId}", companyId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Create a new shipment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SOShipmentDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreateSOShipmentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shipment = await _shipmentService.CreateAsync(companyId, request);
            return Created(shipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating shipment");
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Mark a shipment as shipped
    /// </summary>
    [HttpPost("{id}/ship")]
    [ProducesResponseType(typeof(SOShipmentDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Ship(Guid id)
    {
        try
        {
            var username = GetCurrentUsername() ?? "system";
            var shipment = await _shipmentService.ShipAsync(id, username);
            return Ok(shipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error shipping shipment {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Cancel a shipment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            await _shipmentService.CancelAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling shipment {Id}", id);
            return Error(ex.Message, 400);
        }
    }
}

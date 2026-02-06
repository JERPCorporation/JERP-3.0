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
using JERP.Application.DTOs.SalesOrders;
using JERP.Application.Services.SalesOrders;

namespace JERP.Api.Controllers;

/// <summary>
/// Sales return (RMA) management endpoints
/// </summary>
[Route("api/v1/sales-orders/returns")]
[Authorize]
public class SalesReturnsController : BaseApiController
{
    private readonly ISalesReturnService _returnService;
    private readonly ILogger<SalesReturnsController> _logger;

    public SalesReturnsController(
        ISalesReturnService returnService,
        ILogger<SalesReturnsController> logger)
    {
        _returnService = returnService;
        _logger = logger;
    }

    /// <summary>
    /// Get a sales return by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SalesReturnDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var salesReturn = await _returnService.GetByIdAsync(id);
            if (salesReturn == null)
            {
                return NotFound($"Sales return {id} not found");
            }

            return Ok(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales return {Id}", id);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// List all sales returns for a company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SalesReturnDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] Guid companyId)
    {
        try
        {
            var returns = await _returnService.GetAllAsync(companyId);
            return Ok(returns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing sales returns for company {CompanyId}", companyId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Get sales returns by customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(List<SalesReturnDto>), 200)]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        try
        {
            var returns = await _returnService.GetByCustomerAsync(customerId);
            return Ok(returns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales returns for customer {CustomerId}", customerId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Request a new sales return (RMA)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SalesReturnDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RequestReturn(
        [FromQuery] Guid companyId,
        [FromQuery] Guid customerId,
        [FromQuery] Guid? salesOrderId,
        [FromBody] string reason)
    {
        try
        {
            var salesReturn = await _returnService.RequestAsync(companyId, customerId, salesOrderId, reason);
            return Created(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sales return");
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Approve a sales return request
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(SalesReturnDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            var username = GetCurrentUsername() ?? "system";
            var salesReturn = await _returnService.ApproveAsync(id, username);
            return Ok(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving sales return {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Receive returned items
    /// </summary>
    [HttpPost("{id}/receive")]
    [ProducesResponseType(typeof(SalesReturnDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Receive(Guid id)
    {
        try
        {
            var username = GetCurrentUsername() ?? "system";
            var salesReturn = await _returnService.ReceiveAsync(id, username);
            return Ok(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving sales return {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Process refund for a sales return
    /// </summary>
    [HttpPost("{id}/refund")]
    [ProducesResponseType(typeof(SalesReturnDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Refund(Guid id)
    {
        try
        {
            var salesReturn = await _returnService.RefundAsync(id);
            return Ok(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for sales return {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Close a sales return
    /// </summary>
    [HttpPost("{id}/close")]
    [ProducesResponseType(typeof(SalesReturnDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Close(Guid id)
    {
        try
        {
            var salesReturn = await _returnService.CloseAsync(id);
            return Ok(salesReturn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing sales return {Id}", id);
            return Error(ex.Message, 400);
        }
    }
}

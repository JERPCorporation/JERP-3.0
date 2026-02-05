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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.SalesOrders;
using JERP.Application.Services.SalesOrders;
using JERP.Core.Enums;

namespace JERP.Api.Controllers;

/// <summary>
/// Sales order management endpoints
/// </summary>
[Route("api/v1/sales-orders")]
[Authorize]
public class SalesOrdersController : BaseApiController
{
    private readonly ISalesOrderService _salesOrderService;
    private readonly ILogger<SalesOrdersController> _logger;

    public SalesOrdersController(
        ISalesOrderService salesOrderService,
        ILogger<SalesOrdersController> logger)
    {
        _salesOrderService = salesOrderService;
        _logger = logger;
    }

    /// <summary>
    /// Get a sales order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(id);
            if (salesOrder == null)
            {
                return NotFound($"Sales order {id} not found");
            }

            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales order {Id}", id);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// List all sales orders for a company
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<SalesOrderDto>), 200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid companyId,
        [FromQuery] SalesOrderStatus? status = null)
    {
        try
        {
            var salesOrders = await _salesOrderService.GetAllAsync(companyId, status);
            return Ok(salesOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing sales orders for company {CompanyId}", companyId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Get sales orders by customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(List<SalesOrderDto>), 200)]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        try
        {
            var salesOrders = await _salesOrderService.GetByCustomerAsync(customerId);
            return Ok(salesOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales orders for customer {CustomerId}", customerId);
            return Error(ex.Message, 500);
        }
    }

    /// <summary>
    /// Create a new sales order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SalesOrderDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreateSalesOrderRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesOrder = await _salesOrderService.CreateAsync(companyId, request);
            return Created(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sales order");
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update a sales order (Draft only)
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] CreateSalesOrderRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesOrder = await _salesOrderService.UpdateAsync(id, request);
            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sales order {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Submit a sales order for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Submit(Guid id)
    {
        try
        {
            var salesOrder = await _salesOrderService.SubmitAsync(id);
            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting sales order {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Approve a sales order
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            var username = GetCurrentUsername() ?? "system";
            var salesOrder = await _salesOrderService.ApproveAsync(id, username);
            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving sales order {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Cancel a sales order
    /// </summary>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        try
        {
            var salesOrder = await _salesOrderService.CancelAsync(id, reason);
            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling sales order {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Close a sales order
    /// </summary>
    [HttpPost("{id}/close")]
    [ProducesResponseType(typeof(SalesOrderDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Close(Guid id)
    {
        try
        {
            var salesOrder = await _salesOrderService.CloseAsync(id);
            return Ok(salesOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing sales order {Id}", id);
            return Error(ex.Message, 400);
        }
    }

    /// <summary>
    /// Check credit limit for a customer
    /// </summary>
    [HttpGet("credit-check")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CheckCredit(
        [FromQuery] Guid customerId,
        [FromQuery] decimal orderAmount)
    {
        try
        {
            var hasCredit = await _salesOrderService.CheckCreditLimitAsync(customerId, orderAmount);
            return Ok(new { hasCredit, customerId, orderAmount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking credit for customer {CustomerId}", customerId);
            return Error(ex.Message, 500);
        }
    }
}

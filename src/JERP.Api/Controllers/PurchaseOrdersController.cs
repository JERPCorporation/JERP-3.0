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

using JERP.Application.DTOs.PurchaseOrders;
using JERP.Application.Services.PurchaseOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Purchase order management endpoints
/// </summary>
[Route("api/v1/purchase-orders")]
[Authorize]
public class PurchaseOrdersController : BaseApiController
{
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly ILogger<PurchaseOrdersController> _logger;

    public PurchaseOrdersController(
        IPurchaseOrderService purchaseOrderService,
        ILogger<PurchaseOrdersController> logger)
    {
        _purchaseOrderService = purchaseOrderService;
        _logger = logger;
    }

    /// <summary>
    /// Get all purchase orders with optional status filter
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid companyId,
        [FromQuery] string? status = null)
    {
        var purchaseOrders = await _purchaseOrderService.GetAllAsync(companyId, status);
        return Ok(purchaseOrders);
    }

    /// <summary>
    /// Get purchase order by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchaseOrder = await _purchaseOrderService.GetByIdAsync(id);
        
        if (purchaseOrder == null)
        {
            return NotFound($"Purchase order with ID {id} not found");
        }

        return Ok(purchaseOrder);
    }

    /// <summary>
    /// Create a new purchase order
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreatePurchaseOrderRequest request)
    {
        var userId = GetCurrentUserId();
        var purchaseOrder = await _purchaseOrderService.CreateAsync(companyId, request, userId);
        
        _logger.LogInformation("Purchase order created: {PONumber}", purchaseOrder.PONumber);
        return Created(purchaseOrder);
    }

    /// <summary>
    /// Update an existing purchase order
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseOrderRequest request)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.UpdateAsync(id, request);
            _logger.LogInformation("Purchase order updated: {PONumber}", purchaseOrder.PONumber);
            return Ok(purchaseOrder);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Approve a purchase order
    /// </summary>
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var userId = GetCurrentUserId();
        var purchaseOrder = await _purchaseOrderService.ApproveAsync(id, userId);
        
        _logger.LogInformation("Purchase order approved: {PONumber}", purchaseOrder.PONumber);
        return Ok(purchaseOrder);
    }

    /// <summary>
    /// Cancel a purchase order
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            var purchaseOrder = await _purchaseOrderService.CancelAsync(id);
            _logger.LogInformation("Purchase order cancelled: {PONumber}", purchaseOrder.PONumber);
            return Ok(purchaseOrder);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Close a purchase order
    /// </summary>
    [HttpPost("{id}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var purchaseOrder = await _purchaseOrderService.CloseAsync(id);
        _logger.LogInformation("Purchase order closed: {PONumber}", purchaseOrder.PONumber);
        return Ok(purchaseOrder);
    }
}

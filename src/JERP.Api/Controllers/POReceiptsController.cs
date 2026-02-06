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
/// Purchase order receipt management endpoints
/// </summary>
[Route("api/v1/purchase-orders/receipts")]
[Authorize]
public class POReceiptsController : BaseApiController
{
    private readonly IPOReceiptService _receiptService;
    private readonly ILogger<POReceiptsController> _logger;

    public POReceiptsController(
        IPOReceiptService receiptService,
        ILogger<POReceiptsController> logger)
    {
        _receiptService = receiptService;
        _logger = logger;
    }

    /// <summary>
    /// Get receipt by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var receipt = await _receiptService.GetByIdAsync(id);
        
        if (receipt == null)
        {
            return NotFound($"Receipt with ID {id} not found");
        }

        return Ok(receipt);
    }

    /// <summary>
    /// Get all receipts for a purchase order
    /// </summary>
    [HttpGet("purchase-order/{purchaseOrderId}")]
    public async Task<IActionResult> GetByPurchaseOrder(Guid purchaseOrderId)
    {
        var receipts = await _receiptService.GetByPurchaseOrderAsync(purchaseOrderId);
        return Ok(receipts);
    }

    /// <summary>
    /// Create a new receipt
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreatePOReceiptRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var receipt = await _receiptService.CreateAsync(companyId, request, userId);
            
            _logger.LogInformation("Receipt created: {ReceiptNumber}", receipt.ReceiptNumber);
            return Created(receipt);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

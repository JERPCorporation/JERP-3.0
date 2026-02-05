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

using JERP.Application.DTOs.PurchaseOrders;
using JERP.Application.Services.PurchaseOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Vendor bill management endpoints
/// </summary>
[Route("api/v1/vendors/bills")]
[Authorize]
public class VendorBillsController : BaseApiController
{
    private readonly IVendorBillService _billService;
    private readonly ILogger<VendorBillsController> _logger;

    public VendorBillsController(
        IVendorBillService billService,
        ILogger<VendorBillsController> logger)
    {
        _billService = billService;
        _logger = logger;
    }

    /// <summary>
    /// Get bill by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var bill = await _billService.GetByIdAsync(id);
        
        if (bill == null)
        {
            return NotFound($"Bill with ID {id} not found");
        }

        return Ok(bill);
    }

    /// <summary>
    /// Get all bills for a vendor
    /// </summary>
    [HttpGet("vendor/{vendorId}")]
    public async Task<IActionResult> GetByVendor(Guid vendorId)
    {
        var bills = await _billService.GetByVendorAsync(vendorId);
        return Ok(bills);
    }

    /// <summary>
    /// Create a new vendor bill
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreateVendorBillRequest request)
    {
        try
        {
            var bill = await _billService.CreateAsync(companyId, request);
            _logger.LogInformation("Vendor bill created: {BillNumber}", bill.BillNumber);
            return Created(bill);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

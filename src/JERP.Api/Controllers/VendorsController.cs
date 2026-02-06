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

using JERP.Application.DTOs.Finance;
using JERP.Application.Services.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

/// <summary>
/// Vendor management endpoints
/// </summary>
[Route("api/v1/vendors")]
[Authorize]
public class VendorsController : BaseApiController
{
    private readonly IVendorService _vendorService;
    private readonly ILogger<VendorsController> _logger;

    public VendorsController(
        IVendorService vendorService,
        ILogger<VendorsController> logger)
    {
        _vendorService = vendorService;
        _logger = logger;
    }

    /// <summary>
    /// Get all vendors with optional active filter
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid companyId,
        [FromQuery] bool? isActive = null)
    {
        var vendors = await _vendorService.GetAllAsync(companyId, isActive);
        return Ok(vendors);
    }

    /// <summary>
    /// Get vendor by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vendor = await _vendorService.GetByIdAsync(id);
        
        if (vendor == null)
        {
            return NotFound($"Vendor with ID {id} not found");
        }

        return Ok(vendor);
    }

    /// <summary>
    /// Create a new vendor
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] Guid companyId,
        [FromBody] CreateVendorDto request)
    {
        var vendor = await _vendorService.CreateAsync(companyId, request);
        _logger.LogInformation("Vendor created: {VendorNumber}", vendor.VendorNumber);
        return Created(vendor);
    }

    /// <summary>
    /// Update an existing vendor
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVendorDto request)
    {
        try
        {
            var vendor = await _vendorService.UpdateAsync(id, request);
            _logger.LogInformation("Vendor updated: {VendorNumber}", vendor.VendorNumber);
            return Ok(vendor);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete (deactivate) a vendor
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _vendorService.DeleteAsync(id);
            _logger.LogInformation("Vendor deleted: {VendorId}", id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get vendor balance
    /// </summary>
    [HttpGet("{id}/balance")]
    public async Task<IActionResult> GetBalance(Guid id)
    {
        try
        {
            var balance = await _vendorService.GetVendorBalanceAsync(id);
            return Ok(new { vendorId = id, balance });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

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
using JERP.Application.DTOs.Finance;
using JERP.Application.Services.Finance;
using JERP.Core.Enums;

namespace JERP.Api.Controllers;

/// <summary>
/// Journal entry management endpoints.
/// 
/// WHAT CHANGED AND WHY:
/// 
/// BEFORE: This controller injected JerpDbContext directly and contained ~300 lines of
/// business logic (balance validation, number generation, account balance updates, DTO mapping).
/// 
/// AFTER: This controller injects IJournalEntryService and is ~80 lines. It only handles:
///   - HTTP routing (which URL maps to which method)
///   - HTTP responses (200, 201, 404 status codes)
///   - Authorization (who can access what)
/// 
/// All business logic now lives in JournalEntryService where it can be:
///   - Reused by other services (payroll creates journal entries too)
///   - Unit tested without spinning up an HTTP server
///   - Changed without touching the controller
/// 
/// ALSO FIXED:
///   - Added [Authorize(Roles = "Accountant,Admin")] on post/void (was accessible to all users)
///   - Removed try-catch blocks (ExceptionHandlingMiddleware handles all exceptions globally)
///   - The service throws NotFoundException/BusinessRuleViolationException which the middleware
///     automatically maps to 404/400 HTTP responses
/// </summary>
[Route("api/v1/finance/journal-entries")]
[Authorize]
public class JournalEntriesController : BaseApiController
{
    private readonly IJournalEntryService _journalEntryService;
    private readonly ILogger<JournalEntriesController> _logger;

    public JournalEntriesController(
        IJournalEntryService journalEntryService,
        ILogger<JournalEntriesController> logger)
    {
        _journalEntryService = journalEntryService;
        _logger = logger;
    }

    /// <summary>
    /// List journal entries with optional filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<JournalEntryDto>), 200)]
    public async Task<IActionResult> GetJournalEntries(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] JournalEntryStatus? status = null)
    {
        var entries = await _journalEntryService.GetAllAsync(companyId, startDate, endDate, status);
        return Ok(entries);
    }

    /// <summary>
    /// Get journal entry details by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JournalEntryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetJournalEntry(Guid id)
    {
        var entry = await _journalEntryService.GetByIdAsync(id);

        if (entry == null)
            return NotFound($"Journal entry with ID {id} not found");

        return Ok(entry);
    }

    /// <summary>
    /// Create a manual journal entry
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(JournalEntryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateJournalEntry([FromBody] CreateJournalEntryRequest request)
    {
        // Service validates balance, accounts, and generates the number
        var entry = await _journalEntryService.CreateAsync(request);

        _logger.LogInformation("Created journal entry {Number} for company {CompanyId}",
            entry.JournalEntryNumber, entry.CompanyId);

        return Created(entry);
    }

    /// <summary>
    /// Post a draft journal entry (updates account balances)
    /// </summary>
    /// <remarks>
    /// Restricted to Accountant and Admin roles because posting affects financial balances.
    /// A regular user should not be able to post journal entries to the general ledger.
    /// </remarks>
    [HttpPost("{id}/post")]
    [Authorize(Roles = "Accountant,Admin")]
    [ProducesResponseType(typeof(JournalEntryDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PostJournalEntry(Guid id)
    {
        var entry = await _journalEntryService.PostAsync(id);

        _logger.LogInformation("Posted journal entry {Number}", entry.JournalEntryNumber);

        return Ok(entry);
    }

    /// <summary>
    /// Void a journal entry (reverses account balances if posted)
    /// </summary>
    /// <remarks>
    /// Restricted to Accountant and Admin roles because voiding reverses financial balances.
    /// </remarks>
    [HttpPost("{id}/void")]
    [Authorize(Roles = "Accountant,Admin")]
    [ProducesResponseType(typeof(JournalEntryDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> VoidJournalEntry(Guid id)
    {
        var entry = await _journalEntryService.VoidAsync(id);

        _logger.LogInformation("Voided journal entry {Number}", entry.JournalEntryNumber);

        return Ok(entry);
    }
}

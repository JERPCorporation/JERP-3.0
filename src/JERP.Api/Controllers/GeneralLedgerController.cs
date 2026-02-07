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
using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Finance;
using JERP.Infrastructure.Data;

namespace JERP.Api.Controllers;

/// <summary>
/// General ledger query endpoints
/// </summary>
[Route("api/v1/finance/general-ledger")]
[Authorize]
public class GeneralLedgerController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<GeneralLedgerController> _logger;

    public GeneralLedgerController(
        JerpDbContext context,
        ILogger<GeneralLedgerController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get general ledger entries with filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetGeneralLedgerEntries(
        [FromQuery] Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] Guid? accountId = null)
    {
        var query = _context.GeneralLedgerEntries
            .Include(e => e.Account)
            .Where(e => e.CompanyId == companyId);

        if (startDate.HasValue)
        {
            query = query.Where(e => e.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.TransactionDate <= endDate.Value);
        }

        if (accountId.HasValue)
        {
            query = query.Where(e => e.AccountId == accountId.Value);
        }

        var entries = await query
            .OrderByDescending(e => e.TransactionDate)
            .ThenBy(e => e.CreatedAt)
            .Select(e => new GeneralLedgerEntryDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                JournalEntryId = e.JournalEntryId,
                AccountId = e.AccountId,
                AccountNumber = e.Account.AccountNumber,
                AccountName = e.Account.AccountName,
                TransactionDate = e.TransactionDate,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                Description = e.Description,
                Source = e.Source,
                SourceEntityId = e.SourceEntityId,
                Is280EDeductible = e.Is280EDeductible,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();

        return Ok(entries);
    }

    /// <summary>
    /// Get account activity (ledger entries for a specific account)
    /// </summary>
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetAccountActivity(
        Guid accountId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null)
        {
            return NotFound($"Account with ID {accountId} not found");
        }

        var query = _context.GeneralLedgerEntries
            .Include(e => e.Account)
            .Where(e => e.AccountId == accountId);

        if (startDate.HasValue)
        {
            query = query.Where(e => e.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.TransactionDate <= endDate.Value);
        }

        var entries = await query
            .OrderByDescending(e => e.TransactionDate)
            .ThenBy(e => e.CreatedAt)
            .Select(e => new GeneralLedgerEntryDto
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                JournalEntryId = e.JournalEntryId,
                AccountId = e.AccountId,
                AccountNumber = e.Account.AccountNumber,
                AccountName = e.Account.AccountName,
                TransactionDate = e.TransactionDate,
                DebitAmount = e.DebitAmount,
                CreditAmount = e.CreditAmount,
                Description = e.Description,
                Source = e.Source,
                SourceEntityId = e.SourceEntityId,
                Is280EDeductible = e.Is280EDeductible,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            Account = new
            {
                account.Id,
                account.AccountNumber,
                account.AccountName,
                account.Type,
                account.Balance
            },
            Entries = entries
        });
    }
}

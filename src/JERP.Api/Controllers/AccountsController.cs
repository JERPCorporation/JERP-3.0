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
using Microsoft.EntityFrameworkCore;
using JERP.Application.DTOs.Finance;
using JERP.Core.Entities.Finance;
using JERP.Infrastructure.Data;

namespace JERP.Api.Controllers;

/// <summary>
/// Chart of accounts management endpoints
/// </summary>
[Route("api/v1/finance/accounts")]
[Authorize]
public class AccountsController : BaseApiController
{
    private readonly JerpDbContext _context;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(
        JerpDbContext context,
        ILogger<AccountsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get chart of accounts for a company
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] Guid companyId)
    {
        var accounts = await _context.Accounts
            .Where(a => a.CompanyId == companyId)
            .OrderBy(a => a.AccountNumber)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                CompanyId = a.CompanyId,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Type = a.Type,
                SubType = a.SubType,
                Balance = a.Balance,
                IsActive = a.IsActive,
                IsSystemAccount = a.IsSystemAccount,
                IsCOGS = a.IsCOGS,
                IsNonDeductible = a.IsNonDeductible,
                TaxCategory = a.TaxCategory,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToListAsync();

        return Ok(accounts);
    }

    /// <summary>
    /// Get account details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        var account = await _context.Accounts
            .Where(a => a.Id == id)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                CompanyId = a.CompanyId,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Type = a.Type,
                SubType = a.SubType,
                Balance = a.Balance,
                IsActive = a.IsActive,
                IsSystemAccount = a.IsSystemAccount,
                IsCOGS = a.IsCOGS,
                IsNonDeductible = a.IsNonDeductible,
                TaxCategory = a.TaxCategory,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound($"Account with ID {id} not found");
        }

        return Ok(account);
    }

    /// <summary>
    /// Create a new account
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        // Check if account number already exists
        var exists = await _context.Accounts
            .AnyAsync(a => a.CompanyId == request.CompanyId && a.AccountNumber == request.AccountNumber);

        if (exists)
        {
            return BadRequest($"Account number {request.AccountNumber} already exists");
        }

        var account = new Account
        {
            CompanyId = request.CompanyId,
            AccountNumber = request.AccountNumber,
            AccountName = request.AccountName,
            Type = request.Type,
            SubType = request.SubType,
            IsActive = true,
            IsSystemAccount = false,
            IsCOGS = request.IsCOGS,
            IsNonDeductible = request.IsNonDeductible,
            TaxCategory = request.TaxCategory
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created account {AccountNumber} - {AccountName} for company {CompanyId}",
            account.AccountNumber, account.AccountName, account.CompanyId);

        var dto = new AccountDto
        {
            Id = account.Id,
            CompanyId = account.CompanyId,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Type = account.Type,
            SubType = account.SubType,
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsSystemAccount = account.IsSystemAccount,
            IsCOGS = account.IsCOGS,
            IsNonDeductible = account.IsNonDeductible,
            TaxCategory = account.TaxCategory,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };

        return Created(dto);
    }

    /// <summary>
    /// Update an existing account
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound($"Account with ID {id} not found");
        }

        if (account.IsSystemAccount)
        {
            return BadRequest("Cannot modify system accounts");
        }

        account.AccountName = request.AccountName;
        account.IsActive = request.IsActive;
        account.IsCOGS = request.IsCOGS;
        account.IsNonDeductible = request.IsNonDeductible;
        account.TaxCategory = request.TaxCategory;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated account {AccountId} - {AccountName}",
            account.Id, account.AccountName);

        var dto = new AccountDto
        {
            Id = account.Id,
            CompanyId = account.CompanyId,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Type = account.Type,
            SubType = account.SubType,
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsSystemAccount = account.IsSystemAccount,
            IsCOGS = account.IsCOGS,
            IsNonDeductible = account.IsNonDeductible,
            TaxCategory = account.TaxCategory,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };

        return Ok(dto);
    }
}

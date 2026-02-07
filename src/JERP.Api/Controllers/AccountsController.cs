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
            .Include(a => a.FASBTopic)
            .Include(a => a.FASBSubtopic)
            .OrderBy(a => a.AccountNumber)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                CompanyId = a.CompanyId,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Type = a.Type,
                Balance = a.Balance,
                IsActive = a.IsActive,
                IsSystemAccount = a.IsSystemAccount,
                IsCOGS = a.IsCOGS,
                IsNonDeductible = a.IsNonDeductible,
                TaxCategory = a.TaxCategory,
                FASBTopicId = a.FASBTopicId,
                FASBSubtopicId = a.FASBSubtopicId,
                FASBReference = a.FASBReference,
                FASBTopicName = a.FASBTopic != null ? a.FASBTopic.TopicName : null,
                FASBSubtopicName = a.FASBSubtopic != null ? a.FASBSubtopic.SubtopicName : null,
                FASBCategory = a.FASBTopic != null ? (Core.Enums.FASBCategory?)a.FASBTopic.Category : null,
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
            .Include(a => a.FASBTopic)
            .Include(a => a.FASBSubtopic)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                CompanyId = a.CompanyId,
                AccountNumber = a.AccountNumber,
                AccountName = a.AccountName,
                Type = a.Type,
                Balance = a.Balance,
                IsActive = a.IsActive,
                IsSystemAccount = a.IsSystemAccount,
                IsCOGS = a.IsCOGS,
                IsNonDeductible = a.IsNonDeductible,
                TaxCategory = a.TaxCategory,
                FASBTopicId = a.FASBTopicId,
                FASBSubtopicId = a.FASBSubtopicId,
                FASBReference = a.FASBReference,
                FASBTopicName = a.FASBTopic != null ? a.FASBTopic.TopicName : null,
                FASBSubtopicName = a.FASBSubtopic != null ? a.FASBSubtopic.SubtopicName : null,
                FASBCategory = a.FASBTopic != null ? (Core.Enums.FASBCategory?)a.FASBTopic.Category : null,
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
            IsActive = true,
            IsSystemAccount = false,
            IsCOGS = request.IsCOGS,
            IsNonDeductible = request.IsNonDeductible,
            TaxCategory = request.TaxCategory,
            FASBTopicId = request.FASBTopicId,
            FASBSubtopicId = request.FASBSubtopicId,
            FASBReference = request.FASBReference
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
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsSystemAccount = account.IsSystemAccount,
            IsCOGS = account.IsCOGS,
            IsNonDeductible = account.IsNonDeductible,
            TaxCategory = account.TaxCategory,
            FASBTopicId = account.FASBTopicId,
            FASBSubtopicId = account.FASBSubtopicId,
            FASBReference = account.FASBReference,
            FASBTopicName = null,
            FASBSubtopicName = null,
            FASBCategory = null,
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
        account.FASBTopicId = request.FASBTopicId;
        account.FASBSubtopicId = request.FASBSubtopicId;
        account.FASBReference = request.FASBReference;

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
            Balance = account.Balance,
            IsActive = account.IsActive,
            IsSystemAccount = account.IsSystemAccount,
            IsCOGS = account.IsCOGS,
            IsNonDeductible = account.IsNonDeductible,
            TaxCategory = account.TaxCategory,
            FASBTopicId = account.FASBTopicId,
            FASBSubtopicId = account.FASBSubtopicId,
            FASBReference = account.FASBReference,
            FASBTopicName = null,
            FASBSubtopicName = null,
            FASBCategory = null,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Get all FASB topics
    /// </summary>
    [HttpGet("~/api/v1/finance/fasb-topics")]
    public async Task<IActionResult> GetFASBTopics([FromQuery] string? category = null)
    {
        var query = _context.FASBTopics.AsQueryable();

        if (!string.IsNullOrEmpty(category) && Enum.TryParse<Core.Enums.FASBCategory>(category, true, out var fasbCategory))
        {
            query = query.Where(t => t.Category == fasbCategory);
        }

        var topics = await query
            .OrderBy(t => t.TopicCode)
            .Select(t => new FASBTopicDto
            {
                Id = t.Id,
                TopicCode = t.TopicCode,
                TopicName = t.TopicName,
                Category = t.Category,
                Description = t.Description,
                IsSuperseded = t.IsSuperseded,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(topics);
    }

    /// <summary>
    /// Get subtopics for a specific FASB topic
    /// </summary>
    [HttpGet("~/api/v1/finance/fasb-topics/{topicId}/subtopics")]
    public async Task<IActionResult> GetFASBSubtopics(Guid topicId)
    {
        var topic = await _context.FASBTopics.FindAsync(topicId);
        if (topic == null)
        {
            return NotFound($"FASB topic with ID {topicId} not found");
        }

        var subtopics = await _context.FASBSubtopics
            .Where(s => s.FASBTopicId == topicId)
            .OrderBy(s => s.SubtopicCode)
            .Select(s => new FASBSubtopicDto
            {
                Id = s.Id,
                FASBTopicId = s.FASBTopicId,
                SubtopicCode = s.SubtopicCode,
                SubtopicName = s.SubtopicName,
                FullReference = s.FullReference,
                Description = s.Description,
                IsRepealed = s.IsRepealed,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();

        return Ok(subtopics);
    }
}

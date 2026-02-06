/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using JERP.Application.DTOs.Finance;
using JERP.Application.Services.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JERP.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/finance/account-templates")]
public class AccountTemplatesController : ControllerBase
{
    private readonly IAccountTemplateService _templateService;
    private readonly ILogger<AccountTemplatesController> _logger;

    public AccountTemplatesController(
        IAccountTemplateService templateService,
        ILogger<AccountTemplatesController> logger)
    {
        _templateService = templateService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available account templates
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AccountTemplateSummaryDto>), 200)]
    public async Task<IActionResult> GetAvailableTemplates()
    {
        var templates = await _templateService.GetAvailableTemplatesAsync();
        return Ok(templates);
    }

    /// <summary>
    /// Get template details with all accounts
    /// </summary>
    [HttpGet("{templateCode}")]
    [ProducesResponseType(typeof(AccountTemplateDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTemplate(string templateCode)
    {
        var template = await _templateService.GetTemplateAsync(templateCode);
        
        if (template == null)
            return NotFound(new { message = $"Template '{templateCode}' not found" });
        
        return Ok(template);
    }

    /// <summary>
    /// Load template into company's chart of accounts
    /// </summary>
    [HttpPost("{templateCode}/load")]
    [ProducesResponseType(typeof(TemplateLoadResultDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> LoadTemplate(
        string templateCode,
        [FromBody] LoadTemplateRequest request)
    {
        var result = await _templateService.LoadTemplateAsync(
            request.CompanyId,
            templateCode,
            request.OverwriteExisting);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}

public class LoadTemplateRequest
{
    public Guid CompanyId { get; set; }
    public bool OverwriteExisting { get; set; } = false;
}

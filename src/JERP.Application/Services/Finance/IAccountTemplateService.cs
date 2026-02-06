/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using JERP.Application.DTOs.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for managing Chart of Accounts templates
/// </summary>
public interface IAccountTemplateService
{
    /// <summary>
    /// Get all available templates (summary only)
    /// </summary>
    Task<List<AccountTemplateSummaryDto>> GetAvailableTemplatesAsync();
    
    /// <summary>
    /// Get full template with all accounts
    /// </summary>
    Task<AccountTemplateDto?> GetTemplateAsync(string templateCode);
    
    /// <summary>
    /// Load template into a company's chart of accounts
    /// </summary>
    Task<TemplateLoadResultDto> LoadTemplateAsync(Guid companyId, string templateCode, bool overwriteExisting = false);
}

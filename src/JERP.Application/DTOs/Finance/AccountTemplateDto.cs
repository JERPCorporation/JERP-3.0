/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Account template with full account list
/// </summary>
public class AccountTemplateDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Industry { get; set; }
    public int AccountCount { get; set; }
    public AccountBreakdownDto Breakdown { get; set; } = new();
    public List<AccountTemplateItemDto> Accounts { get; set; } = new();
}

/// <summary>
/// Summary of available templates
/// </summary>
public class AccountTemplateSummaryDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Industry { get; set; }
    public int AccountCount { get; set; }
    public AccountBreakdownDto Breakdown { get; set; } = new();
}

/// <summary>
/// Account type breakdown
/// </summary>
public class AccountBreakdownDto
{
    public int Assets { get; set; }
    public int Liabilities { get; set; }
    public int Equity { get; set; }
    public int Revenue { get; set; }
    public int Expenses { get; set; }
}

/// <summary>
/// Individual account in template
/// </summary>
public class AccountTemplateItemDto
{
    public required string AccountNumber { get; set; }
    public required string AccountName { get; set; }
    public required string Type { get; set; }
    public required string FASBTopicCode { get; set; }
    public required string FASBSubtopicCode { get; set; }
    public required string FASBReference { get; set; }
    public required string NormalBalance { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>
/// Response after loading a template
/// </summary>
public class TemplateLoadResultDto
{
    public int AccountsCreated { get; set; }
    public int AccountsSkipped { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool Success => Errors.Count == 0;
}

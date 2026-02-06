/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using System.Text.Json;
using JERP.Application.DTOs.Finance;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

public class AccountTemplateService : IAccountTemplateService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<AccountTemplateService> _logger;
    private readonly string _templatesPath;

    public AccountTemplateService(
        JerpDbContext context,
        ILogger<AccountTemplateService> logger)
    {
        _context = context;
        _logger = logger;
        
        // Try multiple paths for template files
        var possiblePaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Data", "Templates"),
            Path.Combine(Directory.GetCurrentDirectory(), "Data", "Templates"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Templates")
        };
        
        _templatesPath = possiblePaths.FirstOrDefault(Directory.Exists) 
            ?? possiblePaths[0]; // Use first path as fallback
        
        if (!Directory.Exists(_templatesPath))
        {
            _logger.LogWarning("Templates directory not found at any of the expected paths. Using fallback: {TemplatesPath}", _templatesPath);
        }
        else
        {
            _logger.LogInformation("Template path set to: {TemplatesPath}", _templatesPath);
        }
    }

    public async Task<List<AccountTemplateSummaryDto>> GetAvailableTemplatesAsync()
    {
        // Ensure directory exists
        if (!Directory.Exists(_templatesPath))
        {
            try
            {
                _logger.LogWarning("Templates directory not found: {Path}. Attempting to create...", _templatesPath);
                Directory.CreateDirectory(_templatesPath);
                _logger.LogInformation("Successfully created templates directory: {Path}", _templatesPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create templates directory: {Path}. Template loading may fail.", _templatesPath);
                // Don't throw here - allow the method to continue and fail later when trying to load templates
            }
        }
        
        var templates = new List<AccountTemplateSummaryDto>
        {
            new AccountTemplateSummaryDto
            {
                Code = "basic-business",
                Name = "Basic Business",
                Description = "Perfect for service businesses, consultants, and freelancers",
                Industry = "General",
                AccountCount = 50,
                Breakdown = new AccountBreakdownDto { Assets = 15, Liabilities = 10, Equity = 5, Revenue = 10, Expenses = 10 }
            },
            new AccountTemplateSummaryDto
            {
                Code = "manufacturing",
                Name = "Manufacturing",
                Description = "For product manufacturers and assembly operations",
                Industry = "Manufacturing",
                AccountCount = 100,
                Breakdown = new AccountBreakdownDto { Assets = 30, Liabilities = 20, Equity = 10, Revenue = 10, Expenses = 30 }
            },
            new AccountTemplateSummaryDto
            {
                Code = "retail",
                Name = "Retail",
                Description = "For retail stores, e-commerce, and dispensaries",
                Industry = "Retail",
                AccountCount = 75,
                Breakdown = new AccountBreakdownDto { Assets = 25, Liabilities = 15, Equity = 5, Revenue = 15, Expenses = 15 }
            },
            new AccountTemplateSummaryDto
            {
                Code = "cannabis",
                Name = "Cannabis Business",
                Description = "IRC 280E compliant template for licensed cannabis operations",
                Industry = "Cannabis",
                AccountCount = 120,
                Breakdown = new AccountBreakdownDto { Assets = 35, Liabilities = 20, Equity = 10, Revenue = 20, Expenses = 35 }
            },
            new AccountTemplateSummaryDto
            {
                Code = "construction",
                Name = "Construction",
                Description = "For general contractors and subcontractors",
                Industry = "Construction",
                AccountCount = 90,
                Breakdown = new AccountBreakdownDto { Assets = 25, Liabilities = 20, Equity = 10, Revenue = 15, Expenses = 20 }
            }
        };

        return await Task.FromResult(templates);
    }

    public async Task<AccountTemplateDto?> GetTemplateAsync(string templateCode)
    {
        var templateFile = Path.Combine(_templatesPath, $"{templateCode}.json");
        
        if (!File.Exists(templateFile))
        {
            _logger.LogWarning("Template file not found: {TemplateCode}", templateCode);
            return null;
        }

        try
        {
            var json = await File.ReadAllTextAsync(templateFile);
            var template = JsonSerializer.Deserialize<AccountTemplateDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading template: {TemplateCode}", templateCode);
            return null;
        }
    }

    public async Task<TemplateLoadResultDto> LoadTemplateAsync(Guid companyId, string templateCode, bool overwriteExisting = false)
    {
        var result = new TemplateLoadResultDto();

        // Verify company exists
        var company = await _context.Companies.FindAsync(companyId);
        if (company == null)
        {
            result.Errors.Add("Company not found");
            return result;
        }

        // Load template
        var template = await GetTemplateAsync(templateCode);
        if (template == null)
        {
            result.Errors.Add($"Template '{templateCode}' not found");
            return result;
        }

        // Get existing accounts
        var existingAccounts = await _context.Accounts
            .Where(a => a.CompanyId == companyId)
            .Select(a => a.AccountNumber)
            .ToListAsync();

        // Get FASB topics and subtopics for mapping
        var fasbTopics = await _context.FASBTopics
            .Include(t => t.Subtopics)
            .ToListAsync();

        foreach (var templateItem in template.Accounts)
        {
            // Skip if account exists and not overwriting
            if (existingAccounts.Contains(templateItem.AccountNumber) && !overwriteExisting)
            {
                result.AccountsSkipped++;
                continue;
            }

            try
            {
                // Find FASB topic and subtopic
                var fasbTopic = fasbTopics.FirstOrDefault(t => t.TopicCode == templateItem.FASBTopicCode);
                if (fasbTopic == null)
                {
                    result.Errors.Add($"FASB Topic {templateItem.FASBTopicCode} not found for account {templateItem.AccountNumber}");
                    continue;
                }

                var fasbSubtopic = fasbTopic.Subtopics.FirstOrDefault(s => s.SubtopicCode == templateItem.FASBSubtopicCode);
                if (fasbSubtopic == null)
                {
                    result.Errors.Add($"FASB Subtopic {templateItem.FASBTopicCode}-{templateItem.FASBSubtopicCode} not found for account {templateItem.AccountNumber}");
                    continue;
                }

                // Parse account type
                if (!Enum.TryParse<AccountType>(templateItem.Type, out var accountType))
                {
                    result.Errors.Add($"Invalid account type '{templateItem.Type}' for account {templateItem.AccountNumber}");
                    continue;
                }

                // Create or update account
                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyId,
                    AccountNumber = templateItem.AccountNumber,
                    AccountName = templateItem.AccountName,
                    Type = accountType,
                    FASBTopicId = fasbTopic.Id,
                    FASBSubtopicId = fasbSubtopic.Id,
                    Balance = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Accounts.Add(account);
                result.AccountsCreated++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account {AccountNumber}", templateItem.AccountNumber);
                result.Errors.Add($"Error creating account {templateItem.AccountNumber}: {ex.Message}");
            }
        }

        if (result.AccountsCreated > 0)
        {
            await _context.SaveChangesAsync();
        }

        return result;
    }
}

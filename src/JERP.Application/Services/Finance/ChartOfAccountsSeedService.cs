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

using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for seeding the cannabis-specific chart of accounts
/// </summary>
public class ChartOfAccountsSeedService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<ChartOfAccountsSeedService> _logger;

    public ChartOfAccountsSeedService(
        JerpDbContext context,
        ILogger<ChartOfAccountsSeedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the standard chart of accounts for a new company
    /// NOTE: As of JERP 3.0 with FASB framework, this method is OBSOLETE.
    /// Users must create their own accounts and map them to FASB topics/subtopics.
    /// The FASB reference data (topics and subtopics) is automatically seeded via FASBDataSeeder.
    /// </summary>
    [Obsolete("Account seeding is disabled. Users must create accounts manually with FASB mapping.")]
    public async Task SeedChartOfAccountsAsync(Guid companyId)
    {
        _logger.LogWarning(
            "SeedChartOfAccountsAsync is obsolete. " +
            "Users must create their own accounts with FASB topic/subtopic mapping. " +
            "FASB reference data is automatically seeded on database initialization.");
        
        // DO NOT PRE-POPULATE ACCOUNTS
        // Users create accounts via UI and select appropriate FASB topics/subtopics
        await Task.CompletedTask;
    }
}

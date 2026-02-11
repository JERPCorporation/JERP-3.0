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

using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for journal entry operations.
/// 
/// WHY THIS EXISTS:
/// Previously, all journal entry logic (validation, number generation, balance updates)
/// lived directly in JournalEntriesController. That created two problems:
///   1. The logic couldn't be reused (e.g., payroll processing also creates journal entries)
///   2. The controller was doing business logic AND HTTP concerns - it should only do HTTP
/// 
/// This interface defines WHAT the service can do. The implementation (JournalEntryService)
/// defines HOW it does it. The controller just calls these methods and returns HTTP responses.
/// 
/// PATTERN: This follows the same pattern as ISalesOrderService, IPayrollService, etc.
/// </summary>
public interface IJournalEntryService
{
    /// <summary>
    /// Get journal entries with optional filters
    /// </summary>
    Task<List<JournalEntryDto>> GetAllAsync(
        Guid companyId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        JournalEntryStatus? status = null);

    /// <summary>
    /// Get a single journal entry with its ledger entries
    /// </summary>
    Task<JournalEntryDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Create a new manual journal entry with ledger lines.
    /// Validates that debits equal credits and all accounts exist.
    /// </summary>
    Task<JournalEntryDto> CreateAsync(CreateJournalEntryRequest request);

    /// <summary>
    /// Post a draft journal entry - updates account balances.
    /// Only draft, balanced entries can be posted.
    /// </summary>
    Task<JournalEntryDto> PostAsync(Guid id);

    /// <summary>
    /// Void a journal entry - reverses account balances if it was posted.
    /// </summary>
    Task<JournalEntryDto> VoidAsync(Guid id);
}

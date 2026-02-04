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

using Microsoft.EntityFrameworkCore;

namespace JERP.Infrastructure.Data.Providers;

/// <summary>
/// Interface for database provider factory implementations
/// </summary>
public interface IDatabaseProviderFactory
{
    /// <summary>
    /// Gets the name of the database provider
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Gets the default port for the database provider
    /// </summary>
    string DefaultPort { get; }

    /// <summary>
    /// Configures the DbContext options builder for the specific provider
    /// </summary>
    void ConfigureDbContext(DbContextOptionsBuilder builder, string connectionString);

    /// <summary>
    /// Checks if the provider supports a specific database feature
    /// </summary>
    bool SupportsFeature(DatabaseFeature feature);

    /// <summary>
    /// Gets the connection string template for the provider
    /// </summary>
    string GetConnectionStringTemplate();
}

/// <summary>
/// Enumeration of database features that may vary by provider
/// </summary>
public enum DatabaseFeature
{
    JsonSupport,
    FullTextSearch,
    Sequences,
    MaterializedViews,
    RowLevelSecurity,
    TemporalTables,
    WindowsAuthentication
}

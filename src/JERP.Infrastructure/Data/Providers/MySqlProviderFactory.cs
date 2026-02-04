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
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace JERP.Infrastructure.Data.Providers;

/// <summary>
/// MySQL database provider factory
/// </summary>
public class MySqlProviderFactory : IDatabaseProviderFactory
{
    public string ProviderName => "MySQL";
    
    public string DefaultPort => "3306";

    public void ConfigureDbContext(DbContextOptionsBuilder builder, string connectionString)
    {
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        builder.UseMySql(connectionString, serverVersion, options =>
        {
            options.EnableRetryOnFailure(3);
            options.CommandTimeout(30);
            options.MigrationsHistoryTable("__EFMigrationsHistory");
        });
    }

    public bool SupportsFeature(DatabaseFeature feature)
    {
        return feature switch
        {
            DatabaseFeature.JsonSupport => true,
            DatabaseFeature.FullTextSearch => true,
            _ => false
        };
    }

    public string GetConnectionStringTemplate()
        => "Server={host};Port={port};Database={database};Uid={username};Pwd={password};CharSet=utf8mb4;";
}

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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JERP.Infrastructure.Data;

/// <summary>
/// Design-time factory for creating JerpDbContext during EF Core migrations.
/// This allows EF Core tools to create the DbContext without running the full application.
/// </summary>
public class JerpDbContextFactory : IDesignTimeDbContextFactory<JerpDbContext>
{
    /// <summary>
    /// Creates a new instance of JerpDbContext for design-time operations (migrations, scaffolding).
    /// </summary>
    /// <param name="args">Command-line arguments passed to the EF Core tools</param>
    /// <returns>Configured JerpDbContext instance</returns>
    public JerpDbContext CreateDbContext(string[] args)
    {
        // Build configuration from the API project's appsettings.json
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../JERP.Api");
        
        // Handle cases where the command is run from different directories
        if (!Directory.Exists(basePath))
        {
            basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../JERP.Api");
        }
        
        if (!Directory.Exists(basePath))
        {
            throw new InvalidOperationException(
                "Could not locate JERP.Api directory. Ensure you're running migrations from the src directory.");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in appsettings.json");
        }

        // Create DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<JerpDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("JERP.Infrastructure");
            sqlOptions.CommandTimeout(30);
        });

        return new JerpDbContext(optionsBuilder.Options);
    }
}

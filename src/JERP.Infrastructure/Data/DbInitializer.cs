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

using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace JERP.Infrastructure.Data;

/// <summary>
/// Database initializer for seeding initial data
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Seeds the database with initial data including admin user, roles, permissions, and sample company
    /// </summary>
    public static async Task SeedAsync(JerpDbContext context)
    {
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Permissions
            await SeedPermissionsAsync(context);

            // Seed Roles
            await SeedRolesAsync(context);

            // Seed Admin User
            await SeedAdminUserAsync(context);

            // Seed Sample Company
            await SeedSampleCompanyAsync(context);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to seed database", ex);
        }
    }

    /// <summary>
    /// Seeds system permissions
    /// </summary>
    private static async Task SeedPermissionsAsync(JerpDbContext context)
    {
        if (await context.Permissions.AnyAsync())
        {
            return; // Permissions already seeded
        }

        var permissions = new List<Permission>
        {
            // Employee Permissions
            new Permission { Name = "View Employees", Code = "employees.view", Category = "Employees", Description = "View employee information" },
            new Permission { Name = "Create Employees", Code = "employees.create", Category = "Employees", Description = "Create new employees" },
            new Permission { Name = "Update Employees", Code = "employees.update", Category = "Employees", Description = "Update employee information" },
            new Permission { Name = "Delete Employees", Code = "employees.delete", Category = "Employees", Description = "Delete employees" },

            // Timesheet Permissions
            new Permission { Name = "View Timesheets", Code = "timesheets.view", Category = "Timesheets", Description = "View timesheet entries" },
            new Permission { Name = "Create Timesheets", Code = "timesheets.create", Category = "Timesheets", Description = "Create timesheet entries" },
            new Permission { Name = "Submit Timesheets", Code = "timesheets.submit", Category = "Timesheets", Description = "Submit timesheets for approval" },
            new Permission { Name = "Approve Timesheets", Code = "timesheets.approve", Category = "Timesheets", Description = "Approve or reject timesheets" },

            // Payroll Permissions
            new Permission { Name = "View Payroll", Code = "payroll.view", Category = "Payroll", Description = "View payroll information" },
            new Permission { Name = "Process Payroll", Code = "payroll.process", Category = "Payroll", Description = "Process payroll for pay periods" },
            new Permission { Name = "Approve Payroll", Code = "payroll.approve", Category = "Payroll", Description = "Approve processed payroll" },

            // Compliance Permissions
            new Permission { Name = "View Compliance", Code = "compliance.view", Category = "Compliance", Description = "View compliance violations" },
            new Permission { Name = "Resolve Compliance", Code = "compliance.resolve", Category = "Compliance", Description = "Resolve compliance violations" },

            // User Management Permissions
            new Permission { Name = "View Users", Code = "users.view", Category = "Users", Description = "View user accounts" },
            new Permission { Name = "Create Users", Code = "users.create", Category = "Users", Description = "Create new user accounts" },
            new Permission { Name = "Update Users", Code = "users.update", Category = "Users", Description = "Update user accounts" },
            new Permission { Name = "Delete Users", Code = "users.delete", Category = "Users", Description = "Delete user accounts" },

            // Company Permissions
            new Permission { Name = "View Companies", Code = "companies.view", Category = "Companies", Description = "View company information" },
            new Permission { Name = "Create Companies", Code = "companies.create", Category = "Companies", Description = "Create new companies" },
            new Permission { Name = "Update Companies", Code = "companies.update", Category = "Companies", Description = "Update company information" },
            new Permission { Name = "Delete Companies", Code = "companies.delete", Category = "Companies", Description = "Delete companies" },

            // Department Permissions
            new Permission { Name = "View Departments", Code = "departments.view", Category = "Departments", Description = "View department information" },
            new Permission { Name = "Create Departments", Code = "departments.create", Category = "Departments", Description = "Create new departments" },
            new Permission { Name = "Update Departments", Code = "departments.update", Category = "Departments", Description = "Update department information" },
            new Permission { Name = "Delete Departments", Code = "departments.delete", Category = "Departments", Description = "Delete departments" },

            // Report Permissions
            new Permission { Name = "View Reports", Code = "reports.view", Category = "Reports", Description = "View system reports" },
            new Permission { Name = "Generate Reports", Code = "reports.generate", Category = "Reports", Description = "Generate custom reports" },

            // Audit Permissions
            new Permission { Name = "View Audit Logs", Code = "audit.view", Category = "Audit", Description = "View audit log entries" }
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds system roles
    /// </summary>
    private static async Task SeedRolesAsync(JerpDbContext context)
    {
        if (await context.Roles.AnyAsync())
        {
            return; // Roles already seeded
        }

        var allPermissions = await context.Permissions.ToListAsync();

        var roles = new List<Role>
        {
            new Role
            {
                Name = "SystemAdmin",
                Description = "System administrator with full access to all features"
            },
            new Role
            {
                Name = "CompanyAdmin",
                Description = "Company administrator with access to company-specific features"
            },
            new Role
            {
                Name = "Manager",
                Description = "Manager with access to team management and approval features"
            },
            new Role
            {
                Name = "Employee",
                Description = "Standard employee with basic access to personal information"
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();

        // Assign permissions to roles
        var systemAdminRole = await context.Roles.Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "SystemAdmin");
        
        if (systemAdminRole != null)
        {
            // System Admin gets all permissions
            systemAdminRole.Permissions = allPermissions;
        }

        var companyAdminRole = await context.Roles.Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "CompanyAdmin");
        
        if (companyAdminRole != null)
        {
            // Company Admin gets most permissions except system-level ones
            companyAdminRole.Permissions = allPermissions
                .Where(p => !p.Code.StartsWith("users.") || p.Code == "users.view")
                .ToList();
        }

        var managerRole = await context.Roles.Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "Manager");
        
        if (managerRole != null)
        {
            // Manager gets approval and view permissions
            managerRole.Permissions = allPermissions
                .Where(p => p.Code.Contains("view") || 
                           p.Code.Contains("approve") || 
                           p.Code == "timesheets.create" ||
                           p.Code == "employees.update")
                .ToList();
        }

        var employeeRole = await context.Roles.Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "Employee");
        
        if (employeeRole != null)
        {
            // Employee gets basic view and create permissions
            employeeRole.Permissions = allPermissions
                .Where(p => p.Code == "timesheets.view" || 
                           p.Code == "timesheets.create" || 
                           p.Code == "timesheets.submit")
                .ToList();
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds the admin user
    /// </summary>
    private static async Task SeedAdminUserAsync(JerpDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            return; // Admin user already exists
        }

        var systemAdminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Name == "SystemAdmin");

        if (systemAdminRole == null)
        {
            throw new InvalidOperationException("SystemAdmin role not found. Please seed roles first.");
        }

        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@jerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            FirstName = "System",
            LastName = "Administrator",
            IsActive = true
        };

        adminUser.Roles.Add(systemAdminRole);

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds a sample company with departments
    /// </summary>
    private static async Task SeedSampleCompanyAsync(JerpDbContext context)
    {
        if (await context.Companies.AnyAsync())
        {
            return; // Company already exists
        }

        var company = new Company
        {
            Name = "Sample Corporation",
            TaxId = "12-3456789",
            Address = "123 Business Street",
            City = "New York",
            State = "NY",
            ZipCode = "10001",
            Phone = "(555) 123-4567",
            Email = "info@samplecorp.com"
        };

        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        var departments = new List<Department>
        {
            new Department
            {
                CompanyId = company.Id,
                Name = "Engineering",
                Description = "Software development and technology team"
            },
            new Department
            {
                CompanyId = company.Id,
                Name = "Human Resources",
                Description = "HR and employee management team"
            }
        };

        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();
    }
}

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
using JERP.Application.Services.Security;
using JERP.Core.Entities.Finance;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for customer management
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<CustomerService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Where(c => c.Id == id)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                CompanyId = c.CompanyId,
                CustomerNumber = c.CustomerNumber,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                Email = c.Email,
                Phone = c.Phone,
                Street = c.Street,
                City = c.City,
                State = c.State,
                PostalCode = c.PostalCode,
                Country = c.Country,
                TaxId = c.TaxId,
                PaymentTerms = c.PaymentTerms,
                AccountsReceivableAccountId = c.AccountsReceivableAccountId,
                Balance = c.Balance,
                CreditLimit = c.CreditLimit,
                IsActive = c.IsActive,
                CannabisLicense = c.CannabisLicense,
                IsCannabisCustomer = c.IsCannabisCustomer,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get all customers for a company
    /// </summary>
    public async Task<List<CustomerListDto>> GetAllAsync(Guid companyId, bool? isActive = null)
    {
        var query = _context.Customers
            .Where(c => c.CompanyId == companyId);

        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(c => c.CustomerNumber)
            .Select(c => new CustomerListDto
            {
                Id = c.Id,
                CustomerNumber = c.CustomerNumber,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                Email = c.Email,
                Phone = c.Phone,
                Balance = c.Balance,
                CreditLimit = c.CreditLimit,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    public async Task<CustomerDto> CreateAsync(Guid companyId, CreateCustomerDto dto)
    {
        var customerNumber = await GenerateCustomerNumberAsync(companyId);

        var customer = new Customer
        {
            CompanyId = companyId,
            CustomerNumber = customerNumber,
            CompanyName = dto.CompanyName,
            ContactName = dto.ContactName,
            Email = dto.Email,
            Phone = dto.Phone,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            TaxId = dto.TaxId,
            PaymentTerms = dto.PaymentTerms,
            AccountsReceivableAccountId = dto.AccountsReceivableAccountId,
            Balance = 0,
            CreditLimit = dto.CreditLimit,
            IsActive = true,
            CannabisLicense = dto.CannabisLicense,
            IsCannabisCustomer = dto.IsCannabisCustomer,
            Notes = dto.Notes
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created customer {CustomerNumber} - {CompanyName} for company {CompanyId}",
            customer.CustomerNumber, customer.CompanyName, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "customer_created",
                $"Customer:{customer.Id}",
                $"Created customer {customer.CustomerNumber} - {customer.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for customer creation");
        }

        return (await GetByIdAsync(customer.Id))!;
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer {id} not found");
        }

        customer.CompanyName = dto.CompanyName;
        customer.ContactName = dto.ContactName;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
        customer.Street = dto.Street;
        customer.City = dto.City;
        customer.State = dto.State;
        customer.PostalCode = dto.PostalCode;
        customer.Country = dto.Country;
        customer.TaxId = dto.TaxId;
        customer.PaymentTerms = dto.PaymentTerms;
        customer.AccountsReceivableAccountId = dto.AccountsReceivableAccountId;
        customer.CreditLimit = dto.CreditLimit;
        customer.IsActive = dto.IsActive;
        customer.CannabisLicense = dto.CannabisLicense;
        customer.IsCannabisCustomer = dto.IsCannabisCustomer;
        customer.Notes = dto.Notes;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated customer {CustomerNumber} - {CompanyName}",
            customer.CustomerNumber, customer.CompanyName);

        try
        {
            await _auditLogService.LogAction(
                customer.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "customer_updated",
                $"Customer:{customer.Id}",
                $"Updated customer {customer.CustomerNumber} - {customer.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for customer update");
        }

        return (await GetByIdAsync(customer.Id))!;
    }

    /// <summary>
    /// Soft delete a customer
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer {id} not found");
        }

        customer.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted (soft) customer {CustomerNumber} - {CompanyName}",
            customer.CustomerNumber, customer.CompanyName);

        try
        {
            await _auditLogService.LogAction(
                customer.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "customer_deleted",
                $"Customer:{customer.Id}",
                $"Deleted customer {customer.CustomerNumber} - {customer.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for customer deletion");
        }
    }

    /// <summary>
    /// Get customer balance
    /// </summary>
    public async Task<decimal> GetCustomerBalanceAsync(Guid customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer {customerId} not found");
        }

        return customer.Balance;
    }

    /// <summary>
    /// Get available credit for customer
    /// </summary>
    public async Task<decimal> GetAvailableCreditAsync(Guid customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer {customerId} not found");
        }

        return customer.CreditLimit - customer.Balance;
    }

    /// <summary>
    /// Generate next customer number
    /// </summary>
    private async Task<string> GenerateCustomerNumberAsync(Guid companyId)
    {
        var lastCustomer = await _context.Customers
            .Where(c => c.CompanyId == companyId)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastCustomer == null)
        {
            return "CUS-0001";
        }

        var lastNumber = int.Parse(lastCustomer.CustomerNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"CUS-{nextNumber:D4}";
    }
}

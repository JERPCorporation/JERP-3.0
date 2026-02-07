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
/// Service for vendor management
/// </summary>
public class VendorService : IVendorService
{
    private readonly JerpDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<VendorService> _logger;

    public VendorService(
        JerpDbContext context,
        IAuditLogService auditLogService,
        ILogger<VendorService> logger)
    {
        _context = context;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Get vendor by ID
    /// </summary>
    public async Task<VendorDto?> GetByIdAsync(Guid id)
    {
        return await _context.Vendors
            .Where(v => v.Id == id)
            .Select(v => new VendorDto
            {
                Id = v.Id,
                CompanyId = v.CompanyId,
                VendorNumber = v.VendorNumber,
                CompanyName = v.CompanyName,
                ContactName = v.ContactName,
                Email = v.Email,
                Phone = v.Phone,
                Street = v.Street,
                City = v.City,
                State = v.State,
                PostalCode = v.PostalCode,
                Country = v.Country,
                TaxId = v.TaxId,
                PaymentTerms = v.PaymentTerms,
                AccountsPayableAccountId = v.AccountsPayableAccountId,
                Balance = v.Balance,
                IsActive = v.IsActive,
                CannabisLicense = v.CannabisLicense,
                IsCannabisVendor = v.IsCannabisVendor,
                Notes = v.Notes,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get all vendors for a company
    /// </summary>
    public async Task<List<VendorListDto>> GetAllAsync(Guid companyId, bool? isActive = null)
    {
        var query = _context.Vendors
            .Where(v => v.CompanyId == companyId);

        if (isActive.HasValue)
        {
            query = query.Where(v => v.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(v => v.VendorNumber)
            .Select(v => new VendorListDto
            {
                Id = v.Id,
                VendorNumber = v.VendorNumber,
                CompanyName = v.CompanyName,
                ContactName = v.ContactName,
                Email = v.Email,
                Phone = v.Phone,
                Balance = v.Balance,
                IsActive = v.IsActive
            })
            .ToListAsync();
    }

    /// <summary>
    /// Create a new vendor
    /// </summary>
    public async Task<VendorDto> CreateAsync(Guid companyId, CreateVendorDto dto)
    {
        var vendorNumber = await GenerateVendorNumberAsync(companyId);

        var vendor = new Vendor
        {
            CompanyId = companyId,
            VendorNumber = vendorNumber,
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
            AccountsPayableAccountId = dto.AccountsPayableAccountId,
            Balance = 0,
            IsActive = true,
            CannabisLicense = dto.CannabisLicense,
            IsCannabisVendor = dto.IsCannabisVendor,
            Notes = dto.Notes
        };

        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created vendor {VendorNumber} - {CompanyName} for company {CompanyId}",
            vendor.VendorNumber, vendor.CompanyName, companyId);

        try
        {
            await _auditLogService.LogAction(
                companyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "vendor_created",
                $"Vendor:{vendor.Id}",
                $"Created vendor {vendor.VendorNumber} - {vendor.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for vendor creation");
        }

        return (await GetByIdAsync(vendor.Id))!;
    }

    /// <summary>
    /// Update an existing vendor
    /// </summary>
    public async Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto dto)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor == null)
        {
            throw new InvalidOperationException($"Vendor {id} not found");
        }

        vendor.CompanyName = dto.CompanyName;
        vendor.ContactName = dto.ContactName;
        vendor.Email = dto.Email;
        vendor.Phone = dto.Phone;
        vendor.Street = dto.Street;
        vendor.City = dto.City;
        vendor.State = dto.State;
        vendor.PostalCode = dto.PostalCode;
        vendor.Country = dto.Country;
        vendor.TaxId = dto.TaxId;
        vendor.PaymentTerms = dto.PaymentTerms;
        vendor.AccountsPayableAccountId = dto.AccountsPayableAccountId;
        vendor.IsActive = dto.IsActive;
        vendor.CannabisLicense = dto.CannabisLicense;
        vendor.IsCannabisVendor = dto.IsCannabisVendor;
        vendor.Notes = dto.Notes;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated vendor {VendorNumber} - {CompanyName}",
            vendor.VendorNumber, vendor.CompanyName);

        try
        {
            await _auditLogService.LogAction(
                vendor.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "vendor_updated",
                $"Vendor:{vendor.Id}",
                $"Updated vendor {vendor.VendorNumber} - {vendor.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for vendor update");
        }

        return (await GetByIdAsync(vendor.Id))!;
    }

    /// <summary>
    /// Soft delete a vendor
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor == null)
        {
            throw new InvalidOperationException($"Vendor {id} not found");
        }

        vendor.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted (soft) vendor {VendorNumber} - {CompanyName}",
            vendor.VendorNumber, vendor.CompanyName);

        try
        {
            await _auditLogService.LogAction(
                vendor.CompanyId,
                Guid.Empty,
                "system@jerp.io",
                "System",
                "vendor_deleted",
                $"Vendor:{vendor.Id}",
                $"Deleted vendor {vendor.VendorNumber} - {vendor.CompanyName}",
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create audit log for vendor deletion");
        }
    }

    /// <summary>
    /// Get vendor balance
    /// </summary>
    public async Task<decimal> GetVendorBalanceAsync(Guid vendorId)
    {
        var vendor = await _context.Vendors.FindAsync(vendorId);
        if (vendor == null)
        {
            throw new InvalidOperationException($"Vendor {vendorId} not found");
        }

        return vendor.Balance;
    }

    /// <summary>
    /// Generate next vendor number
    /// </summary>
    public async Task<string> GenerateVendorNumberAsync(Guid companyId)
    {
        var lastVendor = await _context.Vendors
            .Where(v => v.CompanyId == companyId)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastVendor == null)
        {
            return "VEN-0001";
        }

        var lastNumber = int.Parse(lastVendor.VendorNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;

        return $"VEN-{nextNumber:D4}";
    }
}

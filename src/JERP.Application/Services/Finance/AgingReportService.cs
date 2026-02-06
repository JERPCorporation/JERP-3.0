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

using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service for generating aging reports
/// </summary>
public class AgingReportService : IAgingReportService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<AgingReportService> _logger;

    public AgingReportService(
        JerpDbContext context,
        ILogger<AgingReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Generate accounts payable aging report
    /// </summary>
    public async Task<APAgingReportDto> GetAPAgingReportAsync(Guid companyId, DateTime asOfDate)
    {
        var bills = await _context.VendorBills
            .Include(b => b.Vendor)
            .Where(b => b.CompanyId == companyId 
                && b.Status == BillStatus.Approved 
                && !b.IsPaid
                && b.BillDate <= asOfDate)
            .ToListAsync();

        var vendorAging = bills
            .GroupBy(b => new { b.VendorId, b.Vendor.VendorNumber, b.Vendor.CompanyName })
            .Select(g => new VendorAgingDto
            {
                VendorId = g.Key.VendorId,
                VendorNumber = g.Key.VendorNumber,
                VendorName = g.Key.CompanyName,
                Current = g.Where(b => GetDaysOverdue(b.DueDate, asOfDate) <= 0)
                          .Sum(b => b.TotalAmount - b.AmountPaid),
                Days1To30 = g.Where(b => GetDaysOverdue(b.DueDate, asOfDate) >= 1 
                              && GetDaysOverdue(b.DueDate, asOfDate) <= 30)
                          .Sum(b => b.TotalAmount - b.AmountPaid),
                Days31To60 = g.Where(b => GetDaysOverdue(b.DueDate, asOfDate) >= 31 
                               && GetDaysOverdue(b.DueDate, asOfDate) <= 60)
                          .Sum(b => b.TotalAmount - b.AmountPaid),
                Days61To90 = g.Where(b => GetDaysOverdue(b.DueDate, asOfDate) >= 61 
                               && GetDaysOverdue(b.DueDate, asOfDate) <= 90)
                          .Sum(b => b.TotalAmount - b.AmountPaid),
                Days90Plus = g.Where(b => GetDaysOverdue(b.DueDate, asOfDate) > 90)
                          .Sum(b => b.TotalAmount - b.AmountPaid)
            })
            .OrderBy(v => v.VendorName)
            .ToList();

        var summary = new AgingSummaryDto
        {
            Current = vendorAging.Sum(v => v.Current),
            Days1To30 = vendorAging.Sum(v => v.Days1To30),
            Days31To60 = vendorAging.Sum(v => v.Days31To60),
            Days61To90 = vendorAging.Sum(v => v.Days61To90),
            Days90Plus = vendorAging.Sum(v => v.Days90Plus)
        };

        var total = summary.TotalAmount;
        if (total > 0)
        {
            summary.CurrentPercent = summary.Current / total * 100;
            summary.Days1To30Percent = summary.Days1To30 / total * 100;
            summary.Days31To60Percent = summary.Days31To60 / total * 100;
            summary.Days61To90Percent = summary.Days61To90 / total * 100;
            summary.Days90PlusPercent = summary.Days90Plus / total * 100;
        }

        _logger.LogInformation("Generated AP aging report for company {CompanyId} as of {AsOfDate}. Total due: ${TotalDue}",
            companyId, asOfDate, total);

        return new APAgingReportDto
        {
            AsOfDate = asOfDate,
            Vendors = vendorAging,
            Summary = summary
        };
    }

    /// <summary>
    /// Generate accounts receivable aging report
    /// </summary>
    public async Task<ARAgingReportDto> GetARAgingReportAsync(Guid companyId, DateTime asOfDate)
    {
        var invoices = await _context.CustomerInvoices
            .Include(i => i.Customer)
            .Where(i => i.CompanyId == companyId 
                && (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                && !i.IsPaid
                && i.InvoiceDate <= asOfDate)
            .ToListAsync();

        var customerAging = invoices
            .GroupBy(i => new { i.CustomerId, i.Customer.CustomerNumber, i.Customer.CompanyName })
            .Select(g => new CustomerAgingDto
            {
                CustomerId = g.Key.CustomerId,
                CustomerNumber = g.Key.CustomerNumber,
                CustomerName = g.Key.CompanyName,
                Current = g.Where(i => GetDaysOverdue(i.DueDate, asOfDate) <= 0)
                          .Sum(i => i.TotalAmount - i.AmountPaid),
                Days1To30 = g.Where(i => GetDaysOverdue(i.DueDate, asOfDate) >= 1 
                              && GetDaysOverdue(i.DueDate, asOfDate) <= 30)
                          .Sum(i => i.TotalAmount - i.AmountPaid),
                Days31To60 = g.Where(i => GetDaysOverdue(i.DueDate, asOfDate) >= 31 
                               && GetDaysOverdue(i.DueDate, asOfDate) <= 60)
                          .Sum(i => i.TotalAmount - i.AmountPaid),
                Days61To90 = g.Where(i => GetDaysOverdue(i.DueDate, asOfDate) >= 61 
                               && GetDaysOverdue(i.DueDate, asOfDate) <= 90)
                          .Sum(i => i.TotalAmount - i.AmountPaid),
                Days90Plus = g.Where(i => GetDaysOverdue(i.DueDate, asOfDate) > 90)
                          .Sum(i => i.TotalAmount - i.AmountPaid)
            })
            .OrderBy(c => c.CustomerName)
            .ToList();

        var summary = new AgingSummaryDto
        {
            Current = customerAging.Sum(c => c.Current),
            Days1To30 = customerAging.Sum(c => c.Days1To30),
            Days31To60 = customerAging.Sum(c => c.Days31To60),
            Days61To90 = customerAging.Sum(c => c.Days61To90),
            Days90Plus = customerAging.Sum(c => c.Days90Plus)
        };

        var total = summary.TotalAmount;
        if (total > 0)
        {
            summary.CurrentPercent = summary.Current / total * 100;
            summary.Days1To30Percent = summary.Days1To30 / total * 100;
            summary.Days31To60Percent = summary.Days31To60 / total * 100;
            summary.Days61To90Percent = summary.Days61To90 / total * 100;
            summary.Days90PlusPercent = summary.Days90Plus / total * 100;
        }

        _logger.LogInformation("Generated AR aging report for company {CompanyId} as of {AsOfDate}. Total due: ${TotalDue}",
            companyId, asOfDate, total);

        return new ARAgingReportDto
        {
            AsOfDate = asOfDate,
            Customers = customerAging,
            Summary = summary
        };
    }

    private static int GetDaysOverdue(DateTime dueDate, DateTime asOfDate)
    {
        return (int)(asOfDate - dueDate).TotalDays;
    }
}

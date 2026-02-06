/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using JERP.Application.DTOs.Reports;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Reports;

/// <summary>
/// Service for generating payroll reports
/// </summary>
public class PayrollReportService : IPayrollReportService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<PayrollReportService> _logger;

    public PayrollReportService(
        JerpDbContext context,
        ILogger<PayrollReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PayrollSummaryReportDto> GeneratePayrollSummaryAsync(ReportRequestDto request)
    {
        _logger.LogInformation("Generating payroll summary for company {CompanyId}", request.CompanyId);

        var report = new PayrollSummaryReportDto
        {
            CompanyId = request.CompanyId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GeneratedAt = DateTime.UtcNow
        };

        // Get payroll data
        var payrollRecords = await _context.PayrollRecords
            .Include(p => p.Employee)
            .ThenInclude(e => e.Department)
            .Where(p => p.Employee.CompanyId == request.CompanyId
                && p.PayPeriod.StartDate >= request.StartDate
                && p.PayPeriod.EndDate <= request.EndDate)
            .ToListAsync();

        foreach (var payroll in payrollRecords)
        {
            var line = new PayrollSummaryLineDto
            {
                EmployeeId = payroll.EmployeeId,
                EmployeeNumber = payroll.Employee.EmployeeNumber,
                EmployeeName = $"{payroll.Employee.FirstName} {payroll.Employee.LastName}",
                Department = payroll.Employee.Department?.Name ?? "N/A",
                RegularHours = payroll.RegularHours,
                OvertimeHours = payroll.OvertimeHours,
                GrossWages = payroll.GrossPay,
                Deductions = payroll.TotalDeductions,
                NetPay = payroll.NetPay,
                EmployerTaxes = 0, // Simplified - PayrollRecord doesn't have employer tax fields
                TotalCost = payroll.GrossPay
            };
            report.Employees.Add(line);
        }

        // Calculate totals
        report.TotalGrossWages = report.Employees.Sum(e => e.GrossWages);
        report.TotalDeductions = report.Employees.Sum(e => e.Deductions);
        report.TotalNetPay = report.Employees.Sum(e => e.NetPay);
        report.TotalEmployerTaxes = report.Employees.Sum(e => e.EmployerTaxes);
        report.TotalPayrollCost = report.Employees.Sum(e => e.TotalCost);
        report.TotalRegularHours = report.Employees.Sum(e => e.RegularHours);
        report.TotalOvertimeHours = report.Employees.Sum(e => e.OvertimeHours);

        // Group by department
        foreach (var dept in report.Employees.GroupBy(e => e.Department))
        {
            report.CostByDepartment[dept.Key] = dept.Sum(e => e.TotalCost);
        }

        return report;
    }

    public async Task<byte[]> ExportPayrollSummaryToPdfAsync(ReportRequestDto request)
    {
        var report = await GeneratePayrollSummaryAsync(request);
        
        // Placeholder for PDF generation
        return Array.Empty<byte>();
    }

    public async Task<byte[]> Generate941ReportAsync(Guid companyId, int quarter, int year)
    {
        _logger.LogInformation("Generating Form 941 for company {CompanyId}, Q{Quarter} {Year}", companyId, quarter, year);
        
        // Placeholder for Form 941 generation
        return Array.Empty<byte>();
    }

    public async Task<byte[]> GenerateW2sAsync(Guid companyId, int year)
    {
        _logger.LogInformation("Generating W-2s for company {CompanyId}, year {Year}", companyId, year);
        
        // Placeholder for W-2 generation
        return Array.Empty<byte>();
    }
}

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

using JERP.Application.DTOs.Payroll;
using JERP.Application.Services.Finance;
using JERP.Application.Services.Payroll.Tax;
using JERP.Compliance.Services;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Payroll;

/// <summary>
/// Implementation of payroll processing services
/// </summary>
public class PayrollService : IPayrollService
{
    private readonly JerpDbContext _context;
    private readonly ITaxCalculationService _taxCalculationService;
    private readonly IComplianceEngine _complianceEngine;
    private readonly IPayrollToFinanceService? _payrollToFinanceService;
    private readonly ILogger<PayrollService> _logger;

    public PayrollService(
        JerpDbContext context,
        ITaxCalculationService taxCalculationService,
        IComplianceEngine complianceEngine,
        ILogger<PayrollService> logger,
        IPayrollToFinanceService? payrollToFinanceService = null)
    {
        _context = context;
        _taxCalculationService = taxCalculationService;
        _complianceEngine = complianceEngine;
        _logger = logger;
        _payrollToFinanceService = payrollToFinanceService;
    }

    /// <inheritdoc/>
    public async Task<PayPeriodDto> CreatePayPeriodAsync(CreatePayPeriodRequest request)
    {
        _logger.LogInformation("Creating pay period: {StartDate} to {EndDate}",
            request.StartDate, request.EndDate);

        var payPeriod = new PayPeriod
        {
            CompanyId = request.CompanyId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            PayDate = request.PayDate,
            Frequency = request.Frequency,
            Status = PayrollStatus.Draft
        };

        _context.PayPeriods.Add(payPeriod);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Pay period created: {PayPeriodId}", payPeriod.Id);

        return MapPayPeriodToDto(payPeriod);
    }

    /// <inheritdoc/>
    public async Task<PayPeriodDto> GetPayPeriodByIdAsync(Guid id)
    {
        var payPeriod = await _context.PayPeriods
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (payPeriod == null)
        {
            throw new InvalidOperationException($"Pay period with ID {id} not found");
        }

        return MapPayPeriodToDto(payPeriod);
    }

    /// <inheritdoc/>
    public async Task<List<PayPeriodDto>> GetPayPeriodsByYearAsync(int year, Guid companyId)
    {
        var payPeriods = await _context.PayPeriods
            .Where(p => p.CompanyId == companyId &&
                       p.StartDate.Year == year &&
                       !p.IsDeleted)
            .OrderBy(p => p.StartDate)
            .ToListAsync();

        return payPeriods.Select(MapPayPeriodToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<PayrollProcessingResult> ProcessPayrollAsync(Guid payPeriodId)
    {
        _logger.LogInformation("Processing payroll for pay period: {PayPeriodId}", payPeriodId);

        var payPeriod = await _context.PayPeriods
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == payPeriodId && !p.IsDeleted);

        if (payPeriod == null)
        {
            throw new InvalidOperationException($"Pay period with ID {payPeriodId} not found");
        }

        if (payPeriod.Status != PayrollStatus.Draft)
        {
            throw new InvalidOperationException($"Pay period is not in draft status");
        }

        var result = new PayrollProcessingResult();

        // Get all active employees for the company
        var employees = await _context.Employees
            .Where(e => e.CompanyId == payPeriod.CompanyId &&
                       e.Status == EmployeeStatus.Active &&
                       !e.IsDeleted)
            .ToListAsync();

        foreach (var employee in employees)
        {
            try
            {
                var record = await ProcessEmployeePayrollAsync(employee, payPeriod);
                result.ProcessedCount++;
                result.TotalGrossPay += record.GrossPay;
                result.TotalNetPay += record.NetPay;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payroll for employee: {EmployeeId}", employee.Id);
                result.Errors.Add($"Employee {employee.EmployeeNumber}: {ex.Message}");
            }
        }

        // Update pay period totals
        payPeriod.TotalGrossPay = result.TotalGrossPay;
        payPeriod.TotalNetPay = result.TotalNetPay;
        payPeriod.Status = PayrollStatus.Calculated;
        payPeriod.ProcessedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Payroll processing completed: {ProcessedCount} employees, Gross: {Gross}, Net: {Net}",
            result.ProcessedCount, result.TotalGrossPay, result.TotalNetPay);

        return result;
    }

    /// <inheritdoc/>
    public async Task<PayPeriodDto> ApprovePayrollAsync(Guid payPeriodId, Guid approverId)
    {
        _logger.LogInformation("Approving payroll for pay period: {PayPeriodId}", payPeriodId);

        var payPeriod = await _context.PayPeriods
            .FirstOrDefaultAsync(p => p.Id == payPeriodId && !p.IsDeleted);

        if (payPeriod == null)
        {
            throw new InvalidOperationException($"Pay period with ID {payPeriodId} not found");
        }

        if (payPeriod.Status != PayrollStatus.Calculated)
        {
            throw new InvalidOperationException("Only calculated payroll can be approved");
        }

        payPeriod.Status = PayrollStatus.Approved;
        payPeriod.ApprovedAt = DateTime.UtcNow;
        payPeriod.ApprovedById = approverId;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Payroll approved: {PayPeriodId}", payPeriodId);

        // Automatically post to general ledger if finance service is available
        if (_payrollToFinanceService != null)
        {
            try
            {
                _logger.LogInformation("Posting payroll to general ledger: {PayPeriodId}", payPeriodId);
                await _payrollToFinanceService.PostPayrollToGeneralLedgerAsync(payPeriodId);
                _logger.LogInformation("Payroll successfully posted to general ledger: {PayPeriodId}", payPeriodId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to post payroll to general ledger: {PayPeriodId}", payPeriodId);
                // Don't fail the approval if GL posting fails - log and continue
            }
        }

        return MapPayPeriodToDto(payPeriod);
    }

    /// <inheritdoc/>
    public async Task<PayrollRecordDto> GetPayrollRecordAsync(Guid id)
    {
        var record = await _context.PayrollRecords
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (record == null)
        {
            throw new InvalidOperationException($"Payroll record with ID {id} not found");
        }

        return MapPayrollRecordToDto(record);
    }

    /// <inheritdoc/>
    public async Task<List<PayrollRecordDto>> GetEmployeePayrollRecordsAsync(Guid employeeId)
    {
        var records = await _context.PayrollRecords
            .Include(r => r.Details)
            .Where(r => r.EmployeeId == employeeId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return records.Select(MapPayrollRecordToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<PayrollRecordDto> RecalculatePayrollRecordAsync(Guid id)
    {
        _logger.LogInformation("Recalculating payroll record: {RecordId}", id);

        var record = await _context.PayrollRecords
            .Include(r => r.Employee)
            .Include(r => r.PayPeriod)
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (record == null)
        {
            throw new InvalidOperationException($"Payroll record with ID {id} not found");
        }

        if (record.Status == PayrollStatus.Approved)
        {
            throw new InvalidOperationException("Cannot recalculate approved payroll");
        }

        // Remove old details
        _context.PayrollRecordDetails.RemoveRange(record.Details);

        // Recalculate
        await CalculatePayrollRecordAsync(record.Employee, record.PayPeriod, record);

        await _context.SaveChangesAsync();

        // Run compliance checks
        await _complianceEngine.EvaluatePayrollAsync(record.Id);

        _logger.LogInformation("Payroll record recalculated: {RecordId}", id);

        return MapPayrollRecordToDto(record);
    }

    private async Task<PayrollRecord> ProcessEmployeePayrollAsync(Employee employee, PayPeriod payPeriod)
    {
        // Check if record already exists
        var existingRecord = await _context.PayrollRecords
            .FirstOrDefaultAsync(r => r.EmployeeId == employee.Id &&
                                    r.PayPeriodId == payPeriod.Id &&
                                    !r.IsDeleted);

        if (existingRecord != null)
        {
            return existingRecord;
        }

        var record = new PayrollRecord
        {
            PayPeriodId = payPeriod.Id,
            EmployeeId = employee.Id,
            Status = PayrollStatus.Calculated
        };

        _context.PayrollRecords.Add(record);
        await _context.SaveChangesAsync(); // Save to get ID

        await CalculatePayrollRecordAsync(employee, payPeriod, record);

        await _context.SaveChangesAsync();

        // Run compliance checks
        await _complianceEngine.EvaluatePayrollAsync(record.Id);

        return record;
    }

    private async Task CalculatePayrollRecordAsync(Employee employee, PayPeriod payPeriod, PayrollRecord record)
    {
        // Get approved timesheets for the pay period
        var timesheets = await _context.Timesheets
            .Where(t => t.EmployeeId == employee.Id &&
                       t.WorkDate >= payPeriod.StartDate &&
                       t.WorkDate <= payPeriod.EndDate &&
                       t.Status == TimesheetStatus.Approved &&
                       !t.IsDeleted)
            .ToListAsync();

        // Calculate hours
        record.RegularHours = timesheets.Sum(t => t.RegularHours);
        record.OvertimeHours = timesheets.Sum(t => t.OvertimeHours);
        record.DoubleTimeHours = timesheets.Sum(t => t.DoubleTimeHours);

        // Calculate earnings based on employee classification
        if (employee.Classification == EmployeeClassification.NonExempt && employee.HourlyRate.HasValue)
        {
            var rate = employee.HourlyRate.Value;
            record.RegularPay = record.RegularHours * rate;
            record.OvertimePay = record.OvertimeHours * rate * 1.5m;
            record.DoubleTimePay = record.DoubleTimeHours * rate * 2m;
        }
        else if (employee.Classification == EmployeeClassification.Exempt && employee.SalaryAmount.HasValue)
        {
            // Calculate salary for the pay period
            var payPeriods = GetPayPeriodsPerYear(employee.PayFrequency);
            record.RegularPay = employee.SalaryAmount.Value / payPeriods;
        }

        record.GrossPay = record.RegularPay + record.OvertimePay + record.DoubleTimePay +
                         record.BonusPay + record.CommissionPay;

        // Get YTD totals
        var ytdTotals = await GetYTDTotalsAsync(employee.Id, payPeriod.StartDate);

        // Calculate pre-tax deductions
        var preTaxDeductions = await CalculateDeductionsAsync(employee.Id, record.GrossPay, true);
        record.PreTaxDeductions = preTaxDeductions.Sum(d => d.Amount);

        // Calculate taxable gross (gross - pre-tax deductions)
        var taxableGross = record.GrossPay - record.PreTaxDeductions;

        // Calculate taxes
        var taxRequest = new TaxCalculationRequest
        {
            EmployeeId = employee.Id,
            GrossPay = taxableGross,
            PayPeriods = GetPayPeriodsPerYear(employee.PayFrequency),
            YTDGrossPay = ytdTotals.GrossPay,
            YTDFederalTax = ytdTotals.FederalTax,
            YTDStateTax = ytdTotals.StateTax,
            YTDSocialSecurity = ytdTotals.SocialSecurity,
            YTDMedicare = ytdTotals.Medicare
        };

        var taxResult = await _taxCalculationService.CalculateTaxesAsync(taxRequest);
        record.FederalTax = taxResult.FederalTax;
        record.StateTax = taxResult.StateTax;
        record.SocialSecurityTax = taxResult.SocialSecurityTax;
        record.MedicareTax = taxResult.MedicareTax;
        record.TotalTaxes = taxResult.TotalTaxes;

        // Calculate post-tax deductions
        var postTaxDeductions = await CalculateDeductionsAsync(employee.Id, record.GrossPay, false);
        record.PostTaxDeductions = postTaxDeductions.Sum(d => d.Amount);
        record.TotalDeductions = record.PreTaxDeductions + record.PostTaxDeductions;

        // Calculate net pay
        record.NetPay = record.GrossPay - record.TotalTaxes - record.TotalDeductions;

        // Update YTD totals
        record.YTDGrossPay = ytdTotals.GrossPay + record.GrossPay;
        record.YTDFederalTax = ytdTotals.FederalTax + record.FederalTax;
        record.YTDStateTax = ytdTotals.StateTax + record.StateTax;
        record.YTDSocialSecurity = ytdTotals.SocialSecurity + record.SocialSecurityTax;
        record.YTDMedicare = ytdTotals.Medicare + record.MedicareTax;
        record.YTDNetPay = ytdTotals.NetPay + record.NetPay;

        record.CalculatedAt = DateTime.UtcNow;

        // Create detail records
        await CreatePayrollDetailsAsync(record, preTaxDeductions, postTaxDeductions);
    }

    private async Task CreatePayrollDetailsAsync(PayrollRecord record, List<(string Description, decimal Amount)> preTaxDeductions, List<(string Description, decimal Amount)> postTaxDeductions)
    {
        var details = new List<PayrollRecordDetail>();

        // Earnings
        if (record.RegularPay > 0)
        {
            details.Add(new PayrollRecordDetail
            {
                PayrollRecordId = record.Id,
                Type = PayrollRecordDetailType.Earnings,
                Description = "Regular Pay",
                Amount = record.RegularPay
            });
        }

        if (record.OvertimePay > 0)
        {
            details.Add(new PayrollRecordDetail
            {
                PayrollRecordId = record.Id,
                Type = PayrollRecordDetailType.Earnings,
                Description = "Overtime Pay",
                Amount = record.OvertimePay
            });
        }

        if (record.DoubleTimePay > 0)
        {
            details.Add(new PayrollRecordDetail
            {
                PayrollRecordId = record.Id,
                Type = PayrollRecordDetailType.Earnings,
                Description = "Double Time Pay",
                Amount = record.DoubleTimePay
            });
        }

        // Taxes
        details.Add(new PayrollRecordDetail
        {
            PayrollRecordId = record.Id,
            Type = PayrollRecordDetailType.Tax,
            Description = "Federal Income Tax",
            Amount = record.FederalTax
        });

        details.Add(new PayrollRecordDetail
        {
            PayrollRecordId = record.Id,
            Type = PayrollRecordDetailType.Tax,
            Description = "State Income Tax",
            Amount = record.StateTax
        });

        details.Add(new PayrollRecordDetail
        {
            PayrollRecordId = record.Id,
            Type = PayrollRecordDetailType.Tax,
            Description = "Social Security",
            Amount = record.SocialSecurityTax
        });

        details.Add(new PayrollRecordDetail
        {
            PayrollRecordId = record.Id,
            Type = PayrollRecordDetailType.Tax,
            Description = "Medicare",
            Amount = record.MedicareTax
        });

        // Deductions
        foreach (var deduction in preTaxDeductions)
        {
            details.Add(new PayrollRecordDetail
            {
                PayrollRecordId = record.Id,
                Type = PayrollRecordDetailType.Deduction,
                Description = $"{deduction.Description} (Pre-Tax)",
                Amount = deduction.Amount
            });
        }

        foreach (var deduction in postTaxDeductions)
        {
            details.Add(new PayrollRecordDetail
            {
                PayrollRecordId = record.Id,
                Type = PayrollRecordDetailType.Deduction,
                Description = deduction.Description,
                Amount = deduction.Amount
            });
        }

        await _context.PayrollRecordDetails.AddRangeAsync(details);
    }

    private async Task<List<(string Description, decimal Amount)>> CalculateDeductionsAsync(Guid employeeId, decimal grossPay, bool isPreTax)
    {
        var deductions = await _context.Deductions
            .Where(d => d.EmployeeId == employeeId &&
                       d.IsActive &&
                       d.IsPreTax == isPreTax &&
                       !d.IsDeleted)
            .OrderBy(d => d.Priority)
            .ToListAsync();

        var result = new List<(string Description, decimal Amount)>();

        foreach (var deduction in deductions)
        {
            var amount = deduction.IsPercentage
                ? grossPay * (deduction.Amount / 100m)
                : deduction.Amount;

            result.Add((deduction.Description, Math.Round(amount, 2)));
        }

        return result;
    }

    private async Task<(decimal GrossPay, decimal FederalTax, decimal StateTax, decimal SocialSecurity, decimal Medicare, decimal NetPay)>
        GetYTDTotalsAsync(Guid employeeId, DateTime periodStartDate)
    {
        var yearStart = new DateTime(periodStartDate.Year, 1, 1);

        var totals = await _context.PayrollRecords
            .Where(r => r.EmployeeId == employeeId &&
                       r.Status == PayrollStatus.Approved &&
                       r.CreatedAt >= yearStart &&
                       r.CreatedAt < periodStartDate &&
                       !r.IsDeleted)
            .GroupBy(r => r.EmployeeId)
            .Select(g => new
            {
                GrossPay = g.Sum(r => r.GrossPay),
                FederalTax = g.Sum(r => r.FederalTax),
                StateTax = g.Sum(r => r.StateTax),
                SocialSecurity = g.Sum(r => r.SocialSecurityTax),
                Medicare = g.Sum(r => r.MedicareTax),
                NetPay = g.Sum(r => r.NetPay)
            })
            .FirstOrDefaultAsync();

        return totals != null
            ? (totals.GrossPay, totals.FederalTax, totals.StateTax, totals.SocialSecurity, totals.Medicare, totals.NetPay)
            : (0, 0, 0, 0, 0, 0);
    }

    private int GetPayPeriodsPerYear(PayFrequency frequency)
    {
        return frequency switch
        {
            PayFrequency.Weekly => 52,
            PayFrequency.BiWeekly => 26,
            PayFrequency.SemiMonthly => 24,
            PayFrequency.Monthly => 12,
            _ => 26
        };
    }

    private PayPeriodDto MapPayPeriodToDto(PayPeriod payPeriod)
    {
        return new PayPeriodDto
        {
            Id = payPeriod.Id,
            CompanyId = payPeriod.CompanyId,
            StartDate = payPeriod.StartDate,
            EndDate = payPeriod.EndDate,
            PayDate = payPeriod.PayDate,
            Status = payPeriod.Status,
            Frequency = payPeriod.Frequency,
            TotalGrossPay = payPeriod.TotalGrossPay,
            TotalNetPay = payPeriod.TotalNetPay,
            TotalTaxes = payPeriod.TotalTaxes,
            TotalDeductions = payPeriod.TotalDeductions,
            ProcessedAt = payPeriod.ProcessedAt,
            ApprovedAt = payPeriod.ApprovedAt,
            ApprovedById = payPeriod.ApprovedById,
            CreatedAt = payPeriod.CreatedAt,
            UpdatedAt = payPeriod.UpdatedAt
        };
    }

    private PayrollRecordDto MapPayrollRecordToDto(PayrollRecord record)
    {
        return new PayrollRecordDto
        {
            Id = record.Id,
            PayPeriodId = record.PayPeriodId,
            EmployeeId = record.EmployeeId,
            Status = record.Status,
            RegularHours = record.RegularHours,
            OvertimeHours = record.OvertimeHours,
            DoubleTimeHours = record.DoubleTimeHours,
            GrossPay = record.GrossPay,
            RegularPay = record.RegularPay,
            OvertimePay = record.OvertimePay,
            DoubleTimePay = record.DoubleTimePay,
            BonusPay = record.BonusPay,
            CommissionPay = record.CommissionPay,
            FederalTax = record.FederalTax,
            StateTax = record.StateTax,
            SocialSecurityTax = record.SocialSecurityTax,
            MedicareTax = record.MedicareTax,
            TotalTaxes = record.TotalTaxes,
            PreTaxDeductions = record.PreTaxDeductions,
            PostTaxDeductions = record.PostTaxDeductions,
            TotalDeductions = record.TotalDeductions,
            NetPay = record.NetPay,
            YTDGrossPay = record.YTDGrossPay,
            YTDFederalTax = record.YTDFederalTax,
            YTDStateTax = record.YTDStateTax,
            YTDSocialSecurity = record.YTDSocialSecurity,
            YTDMedicare = record.YTDMedicare,
            YTDNetPay = record.YTDNetPay,
            CalculatedAt = record.CalculatedAt,
            ApprovedAt = record.ApprovedAt,
            Details = record.Details.Select(d => new PayrollRecordDetailDto
            {
                Id = d.Id,
                Type = d.Type,
                Description = d.Description,
                Amount = d.Amount,
                IsYTD = d.IsYTD
            }).ToList(),
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }
}

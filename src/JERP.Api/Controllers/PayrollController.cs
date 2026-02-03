using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.Payroll;
using JERP.Application.Services.Payroll;
using JERP.Application.Services.Payroll.Pdf;

namespace JERP.Api.Controllers;

/// <summary>
/// Payroll processing and management endpoints
/// </summary>
[Route("api/v1/payroll")]
[Authorize]
public class PayrollController : BaseApiController
{
    private readonly IPayrollService _payrollService;
    private readonly IPdfGenerationService _pdfGenerationService;
    private readonly ILogger<PayrollController> _logger;

    public PayrollController(
        IPayrollService payrollService,
        IPdfGenerationService pdfGenerationService,
        ILogger<PayrollController> logger)
    {
        _payrollService = payrollService;
        _pdfGenerationService = pdfGenerationService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new pay period
    /// </summary>
    [HttpPost("periods")]
    public async Task<IActionResult> CreatePayPeriod([FromBody] CreatePayPeriodRequest request)
    {
        var payPeriod = await _payrollService.CreatePayPeriodAsync(request);
        _logger.LogInformation("Pay period created: {PayPeriodId}", payPeriod.Id);
        return Created(payPeriod);
    }

    /// <summary>
    /// Get pay period by ID
    /// </summary>
    [HttpGet("periods/{id}")]
    public async Task<IActionResult> GetPayPeriod(int id)
    {
        var payPeriod = await _payrollService.GetPayPeriodByIdAsync(id);
        
        if (payPeriod == null)
        {
            return NotFound($"Pay period with ID {id} not found");
        }

        return Ok(payPeriod);
    }

    /// <summary>
    /// Get pay periods by year
    /// </summary>
    [HttpGet("periods/year/{year}")]
    public async Task<IActionResult> GetPayPeriodsByYear(int year, [FromQuery] int? companyId = null)
    {
        var payPeriods = await _payrollService.GetPayPeriodsByYearAsync(year, companyId);
        return Ok(payPeriods);
    }

    /// <summary>
    /// Process payroll for a pay period
    /// </summary>
    [HttpPost("periods/{id}/process")]
    public async Task<IActionResult> ProcessPayroll(int id)
    {
        var result = await _payrollService.ProcessPayrollAsync(id);
        
        if (result == null)
        {
            return NotFound($"Pay period with ID {id} not found");
        }

        _logger.LogInformation("Payroll processed for period {PayPeriodId}: {RecordsProcessed} records", 
            id, result.RecordsProcessed);
        
        return Ok(result);
    }

    /// <summary>
    /// Approve payroll for a pay period
    /// </summary>
    [HttpPost("periods/{id}/approve")]
    public async Task<IActionResult> ApprovePayroll(int id)
    {
        var payPeriod = await _payrollService.ApprovePayrollAsync(id);
        
        if (payPeriod == null)
        {
            return NotFound($"Pay period with ID {id} not found");
        }

        _logger.LogInformation("Payroll approved for period {PayPeriodId}", id);
        return Ok(payPeriod);
    }

    /// <summary>
    /// Get payroll record by ID
    /// </summary>
    [HttpGet("records/{id}")]
    public async Task<IActionResult> GetPayrollRecord(int id)
    {
        var record = await _payrollService.GetPayrollRecordByIdAsync(id);
        
        if (record == null)
        {
            return NotFound($"Payroll record with ID {id} not found");
        }

        return Ok(record);
    }

    /// <summary>
    /// Get payroll records for an employee
    /// </summary>
    [HttpGet("records/employee/{employeeId}")]
    public async Task<IActionResult> GetEmployeeRecords(int employeeId)
    {
        var records = await _payrollService.GetEmployeePayrollRecordsAsync(employeeId);
        return Ok(records);
    }

    /// <summary>
    /// Recalculate a payroll record
    /// </summary>
    [HttpPost("records/{id}/recalculate")]
    public async Task<IActionResult> RecalculateRecord(int id)
    {
        var record = await _payrollService.RecalculatePayrollRecordAsync(id);
        
        if (record == null)
        {
            return NotFound($"Payroll record with ID {id} not found");
        }

        _logger.LogInformation("Payroll record recalculated: {RecordId}", id);
        return Ok(record);
    }

    /// <summary>
    /// Download pay stub as PDF
    /// </summary>
    [HttpGet("records/{id}/paystub")]
    public async Task<IActionResult> GetPayStub(Guid id)
    {
        var pdfBytes = await _pdfGenerationService.GeneratePayStubAsync(id);
        
        if (pdfBytes == null)
        {
            return NotFound($"Payroll record with ID {id} not found");
        }

        _logger.LogInformation("Pay stub generated for record {RecordId}", id);
        
        return File(pdfBytes, "application/pdf", $"paystub_{id}.pdf");
    }

    /// <summary>
    /// Download all pay stubs for a pay period as a single PDF
    /// </summary>
    [HttpGet("periods/{id}/report")]
    public async Task<IActionResult> GetPayPeriodReport(Guid id)
    {
        var pdfBytes = await _pdfGenerationService.GenerateBulkPayStubsAsync(id);
        
        if (pdfBytes == null)
        {
            return NotFound($"Pay period with ID {id} not found");
        }

        _logger.LogInformation("Pay period report generated for period {PayPeriodId}", id);
        
        return File(pdfBytes, "application/pdf", $"payroll_report_{id}.pdf");
    }
}

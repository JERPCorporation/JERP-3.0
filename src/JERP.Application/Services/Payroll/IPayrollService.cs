using JERP.Application.DTOs.Payroll;

namespace JERP.Application.Services.Payroll;

/// <summary>
/// Interface for payroll processing services
/// </summary>
public interface IPayrollService
{
    /// <summary>
    /// Creates a new pay period
    /// </summary>
    Task<PayPeriodDto> CreatePayPeriodAsync(CreatePayPeriodRequest request);

    /// <summary>
    /// Gets a pay period by ID
    /// </summary>
    Task<PayPeriodDto> GetPayPeriodByIdAsync(Guid id);

    /// <summary>
    /// Gets all pay periods for a year
    /// </summary>
    Task<List<PayPeriodDto>> GetPayPeriodsByYearAsync(int year, Guid companyId);

    /// <summary>
    /// Processes payroll for a pay period
    /// </summary>
    Task<PayrollProcessingResult> ProcessPayrollAsync(Guid payPeriodId);

    /// <summary>
    /// Approves a processed payroll
    /// </summary>
    Task<PayPeriodDto> ApprovePayrollAsync(Guid payPeriodId, Guid approverId);

    /// <summary>
    /// Gets a payroll record by ID
    /// </summary>
    Task<PayrollRecordDto> GetPayrollRecordAsync(Guid id);

    /// <summary>
    /// Gets all payroll records for an employee
    /// </summary>
    Task<List<PayrollRecordDto>> GetEmployeePayrollRecordsAsync(Guid employeeId);

    /// <summary>
    /// Recalculates a payroll record
    /// </summary>
    Task<PayrollRecordDto> RecalculatePayrollRecordAsync(Guid id);
}

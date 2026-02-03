using JERP.Application.DTOs.Employees;
using JERP.Compliance.Services;
using JERP.Core.Entities;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Employees;

/// <summary>
/// Implementation of employee management services
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly JerpDbContext _context;
    private readonly IComplianceEngine _complianceEngine;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        JerpDbContext context,
        IComplianceEngine complianceEngine,
        ILogger<EmployeeService> logger)
    {
        _context = context;
        _complianceEngine = complianceEngine;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto> GetByIdAsync(Guid id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {id} not found");
        }

        return MapToDto(employee);
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto> GetByEmployeeNumberAsync(string employeeNumber)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber && !e.IsDeleted);

        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with number {employeeNumber} not found");
        }

        return MapToDto(employee);
    }

    /// <inheritdoc/>
    public async Task<EmployeeListResponse> GetAllAsync(int page, int pageSize, string? searchTerm = null)
    {
        var query = _context.Employees.Where(e => !e.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(e =>
                e.FirstName.ToLower().Contains(searchTerm) ||
                e.LastName.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.EmployeeNumber.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var employees = await query
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new EmployeeListResponse
        {
            Items = employees.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto> CreateAsync(EmployeeCreateRequest request)
    {
        _logger.LogInformation("Creating employee: {EmployeeNumber}", request.EmployeeNumber);

        // Check for duplicate employee number
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber);

        if (existingEmployee != null)
        {
            throw new InvalidOperationException($"Employee number {request.EmployeeNumber} already exists");
        }

        // Validate pay configuration
        ValidatePayConfiguration(request.Classification, request.HourlyRate, request.SalaryAmount);

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            SSN = request.SSN,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            EmployeeNumber = request.EmployeeNumber,
            CompanyId = request.CompanyId,
            DepartmentId = request.DepartmentId,
            ManagerId = request.ManagerId,
            HireDate = request.HireDate,
            Status = EmployeeStatus.Active,
            EmploymentType = request.EmploymentType,
            Classification = request.Classification,
            HourlyRate = request.HourlyRate,
            SalaryAmount = request.SalaryAmount,
            PayFrequency = request.PayFrequency
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        // Run compliance checks
        var violations = await _complianceEngine.EvaluateEmployeeAsync(employee.Id);
        if (violations.Any())
        {
            _logger.LogWarning("Compliance violations detected for employee {EmployeeNumber}: {Count} violations",
                request.EmployeeNumber, violations.Count);
        }

        _logger.LogInformation("Employee created successfully: {EmployeeNumber}", request.EmployeeNumber);

        return MapToDto(employee);
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto> UpdateAsync(Guid id, EmployeeUpdateRequest request)
    {
        _logger.LogInformation("Updating employee: {EmployeeId}", id);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {id} not found");
        }

        // Validate pay configuration
        ValidatePayConfiguration(request.Classification, request.HourlyRate, request.SalaryAmount);

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.DateOfBirth = request.DateOfBirth;
        employee.Address = request.Address;
        employee.City = request.City;
        employee.State = request.State;
        employee.ZipCode = request.ZipCode;
        employee.DepartmentId = request.DepartmentId;
        employee.ManagerId = request.ManagerId;
        employee.Status = request.Status;
        employee.EmploymentType = request.EmploymentType;
        employee.Classification = request.Classification;
        employee.HourlyRate = request.HourlyRate;
        employee.SalaryAmount = request.SalaryAmount;
        employee.PayFrequency = request.PayFrequency;

        await _context.SaveChangesAsync();

        // Run compliance checks
        var violations = await _complianceEngine.EvaluateEmployeeAsync(employee.Id);
        if (violations.Any())
        {
            _logger.LogWarning("Compliance violations detected for employee {EmployeeId}: {Count} violations",
                id, violations.Count);
        }

        _logger.LogInformation("Employee updated successfully: {EmployeeId}", id);

        return MapToDto(employee);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting employee: {EmployeeId}", id);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {id} not found");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee deleted successfully: {EmployeeId}", id);
    }

    /// <inheritdoc/>
    public async Task<EmployeeDto> TerminateAsync(Guid id, DateTime terminationDate, string reason)
    {
        _logger.LogInformation("Terminating employee: {EmployeeId}", id);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {id} not found");
        }

        employee.Status = EmployeeStatus.Terminated;
        employee.TerminationDate = terminationDate;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Employee terminated successfully: {EmployeeId}, Reason: {Reason}", id, reason);

        return MapToDto(employee);
    }

    private void ValidatePayConfiguration(EmployeeClassification classification, decimal? hourlyRate, decimal? salaryAmount)
    {
        if (classification == EmployeeClassification.NonExempt)
        {
            if (!hourlyRate.HasValue || hourlyRate.Value <= 0)
            {
                throw new InvalidOperationException("Non-exempt employees must have an hourly rate");
            }
        }
        else if (classification == EmployeeClassification.Exempt)
        {
            if (!salaryAmount.HasValue || salaryAmount.Value <= 0)
            {
                throw new InvalidOperationException("Exempt employees must have a salary amount");
            }
        }
    }

    private EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            DateOfBirth = employee.DateOfBirth,
            SSN = employee.SSN,
            Address = employee.Address,
            City = employee.City,
            State = employee.State,
            ZipCode = employee.ZipCode,
            EmployeeNumber = employee.EmployeeNumber,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            ManagerId = employee.ManagerId,
            HireDate = employee.HireDate,
            TerminationDate = employee.TerminationDate,
            Status = employee.Status,
            EmploymentType = employee.EmploymentType,
            Classification = employee.Classification,
            HourlyRate = employee.HourlyRate,
            SalaryAmount = employee.SalaryAmount,
            PayFrequency = employee.PayFrequency,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}

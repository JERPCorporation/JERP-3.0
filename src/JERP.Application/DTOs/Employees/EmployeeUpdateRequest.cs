using JERP.Core.Enums;

namespace JERP.Application.DTOs.Employees;

/// <summary>
/// Request to update an existing employee
/// </summary>
public class EmployeeUpdateRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? ManagerId { get; set; }
    public EmployeeStatus Status { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public EmployeeClassification Classification { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? SalaryAmount { get; set; }
    public PayFrequency PayFrequency { get; set; }
}

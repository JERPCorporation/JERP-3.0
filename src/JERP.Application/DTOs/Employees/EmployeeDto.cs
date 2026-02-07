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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Employees;

/// <summary>
/// Employee data transfer object
/// </summary>
public class EmployeeDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? SSN { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public required string EmployeeNumber { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }
    public Guid? ManagerId { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public EmployeeStatus Status { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public EmployeeClassification Classification { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? SalaryAmount { get; set; }
    public PayFrequency PayFrequency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Full name combining first and last name
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
    
    /// <summary>
    /// Formatted hire date for display
    /// </summary>
    public string HireDateDisplay => HireDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Friendly status display
    /// </summary>
    public string StatusDisplay => Status switch
    {
        EmployeeStatus.Active => "Active",
        EmployeeStatus.Inactive => "Inactive",
        EmployeeStatus.OnLeave => "On Leave",
        EmployeeStatus.Terminated => "Terminated",
        _ => "Unknown"
    };
    
    /// <summary>
    /// Formatted hourly rate for display
    /// </summary>
    public string HourlyRateDisplay => HourlyRate.HasValue && HourlyRate > 0 ? HourlyRate.Value.ToString("C2") : "N/A";
    
    /// <summary>
    /// Formatted salary for display
    /// </summary>
    public string SalaryDisplay => SalaryAmount.HasValue && SalaryAmount > 0 ? SalaryAmount.Value.ToString("C2") : "N/A";
    
    // Computed Properties
    /// <summary>
    /// Years of service with the company
    /// </summary>
    public int YearsOfService => (int)((DateTime.Now - HireDate).Days / 365.25);
    
    /// <summary>
    /// Formatted tenure display
    /// </summary>
    public string TenureDisplay => YearsOfService == 1 ? "1 year" : $"{YearsOfService} years";
    
    /// <summary>
    /// Indicates if employee is currently active
    /// </summary>
    public bool IsActive => Status == EmployeeStatus.Active;
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => Status switch
    {
        EmployeeStatus.Active => "✅",
        EmployeeStatus.OnLeave => "⏸️",
        EmployeeStatus.Terminated => "❌",
        EmployeeStatus.Inactive => "⭕",
        _ => ""
    };
}

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

using System.ComponentModel.DataAnnotations;
using JERP.Core.Enums;

namespace JERP.Core.Entities;

/// <summary>
/// Represents an employee in the system
/// </summary>
public class Employee : BaseEntity
{
    // Personal Information
    /// <summary>
    /// Employee's first name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Employee's last name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Employee's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Employee's phone number
    /// </summary>
    [MaxLength(20)]
    public string? Phone { get; set; }

    /// <summary>
    /// Employee's date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Social Security Number (encrypted)
    /// </summary>
    [MaxLength(100)]
    public string? SSN { get; set; }

    /// <summary>
    /// Street address
    /// </summary>
    [MaxLength(200)]
    public string? Address { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [MaxLength(100)]
    public string? City { get; set; }

    /// <summary>
    /// State or province code
    /// </summary>
    [MaxLength(50)]
    public string? State { get; set; }

    /// <summary>
    /// Postal or ZIP code
    /// </summary>
    [MaxLength(20)]
    public string? ZipCode { get; set; }

    // Employment Information
    /// <summary>
    /// Unique employee number or badge ID
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Foreign key to the department
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// Foreign key to the manager (another employee)
    /// </summary>
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// Date the employee was hired
    /// </summary>
    [Required]
    public DateTime HireDate { get; set; }

    /// <summary>
    /// Date the employee was terminated (if applicable)
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// Current employment status
    /// </summary>
    [Required]
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    /// <summary>
    /// Type of employment relationship
    /// </summary>
    [Required]
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

    /// <summary>
    /// FLSA classification
    /// </summary>
    [Required]
    public EmployeeClassification Classification { get; set; } = EmployeeClassification.NonExempt;

    // Pay Information
    /// <summary>
    /// Hourly rate for non-exempt employees
    /// </summary>
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// Annual salary for exempt employees
    /// </summary>
    public decimal? SalaryAmount { get; set; }

    /// <summary>
    /// How frequently the employee is paid
    /// </summary>
    [Required]
    public PayFrequency PayFrequency { get; set; } = PayFrequency.BiWeekly;

    // Navigation properties
    public Company Company { get; set; } = null!;
    public Department? Department { get; set; }
    public Employee? Manager { get; set; }
    public ICollection<Employee> DirectReports { get; set; } = new List<Employee>();
    public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
    public ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
    public ICollection<TaxWithholding> TaxWithholdings { get; set; } = new List<TaxWithholding>();
    public ICollection<Deduction> Deductions { get; set; } = new List<Deduction>();
}

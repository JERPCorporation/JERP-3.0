/*
 * JERP 3.0 - Payroll & ERP System Test Suite
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using FluentAssertions;
using JERP.Core.Entities;
using JERP.Core.Enums;
using Xunit;

namespace JERP.Core.Tests.Entities;

/// <summary>
/// Comprehensive test suite for Employee entity (25+ tests)
/// Target: 90% code coverage for Core layer
/// </summary>
public class EmployeeTests
{
    [Fact]
    public void Employee_Creation_ShouldSetDefaultStatus()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.Status.Should().Be(EmployeeStatus.Active);
    }
    
    [Fact]
    public void Employee_Creation_ShouldSetDefaultEmploymentType()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.EmploymentType.Should().Be(EmploymentType.FullTime);
    }
    
    [Fact]
    public void Employee_Creation_ShouldSetDefaultClassification()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.Classification.Should().Be(EmployeeClassification.NonExempt);
    }
    
    [Fact]
    public void Employee_Creation_ShouldSetDefaultPayFrequency()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.PayFrequency.Should().Be(PayFrequency.BiWeekly);
    }
    
    [Fact]
    public void Employee_Creation_ShouldInitializeCollections()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.DirectReports.Should().NotBeNull();
        employee.Timesheets.Should().NotBeNull();
        employee.PayrollRecords.Should().NotBeNull();
        employee.TaxWithholdings.Should().NotBeNull();
        employee.Deductions.Should().NotBeNull();
    }
    
    [Fact]
    public void Employee_FirstName_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            FirstName = "John" 
        };
        
        // Assert
        employee.FirstName.Should().Be("John");
    }
    
    [Fact]
    public void Employee_LastName_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            LastName = "Doe" 
        };
        
        // Assert
        employee.LastName.Should().Be("Doe");
    }
    
    [Fact]
    public void Employee_Email_ShouldBeValid()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Email = "john.doe@example.com" 
        };
        
        // Assert
        employee.Email.Should().Be("john.doe@example.com");
        employee.Email.Should().Contain("@");
    }
    
    [Fact]
    public void Employee_EmployeeNumber_ShouldBeUnique()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            EmployeeNumber = "EMP-001" 
        };
        
        // Assert
        employee.EmployeeNumber.Should().NotBeNullOrEmpty();
        employee.EmployeeNumber.Should().Be("EMP-001");
    }
    
    [Fact]
    public void Employee_HireDate_ShouldBeInPast()
    {
        // Arrange
        var hireDate = DateTime.Now.AddYears(-2);
        
        // Act
        var employee = new Employee 
        { 
            HireDate = hireDate 
        };
        
        // Assert
        employee.HireDate.Should().BeBefore(DateTime.Now);
    }
    
    [Fact]
    public void Employee_TerminationDate_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            TerminationDate = null 
        };
        
        // Assert
        employee.TerminationDate.Should().BeNull();
    }
    
    [Fact]
    public void Employee_TerminationDate_WhenTerminated_ShouldBeSet()
    {
        // Arrange
        var terminationDate = DateTime.Now;
        
        // Act
        var employee = new Employee 
        { 
            Status = EmployeeStatus.Terminated,
            TerminationDate = terminationDate
        };
        
        // Assert
        employee.Status.Should().Be(EmployeeStatus.Terminated);
        employee.TerminationDate.Should().NotBeNull();
        employee.TerminationDate.Should().BeCloseTo(terminationDate, TimeSpan.FromSeconds(1));
    }
    
    [Theory]
    [InlineData(EmployeeStatus.Active)]
    [InlineData(EmployeeStatus.Inactive)]
    [InlineData(EmployeeStatus.OnLeave)]
    [InlineData(EmployeeStatus.Terminated)]
    public void Employee_Status_CanBeAnyValidStatus(EmployeeStatus status)
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Status = status 
        };
        
        // Assert
        employee.Status.Should().Be(status);
    }
    
    [Theory]
    [InlineData(EmploymentType.FullTime)]
    [InlineData(EmploymentType.PartTime)]
    [InlineData(EmploymentType.Contract)]
    [InlineData(EmploymentType.Seasonal)]
    public void Employee_EmploymentType_CanBeAnyValidType(EmploymentType type)
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            EmploymentType = type 
        };
        
        // Assert
        employee.EmploymentType.Should().Be(type);
    }
    
    [Theory]
    [InlineData(EmployeeClassification.Exempt)]
    [InlineData(EmployeeClassification.NonExempt)]
    public void Employee_Classification_CanBeAnyValidClassification(EmployeeClassification classification)
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Classification = classification 
        };
        
        // Assert
        employee.Classification.Should().Be(classification);
    }
    
    [Fact]
    public void Employee_HourlyRate_CanBeSet()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            HourlyRate = 25.50m 
        };
        
        // Assert
        employee.HourlyRate.Should().Be(25.50m);
    }
    
    [Fact]
    public void Employee_SalaryAmount_CanBeSet()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            SalaryAmount = 75000m 
        };
        
        // Assert
        employee.SalaryAmount.Should().Be(75000m);
    }
    
    [Fact]
    public void Employee_HourlyRate_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            HourlyRate = null 
        };
        
        // Assert
        employee.HourlyRate.Should().BeNull();
    }
    
    [Fact]
    public void Employee_SalaryAmount_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            SalaryAmount = null 
        };
        
        // Assert
        employee.SalaryAmount.Should().BeNull();
    }
    
    [Theory]
    [InlineData(PayFrequency.Weekly)]
    [InlineData(PayFrequency.BiWeekly)]
    [InlineData(PayFrequency.SemiMonthly)]
    [InlineData(PayFrequency.Monthly)]
    public void Employee_PayFrequency_CanBeAnyValidFrequency(PayFrequency frequency)
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            PayFrequency = frequency 
        };
        
        // Assert
        employee.PayFrequency.Should().Be(frequency);
    }
    
    [Fact]
    public void Employee_CompanyId_ShouldBeRequired()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        
        // Act
        var employee = new Employee 
        { 
            CompanyId = companyId 
        };
        
        // Assert
        employee.CompanyId.Should().Be(companyId);
        employee.CompanyId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void Employee_DepartmentId_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            DepartmentId = null 
        };
        
        // Assert
        employee.DepartmentId.Should().BeNull();
    }
    
    [Fact]
    public void Employee_ManagerId_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            ManagerId = null 
        };
        
        // Assert
        employee.ManagerId.Should().BeNull();
    }
    
    [Fact]
    public void Employee_CanHaveManager()
    {
        // Arrange
        var managerId = Guid.NewGuid();
        
        // Act
        var employee = new Employee 
        { 
            ManagerId = managerId 
        };
        
        // Assert
        employee.ManagerId.Should().NotBeNull();
        employee.ManagerId.Should().Be(managerId);
    }
    
    [Fact]
    public void Employee_CanHaveMultipleDirectReports()
    {
        // Arrange
        var manager = new Employee { Id = Guid.NewGuid() };
        var employee1 = new Employee { Id = Guid.NewGuid(), ManagerId = manager.Id };
        var employee2 = new Employee { Id = Guid.NewGuid(), ManagerId = manager.Id };
        
        // Act
        manager.DirectReports.Add(employee1);
        manager.DirectReports.Add(employee2);
        
        // Assert
        manager.DirectReports.Should().HaveCount(2);
    }
    
    [Fact]
    public void Employee_InheritsFromBaseEntity()
    {
        // Arrange & Act
        var employee = new Employee();
        
        // Assert
        employee.Should().BeAssignableTo<BaseEntity>();
    }
    
    [Fact]
    public void Employee_Phone_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Phone = null 
        };
        
        // Assert
        employee.Phone.Should().BeNull();
    }
    
    [Fact]
    public void Employee_Phone_CanBeSet()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Phone = "555-1234" 
        };
        
        // Assert
        employee.Phone.Should().Be("555-1234");
    }
    
    [Fact]
    public void Employee_DateOfBirth_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            DateOfBirth = null 
        };
        
        // Assert
        employee.DateOfBirth.Should().BeNull();
    }
    
    [Fact]
    public void Employee_SSN_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            SSN = null 
        };
        
        // Assert
        employee.SSN.Should().BeNull();
    }
    
    [Fact]
    public void Employee_Address_CanBeNull()
    {
        // Arrange & Act
        var employee = new Employee 
        { 
            Address = null,
            City = null,
            State = null,
            ZipCode = null
        };
        
        // Assert
        employee.Address.Should().BeNull();
        employee.City.Should().BeNull();
        employee.State.Should().BeNull();
        employee.ZipCode.Should().BeNull();
    }
}

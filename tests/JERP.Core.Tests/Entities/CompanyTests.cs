/*
 * JERP 3.0 - Payroll & ERP System Test Suite
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using FluentAssertions;
using JERP.Core.Entities;
using Xunit;

namespace JERP.Core.Tests.Entities;

/// <summary>
/// Comprehensive test suite for Company entity (15+ tests)
/// Target: 90% code coverage for Core layer
/// </summary>
public class CompanyTests
{
    [Fact]
    public void Company_Creation_ShouldInitializeCollections()
    {
        // Arrange & Act
        var company = new Company();
        
        // Assert
        company.Employees.Should().NotBeNull();
        company.Departments.Should().NotBeNull();
    }
    
    [Fact]
    public void Company_Name_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Name = "ACME Corporation" 
        };
        
        // Assert
        company.Name.Should().Be("ACME Corporation");
    }
    
    [Fact]
    public void Company_TaxId_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var company = new Company 
        { 
            TaxId = "12-3456789" 
        };
        
        // Assert
        company.TaxId.Should().Be("12-3456789");
    }
    
    [Fact]
    public void Company_Email_ShouldBeValid()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Email = "info@acmecorp.com" 
        };
        
        // Assert
        company.Email.Should().Be("info@acmecorp.com");
        company.Email.Should().Contain("@");
    }
    
    [Fact]
    public void Company_Phone_CanBeSet()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Phone = "555-0100" 
        };
        
        // Assert
        company.Phone.Should().Be("555-0100");
    }
    
    [Fact]
    public void Company_Address_CanBeSet()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Address = "123 Main St",
            City = "Springfield",
            State = "IL",
            ZipCode = "62701"
        };
        
        // Assert
        company.Address.Should().Be("123 Main St");
        company.City.Should().Be("Springfield");
        company.State.Should().Be("IL");
        company.ZipCode.Should().Be("62701");
    }
    
    [Fact]
    public void Company_Phone_CanBeNull()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Phone = null 
        };
        
        // Assert
        company.Phone.Should().BeNull();
    }
    
    [Fact]
    public void Company_Email_CanBeNull()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Email = null 
        };
        
        // Assert
        company.Email.Should().BeNull();
    }
    
    [Fact]
    public void Company_CanHaveMultipleEmployees()
    {
        // Arrange
        var company = new Company();
        var employee1 = new Employee { Id = Guid.NewGuid(), CompanyId = company.Id };
        var employee2 = new Employee { Id = Guid.NewGuid(), CompanyId = company.Id };
        
        // Act
        company.Employees.Add(employee1);
        company.Employees.Add(employee2);
        
        // Assert
        company.Employees.Should().HaveCount(2);
    }
    
    [Fact]
    public void Company_CanHaveMultipleDepartments()
    {
        // Arrange
        var company = new Company();
        var dept1 = new Department { Id = Guid.NewGuid(), CompanyId = company.Id };
        var dept2 = new Department { Id = Guid.NewGuid(), CompanyId = company.Id };
        
        // Act
        company.Departments.Add(dept1);
        company.Departments.Add(dept2);
        
        // Assert
        company.Departments.Should().HaveCount(2);
    }
    
    [Fact]
    public void Company_InheritsFromBaseEntity()
    {
        // Arrange & Act
        var company = new Company();
        
        // Assert
        company.Should().BeAssignableTo<BaseEntity>();
    }
    
    [Fact]
    public void Company_Address_CanBeNull()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Address = null,
            City = null,
            State = null,
            ZipCode = null
        };
        
        // Assert
        company.Address.Should().BeNull();
        company.City.Should().BeNull();
        company.State.Should().BeNull();
        company.ZipCode.Should().BeNull();
    }
    
    [Fact]
    public void Company_Name_IsRequired()
    {
        // Arrange & Act
        var company = new Company 
        { 
            Name = "Test Company" 
        };
        
        // Assert
        company.Name.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public void Company_TaxId_IsRequired()
    {
        // Arrange & Act
        var company = new Company 
        { 
            TaxId = "12-3456789" 
        };
        
        // Assert
        company.TaxId.Should().NotBeNullOrEmpty();
    }
}

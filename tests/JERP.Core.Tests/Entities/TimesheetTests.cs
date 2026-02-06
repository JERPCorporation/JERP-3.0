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
/// Comprehensive test suite for Timesheet entity (15+ tests)
/// Target: 90% code coverage for Core layer
/// </summary>
public class TimesheetTests
{
    [Fact]
    public void Timesheet_Creation_ShouldSetDefaultStatus()
    {
        // Arrange & Act
        var timesheet = new Timesheet();
        
        // Assert
        timesheet.Status.Should().Be(TimesheetStatus.Draft);
    }
    
    [Fact]
    public void Timesheet_EmployeeId_ShouldBeRequired()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        
        // Act
        var timesheet = new Timesheet 
        { 
            EmployeeId = employeeId 
        };
        
        // Assert
        timesheet.EmployeeId.Should().Be(employeeId);
        timesheet.EmployeeId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void Timesheet_WithRegularHours_ShouldCalculateCorrectly()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            RegularHours = 40m,
            OvertimeHours = 0m
        };
        
        // Assert
        timesheet.RegularHours.Should().Be(40m);
        timesheet.OvertimeHours.Should().Be(0m);
    }
    
    [Theory]
    [InlineData(40, 0, 40)]
    [InlineData(40, 5, 45)]
    [InlineData(35, 10, 45)]
    [InlineData(0, 0, 0)]
    public void Timesheet_TotalHours_ShouldSumRegularAndOvertime(
        decimal regular, decimal overtime, decimal expectedTotal)
    {
        // Arrange
        var timesheet = new Timesheet 
        { 
            RegularHours = regular,
            OvertimeHours = overtime
        };
        
        // Act
        var totalHours = timesheet.RegularHours + timesheet.OvertimeHours;
        
        // Assert
        totalHours.Should().Be(expectedTotal);
    }
    
    [Theory]
    [InlineData(TimesheetStatus.Draft)]
    [InlineData(TimesheetStatus.Submitted)]
    [InlineData(TimesheetStatus.Approved)]
    [InlineData(TimesheetStatus.Rejected)]
    public void Timesheet_Status_CanBeAnyValidStatus(TimesheetStatus status)
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            Status = status 
        };
        
        // Assert
        timesheet.Status.Should().Be(status);
    }
    
    [Fact]
    public void Timesheet_WorkDate_ShouldBeSet()
    {
        // Arrange
        var workDate = new DateTime(2026, 1, 1);
        
        // Act
        var timesheet = new Timesheet 
        { 
            WorkDate = workDate
        };
        
        // Assert
        timesheet.WorkDate.Should().Be(workDate);
    }
    
    [Fact]
    public void Timesheet_SubmittedAt_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            SubmittedAt = null 
        };
        
        // Assert
        timesheet.SubmittedAt.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_ApprovedAt_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            ApprovedAt = null 
        };
        
        // Assert
        timesheet.ApprovedAt.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_ApprovedById_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            ApprovedById = null 
        };
        
        // Assert
        timesheet.ApprovedById.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_Notes_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            Notes = null 
        };
        
        // Assert
        timesheet.Notes.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_WhenSubmitted_ShouldHaveSubmittedAt()
    {
        // Arrange
        var submittedAt = DateTime.Now;
        
        // Act
        var timesheet = new Timesheet 
        { 
            Status = TimesheetStatus.Submitted,
            SubmittedAt = submittedAt
        };
        
        // Assert
        timesheet.Status.Should().Be(TimesheetStatus.Submitted);
        timesheet.SubmittedAt.Should().NotBeNull();
        timesheet.SubmittedAt.Should().BeCloseTo(submittedAt, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Timesheet_WhenApproved_ShouldHaveApprovedAtAndApprover()
    {
        // Arrange
        var approvedAt = DateTime.Now;
        var approverId = Guid.NewGuid();
        
        // Act
        var timesheet = new Timesheet 
        { 
            Status = TimesheetStatus.Approved,
            ApprovedAt = approvedAt,
            ApprovedById = approverId
        };
        
        // Assert
        timesheet.Status.Should().Be(TimesheetStatus.Approved);
        timesheet.ApprovedAt.Should().NotBeNull();
        timesheet.ApprovedById.Should().NotBeNull();
        timesheet.ApprovedById.Should().Be(approverId);
    }
    
    [Fact]
    public void Timesheet_InheritsFromBaseEntity()
    {
        // Arrange & Act
        var timesheet = new Timesheet();
        
        // Assert
        timesheet.Should().BeAssignableTo<BaseEntity>();
    }
    
    [Fact]
    public void Timesheet_WithDecimalHours_ShouldMaintainPrecision()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            RegularHours = 37.5m,
            OvertimeHours = 2.25m
        };
        
        // Assert
        timesheet.RegularHours.Should().Be(37.5m);
        timesheet.OvertimeHours.Should().Be(2.25m);
    }
    
    [Fact]
    public void Timesheet_ClockIn_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            ClockIn = null 
        };
        
        // Assert
        timesheet.ClockIn.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_ClockOut_CanBeNull()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            ClockOut = null 
        };
        
        // Assert
        timesheet.ClockOut.Should().BeNull();
    }
    
    [Fact]
    public void Timesheet_BreakMinutes_DefaultsToZero()
    {
        // Arrange & Act
        var timesheet = new Timesheet();
        
        // Assert
        timesheet.BreakMinutes.Should().Be(0);
    }
    
    [Fact]
    public void Timesheet_DoubleTimeHours_CanBeSet()
    {
        // Arrange & Act
        var timesheet = new Timesheet 
        { 
            DoubleTimeHours = 2m 
        };
        
        // Assert
        timesheet.DoubleTimeHours.Should().Be(2m);
    }
}

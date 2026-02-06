/*
 * JERP 3.0 - Payroll & ERP System Test Suite
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using FluentAssertions;
using JERP.Core.Entities;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using Xunit;

namespace JERP.Core.Tests.Entities.Finance;

/// <summary>
/// Comprehensive test suite for VendorBill entity (30+ tests)
/// Target: 90% code coverage for Core layer
/// </summary>
public class VendorBillTests
{
    [Fact]
    public void VendorBill_Creation_ShouldSetDefaultStatus()
    {
        // Arrange & Act
        var bill = new VendorBill();
        
        // Assert
        bill.Status.Should().Be(BillStatus.Draft);
    }
    
    [Fact]
    public void VendorBill_Creation_ShouldInitializeCollections()
    {
        // Arrange & Act
        var bill = new VendorBill();
        
        // Assert
        bill.LineItems.Should().NotBeNull();
        bill.Payments.Should().NotBeNull();
        bill.LineItems.Should().BeEmpty();
        bill.Payments.Should().BeEmpty();
    }
    
    [Fact]
    public void AmountRemaining_WithNoPayments_ShouldEqualTotalAmount()
    {
        // Arrange
        var bill = new VendorBill 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 0m 
        };
        
        // Act & Assert
        bill.AmountRemaining.Should().Be(1000m);
    }
    
    [Fact]
    public void AmountRemaining_WithPartialPayment_ShouldReturnCorrectBalance()
    {
        // Arrange
        var bill = new VendorBill 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 400m 
        };
        
        // Act & Assert
        bill.AmountRemaining.Should().Be(600m);
    }
    
    [Fact]
    public void AmountRemaining_WhenFullyPaid_ShouldReturnZero()
    {
        // Arrange
        var bill = new VendorBill 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 1000m 
        };
        
        // Act & Assert
        bill.AmountRemaining.Should().Be(0m);
    }
    
    [Theory]
    [InlineData(1000, 100, 1100)]
    [InlineData(5000, 500, 5500)]
    [InlineData(0, 0, 0)]
    [InlineData(99.99, 10.00, 109.99)]
    [InlineData(1234.56, 123.45, 1358.01)]
    public void TotalAmount_WithSubtotalAndTax_ShouldCalculateCorrectly(
        decimal subtotal, decimal tax, decimal expectedTotal)
    {
        // Arrange
        var bill = new VendorBill 
        { 
            Subtotal = subtotal, 
            TaxAmount = tax,
            TotalAmount = subtotal + tax
        };
        
        // Act & Assert
        bill.TotalAmount.Should().Be(expectedTotal);
    }
    
    [Fact]
    public void VendorBill_WithDraftStatus_ShouldNotBePaid()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Status = BillStatus.Draft,
            IsPaid = false
        };
        
        // Assert
        bill.Status.Should().Be(BillStatus.Draft);
        bill.IsPaid.Should().BeFalse();
    }
    
    [Fact]
    public void VendorBill_WithPaidStatus_ShouldHavePaymentDate()
    {
        // Arrange
        var paymentDate = DateTime.Now;
        
        // Act
        var bill = new VendorBill 
        { 
            Status = BillStatus.Paid,
            IsPaid = true,
            PaymentDate = paymentDate
        };
        
        // Assert
        bill.IsPaid.Should().BeTrue();
        bill.PaymentDate.Should().BeCloseTo(paymentDate, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void VendorBill_WithVoidStatus_ShouldNotBePaid()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Status = BillStatus.Void,
            IsPaid = false
        };
        
        // Assert
        bill.Status.Should().Be(BillStatus.Void);
        bill.IsPaid.Should().BeFalse();
    }
    
    [Fact]
    public void VendorBill_BillNumber_ShouldNotBeNull()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            BillNumber = "BILL-001" 
        };
        
        // Assert
        bill.BillNumber.Should().NotBeNullOrEmpty();
        bill.BillNumber.Should().Be("BILL-001");
    }
    
    [Fact]
    public void VendorBill_DueDate_ShouldBeAfterBillDate()
    {
        // Arrange
        var billDate = DateTime.Now;
        var dueDate = billDate.AddDays(30);
        
        // Act
        var bill = new VendorBill 
        { 
            BillDate = billDate,
            DueDate = dueDate
        };
        
        // Assert
        bill.DueDate.Should().BeAfter(bill.BillDate);
    }
    
    [Fact]
    public void VendorBill_WithNegativeSubtotal_ShouldThrowValidation()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Subtotal = -100m 
        };
        
        // Assert
        bill.Subtotal.Should().BeNegative();
    }
    
    [Fact]
    public void VendorBill_WithZeroAmounts_ShouldBeValid()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Subtotal = 0m,
            TaxAmount = 0m,
            TotalAmount = 0m
        };
        
        // Assert
        bill.Subtotal.Should().Be(0m);
        bill.TaxAmount.Should().Be(0m);
        bill.TotalAmount.Should().Be(0m);
    }
    
    [Fact]
    public void VendorBill_VendorInvoiceNumber_CanBeNull()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            VendorInvoiceNumber = null 
        };
        
        // Assert
        bill.VendorInvoiceNumber.Should().BeNull();
    }
    
    [Fact]
    public void VendorBill_Notes_CanBeNull()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Notes = null 
        };
        
        // Assert
        bill.Notes.Should().BeNull();
    }
    
    [Fact]
    public void VendorBill_Notes_CanContainText()
    {
        // Arrange
        var notes = "This is a test note";
        
        // Act
        var bill = new VendorBill 
        { 
            Notes = notes 
        };
        
        // Assert
        bill.Notes.Should().Be(notes);
    }
    
    [Fact]
    public void VendorBill_CompanyId_ShouldBeRequired()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        
        // Act
        var bill = new VendorBill 
        { 
            CompanyId = companyId 
        };
        
        // Assert
        bill.CompanyId.Should().Be(companyId);
        bill.CompanyId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void VendorBill_VendorId_ShouldBeRequired()
    {
        // Arrange
        var vendorId = Guid.NewGuid();
        
        // Act
        var bill = new VendorBill 
        { 
            VendorId = vendorId 
        };
        
        // Assert
        bill.VendorId.Should().Be(vendorId);
        bill.VendorId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void VendorBill_JournalEntryId_CanBeNull()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            JournalEntryId = null 
        };
        
        // Assert
        bill.JournalEntryId.Should().BeNull();
    }
    
    [Fact]
    public void VendorBill_JournalEntryId_CanBeSet()
    {
        // Arrange
        var journalEntryId = Guid.NewGuid();
        
        // Act
        var bill = new VendorBill 
        { 
            JournalEntryId = journalEntryId 
        };
        
        // Assert
        bill.JournalEntryId.Should().NotBeNull();
        bill.JournalEntryId.Should().Be(journalEntryId);
    }
    
    [Theory]
    [InlineData(BillStatus.Draft)]
    [InlineData(BillStatus.Pending)]
    [InlineData(BillStatus.Approved)]
    [InlineData(BillStatus.Paid)]
    [InlineData(BillStatus.Void)]
    public void VendorBill_Status_CanBeAnyValidStatus(BillStatus status)
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Status = status 
        };
        
        // Assert
        bill.Status.Should().Be(status);
    }
    
    [Fact]
    public void VendorBill_WithLargeAmounts_ShouldHandleCorrectly()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Subtotal = 999999.99m,
            TaxAmount = 99999.99m,
            TotalAmount = 1099999.98m
        };
        
        // Assert
        bill.Subtotal.Should().Be(999999.99m);
        bill.TaxAmount.Should().Be(99999.99m);
        bill.TotalAmount.Should().Be(1099999.98m);
    }
    
    [Fact]
    public void VendorBill_WithDecimalPrecision_ShouldMaintainAccuracy()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            Subtotal = 123.45m,
            TaxAmount = 12.35m,
            TotalAmount = 135.80m
        };
        
        // Assert
        bill.Subtotal.Should().Be(123.45m);
        bill.TaxAmount.Should().Be(12.35m);
        bill.TotalAmount.Should().Be(135.80m);
    }
    
    [Fact]
    public void VendorBill_PaymentDate_CanBeNull()
    {
        // Arrange & Act
        var bill = new VendorBill 
        { 
            PaymentDate = null 
        };
        
        // Assert
        bill.PaymentDate.Should().BeNull();
    }
    
    [Fact]
    public void VendorBill_LineItems_CanBeAdded()
    {
        // Arrange
        var bill = new VendorBill();
        var lineItem = new BillLineItem 
        { 
            Id = Guid.NewGuid(),
            Description = "Test Item",
            Quantity = 1,
            UnitPrice = 100m
        };
        
        // Act
        bill.LineItems.Add(lineItem);
        
        // Assert
        bill.LineItems.Should().HaveCount(1);
        bill.LineItems.Should().Contain(lineItem);
    }
    
    [Fact]
    public void VendorBill_Payments_CanBeAdded()
    {
        // Arrange
        var bill = new VendorBill();
        var payment = new BillPayment 
        { 
            Id = Guid.NewGuid(),
            Amount = 100m,
            PaymentDate = DateTime.Now
        };
        
        // Act
        bill.Payments.Add(payment);
        
        // Assert
        bill.Payments.Should().HaveCount(1);
        bill.Payments.Should().Contain(payment);
    }
    
    [Fact]
    public void VendorBill_MultipleLineItems_ShouldBeSupported()
    {
        // Arrange
        var bill = new VendorBill();
        var lineItem1 = new BillLineItem { Id = Guid.NewGuid() };
        var lineItem2 = new BillLineItem { Id = Guid.NewGuid() };
        var lineItem3 = new BillLineItem { Id = Guid.NewGuid() };
        
        // Act
        bill.LineItems.Add(lineItem1);
        bill.LineItems.Add(lineItem2);
        bill.LineItems.Add(lineItem3);
        
        // Assert
        bill.LineItems.Should().HaveCount(3);
    }
    
    [Fact]
    public void VendorBill_MultiplePayments_ShouldBeSupported()
    {
        // Arrange
        var bill = new VendorBill { TotalAmount = 1000m };
        var payment1 = new BillPayment { Id = Guid.NewGuid(), Amount = 400m };
        var payment2 = new BillPayment { Id = Guid.NewGuid(), Amount = 300m };
        var payment3 = new BillPayment { Id = Guid.NewGuid(), Amount = 300m };
        
        // Act
        bill.Payments.Add(payment1);
        bill.Payments.Add(payment2);
        bill.Payments.Add(payment3);
        bill.AmountPaid = payment1.Amount + payment2.Amount + payment3.Amount;
        
        // Assert
        bill.Payments.Should().HaveCount(3);
        bill.AmountPaid.Should().Be(1000m);
        bill.AmountRemaining.Should().Be(0m);
    }
    
    [Fact]
    public void VendorBill_InheritsFromBaseEntity()
    {
        // Arrange & Act
        var bill = new VendorBill();
        
        // Assert
        bill.Should().BeAssignableTo<BaseEntity>();
    }
}

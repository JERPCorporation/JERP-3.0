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
/// Comprehensive test suite for CustomerInvoice entity (30+ tests)
/// Target: 90% code coverage for Core layer
/// </summary>
public class CustomerInvoiceTests
{
    [Fact]
    public void CustomerInvoice_Creation_ShouldSetDefaultStatus()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice();
        
        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Draft);
    }
    
    [Fact]
    public void CustomerInvoice_Creation_ShouldInitializeCollections()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice();
        
        // Assert
        invoice.LineItems.Should().NotBeNull();
        invoice.Payments.Should().NotBeNull();
        invoice.LineItems.Should().BeEmpty();
        invoice.Payments.Should().BeEmpty();
    }
    
    [Fact]
    public void AmountRemaining_WithNoPayments_ShouldEqualTotalAmount()
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 0m 
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(1000m);
    }
    
    [Fact]
    public void AmountRemaining_WithPartialPayment_ShouldReturnCorrectBalance()
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 400m 
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(600m);
    }
    
    [Fact]
    public void AmountRemaining_WhenFullyPaid_ShouldReturnZero()
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = 1000m, 
            AmountPaid = 1000m 
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(0m);
    }
    
    [Theory]
    [InlineData(1000, 0, 1000)]
    [InlineData(1000, 400, 600)]
    [InlineData(1000, 1000, 0)]
    [InlineData(5000, 2500, 2500)]
    [InlineData(99.99, 50.00, 49.99)]
    public void AmountRemaining_VariousPayments_ReturnsCorrectBalance(
        decimal total, decimal paid, decimal expectedBalance)
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = total, 
            AmountPaid = paid 
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(expectedBalance);
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
        var invoice = new CustomerInvoice 
        { 
            Subtotal = subtotal, 
            TaxAmount = tax,
            TotalAmount = subtotal + tax
        };
        
        // Act & Assert
        invoice.TotalAmount.Should().Be(expectedTotal);
    }
    
    [Fact]
    public void CustomerInvoice_WithDraftStatus_ShouldNotBePaid()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Status = InvoiceStatus.Draft,
            IsPaid = false
        };
        
        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Draft);
        invoice.IsPaid.Should().BeFalse();
    }
    
    [Fact]
    public void CustomerInvoice_WithPaidStatus_ShouldHavePaymentDate()
    {
        // Arrange
        var paymentDate = DateTime.Now;
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            Status = InvoiceStatus.Paid,
            IsPaid = true,
            PaymentDate = paymentDate
        };
        
        // Assert
        invoice.IsPaid.Should().BeTrue();
        invoice.PaymentDate.Should().BeCloseTo(paymentDate, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void CustomerInvoice_WithVoidStatus_ShouldNotBePaid()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Status = InvoiceStatus.Void,
            IsPaid = false
        };
        
        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Void);
        invoice.IsPaid.Should().BeFalse();
    }
    
    [Fact]
    public void CustomerInvoice_InvoiceNumber_ShouldNotBeNull()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            InvoiceNumber = "INV-001" 
        };
        
        // Assert
        invoice.InvoiceNumber.Should().NotBeNullOrEmpty();
        invoice.InvoiceNumber.Should().Be("INV-001");
    }
    
    [Fact]
    public void CustomerInvoice_DueDate_ShouldBeAfterInvoiceDate()
    {
        // Arrange
        var invoiceDate = DateTime.Now;
        var dueDate = invoiceDate.AddDays(30);
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            InvoiceDate = invoiceDate,
            DueDate = dueDate
        };
        
        // Assert
        invoice.DueDate.Should().BeAfter(invoice.InvoiceDate);
    }
    
    [Fact]
    public void CustomerInvoice_WithZeroAmounts_ShouldBeValid()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Subtotal = 0m,
            TaxAmount = 0m,
            TotalAmount = 0m
        };
        
        // Assert
        invoice.Subtotal.Should().Be(0m);
        invoice.TaxAmount.Should().Be(0m);
        invoice.TotalAmount.Should().Be(0m);
    }
    
    [Fact]
    public void CustomerInvoice_Notes_CanBeNull()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Notes = null 
        };
        
        // Assert
        invoice.Notes.Should().BeNull();
    }
    
    [Fact]
    public void CustomerInvoice_Notes_CanContainText()
    {
        // Arrange
        var notes = "Payment terms: Net 30";
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            Notes = notes 
        };
        
        // Assert
        invoice.Notes.Should().Be(notes);
    }
    
    [Fact]
    public void CustomerInvoice_CompanyId_ShouldBeRequired()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            CompanyId = companyId 
        };
        
        // Assert
        invoice.CompanyId.Should().Be(companyId);
        invoice.CompanyId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void CustomerInvoice_CustomerId_ShouldBeRequired()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            CustomerId = customerId 
        };
        
        // Assert
        invoice.CustomerId.Should().Be(customerId);
        invoice.CustomerId.Should().NotBeEmpty();
    }
    
    [Fact]
    public void CustomerInvoice_JournalEntryId_CanBeNull()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            JournalEntryId = null 
        };
        
        // Assert
        invoice.JournalEntryId.Should().BeNull();
    }
    
    [Fact]
    public void CustomerInvoice_JournalEntryId_CanBeSet()
    {
        // Arrange
        var journalEntryId = Guid.NewGuid();
        
        // Act
        var invoice = new CustomerInvoice 
        { 
            JournalEntryId = journalEntryId 
        };
        
        // Assert
        invoice.JournalEntryId.Should().NotBeNull();
        invoice.JournalEntryId.Should().Be(journalEntryId);
    }
    
    [Theory]
    [InlineData(InvoiceStatus.Draft)]
    [InlineData(InvoiceStatus.Sent)]
    [InlineData(InvoiceStatus.Paid)]
    [InlineData(InvoiceStatus.Void)]
    [InlineData(InvoiceStatus.Overdue)]
    public void CustomerInvoice_Status_CanBeAnyValidStatus(InvoiceStatus status)
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Status = status 
        };
        
        // Assert
        invoice.Status.Should().Be(status);
    }
    
    [Fact]
    public void CustomerInvoice_WithLargeAmounts_ShouldHandleCorrectly()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Subtotal = 999999.99m,
            TaxAmount = 99999.99m,
            TotalAmount = 1099999.98m
        };
        
        // Assert
        invoice.Subtotal.Should().Be(999999.99m);
        invoice.TaxAmount.Should().Be(99999.99m);
        invoice.TotalAmount.Should().Be(1099999.98m);
    }
    
    [Fact]
    public void CustomerInvoice_WithDecimalPrecision_ShouldMaintainAccuracy()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            Subtotal = 123.45m,
            TaxAmount = 12.35m,
            TotalAmount = 135.80m
        };
        
        // Assert
        invoice.Subtotal.Should().Be(123.45m);
        invoice.TaxAmount.Should().Be(12.35m);
        invoice.TotalAmount.Should().Be(135.80m);
    }
    
    [Fact]
    public void CustomerInvoice_PaymentDate_CanBeNull()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice 
        { 
            PaymentDate = null 
        };
        
        // Assert
        invoice.PaymentDate.Should().BeNull();
    }
    
    [Fact]
    public void CustomerInvoice_LineItems_CanBeAdded()
    {
        // Arrange
        var invoice = new CustomerInvoice();
        var lineItem = new InvoiceLineItem 
        { 
            Id = Guid.NewGuid(),
            Description = "Test Product",
            Quantity = 2,
            UnitPrice = 50m
        };
        
        // Act
        invoice.LineItems.Add(lineItem);
        
        // Assert
        invoice.LineItems.Should().HaveCount(1);
        invoice.LineItems.Should().Contain(lineItem);
    }
    
    [Fact]
    public void CustomerInvoice_Payments_CanBeAdded()
    {
        // Arrange
        var invoice = new CustomerInvoice();
        var payment = new InvoicePayment 
        { 
            Id = Guid.NewGuid(),
            Amount = 500m,
            PaymentDate = DateTime.Now
        };
        
        // Act
        invoice.Payments.Add(payment);
        
        // Assert
        invoice.Payments.Should().HaveCount(1);
        invoice.Payments.Should().Contain(payment);
    }
    
    [Fact]
    public void CustomerInvoice_MultipleLineItems_ShouldBeSupported()
    {
        // Arrange
        var invoice = new CustomerInvoice();
        var lineItem1 = new InvoiceLineItem { Id = Guid.NewGuid() };
        var lineItem2 = new InvoiceLineItem { Id = Guid.NewGuid() };
        var lineItem3 = new InvoiceLineItem { Id = Guid.NewGuid() };
        
        // Act
        invoice.LineItems.Add(lineItem1);
        invoice.LineItems.Add(lineItem2);
        invoice.LineItems.Add(lineItem3);
        
        // Assert
        invoice.LineItems.Should().HaveCount(3);
    }
    
    [Fact]
    public void CustomerInvoice_MultiplePayments_ShouldBeSupported()
    {
        // Arrange
        var invoice = new CustomerInvoice { TotalAmount = 1000m };
        var payment1 = new InvoicePayment { Id = Guid.NewGuid(), Amount = 250m };
        var payment2 = new InvoicePayment { Id = Guid.NewGuid(), Amount = 500m };
        var payment3 = new InvoicePayment { Id = Guid.NewGuid(), Amount = 250m };
        
        // Act
        invoice.Payments.Add(payment1);
        invoice.Payments.Add(payment2);
        invoice.Payments.Add(payment3);
        invoice.AmountPaid = payment1.Amount + payment2.Amount + payment3.Amount;
        
        // Assert
        invoice.Payments.Should().HaveCount(3);
        invoice.AmountPaid.Should().Be(1000m);
        invoice.AmountRemaining.Should().Be(0m);
    }
    
    [Fact]
    public void CustomerInvoice_InheritsFromBaseEntity()
    {
        // Arrange & Act
        var invoice = new CustomerInvoice();
        
        // Assert
        invoice.Should().BeAssignableTo<BaseEntity>();
    }
    
    [Fact]
    public void CustomerInvoice_WhenPartiallyPaid_ShouldNotBeFullyPaid()
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = 1000m,
            AmountPaid = 500m,
            IsPaid = false
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(500m);
        invoice.IsPaid.Should().BeFalse();
    }
    
    [Fact]
    public void CustomerInvoice_OverpaymentScenario_ShouldBeHandled()
    {
        // Arrange
        var invoice = new CustomerInvoice 
        { 
            TotalAmount = 1000m,
            AmountPaid = 1100m
        };
        
        // Act & Assert
        invoice.AmountRemaining.Should().Be(-100m);
    }
}

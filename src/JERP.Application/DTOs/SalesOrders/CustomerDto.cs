/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.SalesOrders;

public class CustomerDto
{
    public Guid Id { get; set; }
    
    public string CustomerCode { get; set; } = string.Empty;
    
    public string CustomerName { get; set; } = string.Empty;
    
    public string? LegalName { get; set; }
    
    public string? TaxId { get; set; }
    
    public string CustomerType { get; set; } = string.Empty; // Retail, Wholesale, Distributor, Medical, Recreational
    
    public string? ContactPerson { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Website { get; set; }
    
    // Billing Address
    public string? BillingAddressLine1 { get; set; }
    
    public string? BillingAddressLine2 { get; set; }
    
    public string? BillingCity { get; set; }
    
    public string? BillingState { get; set; }
    
    public string? BillingPostalCode { get; set; }
    
    public string? BillingCountry { get; set; }
    
    // Shipping Address
    public string? ShippingAddressLine1 { get; set; }
    
    public string? ShippingAddressLine2 { get; set; }
    
    public string? ShippingCity { get; set; }
    
    public string? ShippingState { get; set; }
    
    public string? ShippingPostalCode { get; set; }
    
    public string? ShippingCountry { get; set; }
    
    // Payment Terms
    public string? PaymentTerms { get; set; } // Net 30, COD, Credit Card, Due on Receipt
    
    public int? PaymentTermsDays { get; set; }
    
    public decimal CreditLimit { get; set; }
    
    public decimal CurrentBalance { get; set; }
    
    public decimal AvailableCredit { get; set; }
    
    // Pricing
    public string? PricingTier { get; set; } // Standard, Premium, Wholesale
    
    public decimal? DiscountPercent { get; set; }
    
    // Tax
    public bool IsTaxExempt { get; set; }
    
    public string? TaxExemptNumber { get; set; }
    
    public decimal? DefaultTaxRate { get; set; }
    
    // Accounting
    public Guid? DefaultARAccountId { get; set; }
    
    public Guid? DefaultRevenueAccountId { get; set; }
    
    // Status
    public bool IsActive { get; set; }
    
    public bool IsApproved { get; set; }
    
    public bool IsCreditHold { get; set; }
    
    public string? CreditHoldReason { get; set; }
    
    // Cannabis-specific
    public bool IsCannabisCustomer { get; set; }
    
    public string? LicenseNumber { get; set; }
    
    public DateTime? LicenseExpiration { get; set; }
    
    public string? LicenseType { get; set; } // Medical, Recreational, Both
    
    public bool RequiresMetrcTracking { get; set; }
    
    public string? MetrcLicenseNumber { get; set; }
    
    // Sales Rep
    public Guid? SalesRepId { get; set; }
    
    public string? SalesRepName { get; set; }
    
    // Metadata
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}

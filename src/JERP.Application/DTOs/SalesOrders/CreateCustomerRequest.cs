using System;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.SalesOrders;

public class CreateCustomerRequest
{
    [Required]
    [MaxLength(50)]
    public string CustomerCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? LegalName { get; set; }
    
    [MaxLength(50)]
    public string? TaxId { get; set; }
    
    [Required]
    public string CustomerType { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? ContactPerson { get; set; }
    
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    // Billing Address
    [MaxLength(200)]
    public string? BillingAddressLine1 { get; set; }
    
    [MaxLength(200)]
    public string? BillingAddressLine2 { get; set; }
    
    [MaxLength(100)]
    public string? BillingCity { get; set; }
    
    [MaxLength(50)]
    public string? BillingState { get; set; }
    
    [MaxLength(20)]
    public string? BillingPostalCode { get; set; }
    
    [MaxLength(100)]
    public string? BillingCountry { get; set; }
    
    // Shipping Address
    [MaxLength(200)]
    public string? ShippingAddressLine1 { get; set; }
    
    [MaxLength(200)]
    public string? ShippingAddressLine2 { get; set; }
    
    [MaxLength(100)]
    public string? ShippingCity { get; set; }
    
    [MaxLength(50)]
    public string? ShippingState { get; set; }
    
    [MaxLength(20)]
    public string? ShippingPostalCode { get; set; }
    
    [MaxLength(100)]
    public string? ShippingCountry { get; set; }
    
    [MaxLength(50)]
    public string? PaymentTerms { get; set; }
    
    public int? PaymentTermsDays { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal CreditLimit { get; set; }
    
    [MaxLength(50)]
    public string? PricingTier { get; set; }
    
    [Range(0, 100)]
    public decimal? DiscountPercent { get; set; }
    
    public bool IsTaxExempt { get; set; }
    
    [MaxLength(50)]
    public string? TaxExemptNumber { get; set; }
    
    public decimal? DefaultTaxRate { get; set; }
    
    public Guid? DefaultARAccountId { get; set; }
    
    public Guid? DefaultRevenueAccountId { get; set; }
    
    public bool IsCannabisCustomer { get; set; }
    
    [MaxLength(100)]
    public string? LicenseNumber { get; set; }
    
    public DateTime? LicenseExpiration { get; set; }
    
    [MaxLength(50)]
    public string? LicenseType { get; set; }
    
    public bool RequiresMetrcTracking { get; set; }
    
    [MaxLength(100)]
    public string? MetrcLicenseNumber { get; set; }
    
    public Guid? SalesRepId { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
}

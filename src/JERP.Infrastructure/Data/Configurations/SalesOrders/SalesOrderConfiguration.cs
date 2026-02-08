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

using JERP.Core.Entities.SalesOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.SalesOrders;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrders");

        builder.HasKey(so => so.Id);

        builder.Property(so => so.SONumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(so => so.ShippingMethod)
            .HasMaxLength(100);

        builder.Property(so => so.ShippingTerms)
            .HasMaxLength(50);

        builder.Property(so => so.PaymentTerms)
            .HasMaxLength(50);

        builder.Property(so => so.ShipToAddressLine1)
            .HasMaxLength(200);

        builder.Property(so => so.ShipToAddressLine2)
            .HasMaxLength(200);

        builder.Property(so => so.ShipToCity)
            .HasMaxLength(100);

        builder.Property(so => so.ShipToState)
            .HasMaxLength(50);

        builder.Property(so => so.ShipToPostalCode)
            .HasMaxLength(20);

        builder.Property(so => so.ShipToCountry)
            .HasMaxLength(100);

        builder.Property(so => so.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(so => so.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(so => so.ShippingAmount)
            .HasPrecision(18, 2);

        builder.Property(so => so.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(so => so.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(so => so.TotalShipped)
            .HasPrecision(18, 2);

        builder.Property(so => so.TotalInvoiced)
            .HasPrecision(18, 2);

        builder.Property(so => so.ApprovedBy)
            .HasMaxLength(100);

        builder.Property(so => so.SalesQuoteNumber)
            .HasMaxLength(50);

        builder.Property(so => so.CustomerPONumber)
            .HasMaxLength(50);

        builder.Property(so => so.Notes)
            .HasMaxLength(2000);

        builder.Property(so => so.InternalNotes)
            .HasMaxLength(2000);

        builder.Property(so => so.MetrcManifestNumber)
            .HasMaxLength(100);

        builder.Property(so => so.CreatedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(so => so.CompanyId)
            .HasDatabaseName("IX_SalesOrders_CompanyId");

        builder.HasIndex(so => so.CustomerId)
            .HasDatabaseName("IX_SalesOrders_CustomerId");

        builder.HasIndex(so => new { so.CompanyId, so.SONumber })
            .IsUnique()
            .HasDatabaseName("IX_SalesOrders_CompanyId_SONumber");

        builder.HasIndex(so => so.OrderDate)
            .HasDatabaseName("IX_SalesOrders_OrderDate");

        builder.HasIndex(so => so.Status)
            .HasDatabaseName("IX_SalesOrders_Status");

        // Relationships - Use Restrict for financial entities
        builder.HasOne(so => so.Company)
            .WithMany()
            .HasForeignKey(so => so.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(so => so.Warehouse)
            .WithMany()
            .HasForeignKey(so => so.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // LineItems - Cascade OK for true parent-child detail records
        builder.HasMany(so => so.LineItems)
            .WithOne(li => li.SalesOrder)
            .HasForeignKey(li => li.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Shipments - Restrict because shipments are independent operational records
        builder.HasMany(so => so.Shipments)
            .WithOne(s => s.SalesOrder)
            .HasForeignKey(s => s.SalesOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(so => !so.IsDeleted);
    }
}

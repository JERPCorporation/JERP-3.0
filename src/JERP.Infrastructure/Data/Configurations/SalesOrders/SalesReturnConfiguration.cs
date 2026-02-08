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

public class SalesReturnConfiguration : IEntityTypeConfiguration<SalesReturn>
{
    public void Configure(EntityTypeBuilder<SalesReturn> builder)
    {
        builder.ToTable("SalesReturns");

        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.RMANumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(sr => sr.Reason)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sr => sr.ReturnType)
            .HasMaxLength(50);

        builder.Property(sr => sr.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(sr => sr.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(sr => sr.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(sr => sr.ApprovedBy)
            .HasMaxLength(100);

        builder.Property(sr => sr.ReceivedBy)
            .HasMaxLength(100);

        builder.Property(sr => sr.Notes)
            .HasMaxLength(2000);

        // Indexes
        builder.HasIndex(sr => sr.CompanyId)
            .HasDatabaseName("IX_SalesReturns_CompanyId");

        builder.HasIndex(sr => sr.CustomerId)
            .HasDatabaseName("IX_SalesReturns_CustomerId");

        builder.HasIndex(sr => new { sr.CompanyId, sr.RMANumber })
            .IsUnique()
            .HasDatabaseName("IX_SalesReturns_CompanyId_RMANumber");

        builder.HasIndex(sr => sr.ReturnDate)
            .HasDatabaseName("IX_SalesReturns_ReturnDate");

        // Relationships - Use Restrict for financial entities
        builder.HasOne(sr => sr.Company)
            .WithMany()
            .HasForeignKey(sr => sr.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.Customer)
            .WithMany()
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.SalesOrder)
            .WithMany()
            .HasForeignKey(sr => sr.SalesOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.Invoice)
            .WithMany()
            .HasForeignKey(sr => sr.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // LineItems - Cascade OK for true parent-child detail records
        builder.HasMany(sr => sr.LineItems)
            .WithOne(li => li.SalesReturn)
            .HasForeignKey(li => li.SalesReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(sr => !sr.IsDeleted);
    }
}

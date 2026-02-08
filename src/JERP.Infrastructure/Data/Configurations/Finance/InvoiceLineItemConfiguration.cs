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

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("InvoiceLineItems");

        builder.HasKey(ili => ili.Id);

        builder.Property(ili => ili.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ili => ili.Quantity)
            .HasPrecision(18, 4);

        builder.Property(ili => ili.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(ili => ili.Amount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(ili => ili.InvoiceId)
            .HasDatabaseName("IX_InvoiceLineItems_InvoiceId");

        builder.HasIndex(ili => ili.AccountId)
            .HasDatabaseName("IX_InvoiceLineItems_AccountId");

        // Relationships
        // Invoice -> LineItems uses Cascade (parent-child detail record)
        builder.HasOne(ili => ili.Invoice)
            .WithMany(i => i.LineItems)
            .HasForeignKey(ili => ili.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Account uses Restrict to prevent cascade delete cycles
        builder.HasOne(ili => ili.Account)
            .WithMany()
            .HasForeignKey(ili => ili.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(ili => !ili.IsDeleted);
    }
}

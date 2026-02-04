/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

/// <summary>
/// Entity Framework configuration for the InvoiceLineItem entity
/// </summary>
public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("InvoiceLineItems");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.InvoiceId)
            .IsRequired();

        builder.Property(l => l.AccountId)
            .IsRequired();

        builder.Property(l => l.Description)
            .HasMaxLength(500);

        builder.Property(l => l.Quantity)
            .HasPrecision(18, 4);

        builder.Property(l => l.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(l => l.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(l => l.InvoiceId);
        builder.HasIndex(l => l.AccountId);

        // Relationships
        builder.HasOne(l => l.Invoice)
            .WithMany(i => i.LineItems)
            .HasForeignKey(l => l.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}

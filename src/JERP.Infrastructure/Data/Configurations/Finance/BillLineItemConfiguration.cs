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

public class BillLineItemConfiguration : IEntityTypeConfiguration<BillLineItem>
{
    public void Configure(EntityTypeBuilder<BillLineItem> builder)
    {
        builder.ToTable("BillLineItems");

        builder.HasKey(bli => bli.Id);

        builder.Property(bli => bli.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(bli => bli.Quantity)
            .HasPrecision(18, 4);

        builder.Property(bli => bli.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(bli => bli.Amount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(bli => bli.BillId)
            .HasDatabaseName("IX_BillLineItems_BillId");

        builder.HasIndex(bli => bli.AccountId)
            .HasDatabaseName("IX_BillLineItems_AccountId");

        // Relationships
        // Bill -> LineItems uses Cascade (parent-child detail record)
        builder.HasOne(bli => bli.Bill)
            .WithMany(b => b.LineItems)
            .HasForeignKey(bli => bli.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Account uses Restrict to prevent cascade delete cycles
        builder.HasOne(bli => bli.Account)
            .WithMany()
            .HasForeignKey(bli => bli.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(bli => !bli.IsDeleted);
    }
}

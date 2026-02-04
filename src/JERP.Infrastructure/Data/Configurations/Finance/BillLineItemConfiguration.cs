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
/// Entity Framework configuration for the BillLineItem entity
/// </summary>
public class BillLineItemConfiguration : IEntityTypeConfiguration<BillLineItem>
{
    public void Configure(EntityTypeBuilder<BillLineItem> builder)
    {
        builder.ToTable("BillLineItems");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.BillId)
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
        builder.HasIndex(l => l.BillId);
        builder.HasIndex(l => l.AccountId);

        // Relationships
        builder.HasOne(l => l.Bill)
            .WithMany(b => b.LineItems)
            .HasForeignKey(l => l.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}

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
/// Entity Framework configuration for the Bill entity
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CompanyId)
            .IsRequired();

        builder.Property(b => b.VendorId)
            .IsRequired();

        builder.Property(b => b.BillNumber)
            .HasMaxLength(50);

        builder.Property(b => b.BillDate)
            .IsRequired();

        builder.Property(b => b.DueDate)
            .IsRequired();

        builder.Property(b => b.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(b => b.PaidAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.Status)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(b => b.CompanyId);
        builder.HasIndex(b => b.VendorId);
        builder.HasIndex(b => b.BillDate);
        builder.HasIndex(b => b.DueDate);
        builder.HasIndex(b => b.Status);

        // Relationships
        builder.HasOne(b => b.Company)
            .WithMany()
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Vendor)
            .WithMany(v => v.Bills)
            .HasForeignKey(b => b.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.JournalEntry)
            .WithMany()
            .HasForeignKey(b => b.JournalEntryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.LineItems)
            .WithOne(l => l.Bill)
            .HasForeignKey(l => l.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Payments)
            .WithOne(p => p.Bill)
            .HasForeignKey(p => p.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}

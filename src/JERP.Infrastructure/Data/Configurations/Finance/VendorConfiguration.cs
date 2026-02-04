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
/// Entity Framework configuration for the Vendor entity
/// </summary>
public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.CompanyId)
            .IsRequired();

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.VendorCode)
            .HasMaxLength(50);

        builder.Property(v => v.ContactName)
            .HasMaxLength(100);

        builder.Property(v => v.Email)
            .HasMaxLength(100);

        builder.Property(v => v.Phone)
            .HasMaxLength(20);

        builder.Property(v => v.Address)
            .HasMaxLength(500);

        builder.Property(v => v.City)
            .HasMaxLength(100);

        builder.Property(v => v.State)
            .HasMaxLength(50);

        builder.Property(v => v.ZipCode)
            .HasMaxLength(20);

        builder.Property(v => v.TaxId)
            .HasMaxLength(50);

        builder.Property(v => v.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(v => v.CompanyId);
        builder.HasIndex(v => new { v.CompanyId, v.VendorCode })
            .IsUnique()
            .HasFilter("[VendorCode] IS NOT NULL");

        builder.HasIndex(v => v.IsActive);

        // Relationships
        builder.HasOne(v => v.Company)
            .WithMany()
            .HasForeignKey(v => v.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Bills)
            .WithOne(b => b.Vendor)
            .HasForeignKey(b => b.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}

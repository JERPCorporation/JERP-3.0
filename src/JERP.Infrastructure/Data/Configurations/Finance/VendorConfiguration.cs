/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.VendorNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.ContactName)
            .HasMaxLength(100);

        builder.Property(v => v.Email)
            .HasMaxLength(100);

        builder.Property(v => v.Phone)
            .HasMaxLength(20);

        builder.Property(v => v.Street)
            .HasMaxLength(200);

        builder.Property(v => v.City)
            .HasMaxLength(100);

        builder.Property(v => v.State)
            .HasMaxLength(50);

        builder.Property(v => v.PostalCode)
            .HasMaxLength(20);

        builder.Property(v => v.Country)
            .HasMaxLength(100);

        builder.Property(v => v.TaxId)
            .HasMaxLength(50);

        builder.Property(v => v.Balance)
            .HasPrecision(18, 2);

        builder.Property(v => v.CannabisLicense)
            .HasMaxLength(50);

        builder.Property(v => v.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(v => v.CompanyId)
            .HasDatabaseName("IX_Vendors_CompanyId");

        builder.HasIndex(v => new { v.CompanyId, v.VendorNumber })
            .IsUnique()
            .HasDatabaseName("IX_Vendors_CompanyId_VendorNumber");

        // Relationships
        builder.HasOne(v => v.Company)
            .WithMany()
            .HasForeignKey(v => v.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.AccountsPayableAccount)
            .WithMany()
            .HasForeignKey(v => v.AccountsPayableAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Bills)
            .WithOne(b => b.Vendor)
            .HasForeignKey(b => b.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}

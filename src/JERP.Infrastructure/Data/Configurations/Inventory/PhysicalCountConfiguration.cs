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

using JERP.Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Inventory;

public class PhysicalCountConfiguration : IEntityTypeConfiguration<PhysicalCount>
{
    public void Configure(EntityTypeBuilder<PhysicalCount> builder)
    {
        builder.ToTable("PhysicalCounts");

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.CountNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pc => pc.Type)
            .HasConversion<string>();

        builder.Property(pc => pc.Status)
            .HasConversion<string>();

        builder.Property(pc => pc.TotalVarianceValue)
            .HasPrecision(18, 2);

        builder.Property(pc => pc.AccuracyRate)
            .HasPrecision(5, 2);

        builder.Property(pc => pc.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(pc => pc.CompanyId)
            .HasDatabaseName("IX_PhysicalCounts_CompanyId");

        builder.HasIndex(pc => new { pc.CompanyId, pc.CountNumber })
            .IsUnique()
            .HasDatabaseName("IX_PhysicalCounts_CompanyId_CountNumber");

        builder.HasIndex(pc => pc.WarehouseId)
            .HasDatabaseName("IX_PhysicalCounts_WarehouseId");

        builder.HasIndex(pc => pc.Status)
            .HasDatabaseName("IX_PhysicalCounts_Status");

        builder.HasIndex(pc => pc.CountDate)
            .HasDatabaseName("IX_PhysicalCounts_CountDate");

        // Relationships
        builder.HasOne(pc => pc.Company)
            .WithMany()
            .HasForeignKey(pc => pc.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pc => pc.Warehouse)
            .WithMany()
            .HasForeignKey(pc => pc.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pc => pc.Adjustment)
            .WithMany()
            .HasForeignKey(pc => pc.AdjustmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pc => pc.Lines)
            .WithOne(l => l.Count)
            .HasForeignKey(l => l.CountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(pc => !pc.IsDeleted);
    }
}

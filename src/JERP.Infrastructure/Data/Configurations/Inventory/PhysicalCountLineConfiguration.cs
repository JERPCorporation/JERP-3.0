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

public class PhysicalCountLineConfiguration : IEntityTypeConfiguration<PhysicalCountLine>
{
    public void Configure(EntityTypeBuilder<PhysicalCountLine> builder)
    {
        builder.ToTable("PhysicalCountLines");

        builder.HasKey(pcl => pcl.Id);

        builder.Property(pcl => pcl.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(pcl => pcl.Reason)
            .HasConversion<string>();

        builder.Property(pcl => pcl.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(pcl => pcl.CountId)
            .HasDatabaseName("IX_PhysicalCountLines_CountId");

        builder.HasIndex(pcl => pcl.ProductId)
            .HasDatabaseName("IX_PhysicalCountLines_ProductId");

        builder.HasIndex(pcl => pcl.BatchId)
            .HasDatabaseName("IX_PhysicalCountLines_BatchId");

        // Relationships
        builder.HasOne(pcl => pcl.Count)
            .WithMany(pc => pc.Lines)
            .HasForeignKey(pcl => pcl.CountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pcl => pcl.Product)
            .WithMany()
            .HasForeignKey(pcl => pcl.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pcl => pcl.Batch)
            .WithMany()
            .HasForeignKey(pcl => pcl.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(pcl => !pcl.IsDeleted);
    }
}

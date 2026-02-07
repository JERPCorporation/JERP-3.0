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

using JERP.Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Inventory;

public class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
{
    public void Configure(EntityTypeBuilder<ProductBatch> builder)
    {
        builder.ToTable("ProductBatches");

        builder.HasKey(pb => pb.Id);

        builder.Property(pb => pb.BatchNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pb => pb.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(pb => pb.ActualTHC)
            .HasPrecision(5, 2);

        builder.Property(pb => pb.ActualCBD)
            .HasPrecision(5, 2);

        builder.Property(pb => pb.TestingLab)
            .HasMaxLength(100);

        builder.Property(pb => pb.TestCertificateUrl)
            .HasMaxLength(500);

        builder.Property(pb => pb.MetrcUID)
            .HasMaxLength(50);

        builder.Property(pb => pb.SourceLicense)
            .HasMaxLength(100);

        builder.Property(pb => pb.QuarantineReason)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(pb => pb.ProductId)
            .HasDatabaseName("IX_ProductBatches_ProductId");

        builder.HasIndex(pb => pb.WarehouseId)
            .HasDatabaseName("IX_ProductBatches_WarehouseId");

        builder.HasIndex(pb => pb.BatchNumber)
            .HasDatabaseName("IX_ProductBatches_BatchNumber");

        builder.HasIndex(pb => pb.ExpirationDate)
            .HasDatabaseName("IX_ProductBatches_ExpirationDate");

        // Relationships
        builder.HasOne(pb => pb.Product)
            .WithMany(p => p.Batches)
            .HasForeignKey(pb => pb.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pb => pb.Warehouse)
            .WithMany()
            .HasForeignKey(pb => pb.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(pb => !pb.IsDeleted);
    }
}

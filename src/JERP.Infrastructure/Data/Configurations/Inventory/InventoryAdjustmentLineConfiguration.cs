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

public class InventoryAdjustmentLineConfiguration : IEntityTypeConfiguration<InventoryAdjustmentLine>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustmentLine> builder)
    {
        builder.ToTable("InventoryAdjustmentLines");

        builder.HasKey(ial => ial.Id);

        builder.Property(ial => ial.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(ial => ial.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(ial => ial.AdjustmentId)
            .HasDatabaseName("IX_InventoryAdjustmentLines_AdjustmentId");

        builder.HasIndex(ial => ial.ProductId)
            .HasDatabaseName("IX_InventoryAdjustmentLines_ProductId");

        builder.HasIndex(ial => ial.BatchId)
            .HasDatabaseName("IX_InventoryAdjustmentLines_BatchId");

        // Relationships
        builder.HasOne(ial => ial.Adjustment)
            .WithMany(ia => ia.Lines)
            .HasForeignKey(ial => ial.AdjustmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ial => ial.Product)
            .WithMany()
            .HasForeignKey(ial => ial.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ial => ial.Batch)
            .WithMany()
            .HasForeignKey(ial => ial.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(ial => !ial.IsDeleted);
    }
}

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

public class InventoryLevelConfiguration : IEntityTypeConfiguration<InventoryLevel>
{
    public void Configure(EntityTypeBuilder<InventoryLevel> builder)
    {
        builder.ToTable("InventoryLevels");

        builder.HasKey(il => il.Id);

        builder.Property(il => il.TotalValue)
            .HasPrecision(18, 2);

        builder.Property(il => il.AverageCost)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(il => il.ProductId)
            .HasDatabaseName("IX_InventoryLevels_ProductId");

        builder.HasIndex(il => il.WarehouseId)
            .HasDatabaseName("IX_InventoryLevels_WarehouseId");

        builder.HasIndex(il => new { il.ProductId, il.WarehouseId })
            .IsUnique()
            .HasDatabaseName("IX_InventoryLevels_ProductId_WarehouseId");

        // Relationships
        builder.HasOne(il => il.Product)
            .WithMany(p => p.InventoryLevels)
            .HasForeignKey(il => il.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(il => il.Warehouse)
            .WithMany(w => w.InventoryLevels)
            .HasForeignKey(il => il.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(il => !il.IsDeleted);
    }
}

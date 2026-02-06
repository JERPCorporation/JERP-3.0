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

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Description)
            .HasMaxLength(500);

        builder.Property(w => w.Street)
            .HasMaxLength(200);

        builder.Property(w => w.City)
            .HasMaxLength(100);

        builder.Property(w => w.State)
            .HasMaxLength(2);

        builder.Property(w => w.Zip)
            .HasMaxLength(10);

        builder.Property(w => w.WarehouseType)
            .HasMaxLength(50);

        builder.Property(w => w.CapacityUnit)
            .HasMaxLength(50);

        builder.Property(w => w.CannabisLicense)
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(w => w.CompanyId)
            .HasDatabaseName("IX_Warehouses_CompanyId");

        builder.HasIndex(w => new { w.CompanyId, w.Code })
            .IsUnique()
            .HasDatabaseName("IX_Warehouses_CompanyId_Code");

        // Relationships
        builder.HasOne(w => w.Company)
            .WithMany()
            .HasForeignKey(w => w.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.InventoryLevels)
            .WithOne(il => il.Warehouse)
            .HasForeignKey(il => il.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(w => !w.IsDeleted);
    }
}

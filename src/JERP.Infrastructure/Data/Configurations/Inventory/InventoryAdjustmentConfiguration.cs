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

public class InventoryAdjustmentConfiguration : IEntityTypeConfiguration<InventoryAdjustment>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustment> builder)
    {
        builder.ToTable("InventoryAdjustments");

        builder.HasKey(ia => ia.Id);

        builder.Property(ia => ia.AdjustmentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(ia => ia.Type)
            .HasConversion<string>();

        builder.Property(ia => ia.Reason)
            .HasConversion<string>();

        builder.Property(ia => ia.TotalValue)
            .HasPrecision(18, 2);

        builder.Property(ia => ia.Notes)
            .HasMaxLength(1000);

        builder.Property(ia => ia.Status)
            .HasConversion<string>();

        // Indexes
        builder.HasIndex(ia => ia.CompanyId)
            .HasDatabaseName("IX_InventoryAdjustments_CompanyId");

        builder.HasIndex(ia => new { ia.CompanyId, ia.AdjustmentNumber })
            .IsUnique()
            .HasDatabaseName("IX_InventoryAdjustments_CompanyId_AdjustmentNumber");

        builder.HasIndex(ia => ia.WarehouseId)
            .HasDatabaseName("IX_InventoryAdjustments_WarehouseId");

        builder.HasIndex(ia => ia.Status)
            .HasDatabaseName("IX_InventoryAdjustments_Status");

        builder.HasIndex(ia => ia.AdjustmentDate)
            .HasDatabaseName("IX_InventoryAdjustments_AdjustmentDate");

        // Relationships
        builder.HasOne(ia => ia.Company)
            .WithMany()
            .HasForeignKey(ia => ia.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ia => ia.Warehouse)
            .WithMany()
            .HasForeignKey(ia => ia.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ia => ia.JournalEntry)
            .WithMany()
            .HasForeignKey(ia => ia.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ia => ia.Lines)
            .WithOne(l => l.Adjustment)
            .HasForeignKey(l => l.AdjustmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(ia => !ia.IsDeleted);
    }
}

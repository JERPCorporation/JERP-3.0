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

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("InventoryTransactions");

        builder.HasKey(it => it.Id);

        builder.Property(it => it.Type)
            .HasConversion<string>();

        builder.Property(it => it.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(it => it.SourceType)
            .HasMaxLength(50);

        builder.Property(it => it.SourceNumber)
            .HasMaxLength(50);

        builder.Property(it => it.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(it => it.CompanyId)
            .HasDatabaseName("IX_InventoryTransactions_CompanyId");

        builder.HasIndex(it => it.ProductId)
            .HasDatabaseName("IX_InventoryTransactions_ProductId");

        builder.HasIndex(it => it.WarehouseId)
            .HasDatabaseName("IX_InventoryTransactions_WarehouseId");

        builder.HasIndex(it => it.BatchId)
            .HasDatabaseName("IX_InventoryTransactions_BatchId");

        builder.HasIndex(it => it.TransactionDate)
            .HasDatabaseName("IX_InventoryTransactions_TransactionDate");

        builder.HasIndex(it => it.Type)
            .HasDatabaseName("IX_InventoryTransactions_Type");

        builder.HasIndex(it => new { it.SourceType, it.SourceId })
            .HasDatabaseName("IX_InventoryTransactions_Source");

        // Relationships
        builder.HasOne(it => it.Company)
            .WithMany()
            .HasForeignKey(it => it.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Product)
            .WithMany()
            .HasForeignKey(it => it.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Warehouse)
            .WithMany()
            .HasForeignKey(it => it.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Batch)
            .WithMany()
            .HasForeignKey(it => it.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(it => !it.IsDeleted);
    }
}

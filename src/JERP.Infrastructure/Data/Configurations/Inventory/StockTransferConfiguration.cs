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

public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.ToTable("StockTransfers");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.TransferNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(st => st.Status)
            .HasConversion<string>();

        builder.Property(st => st.TotalValue)
            .HasPrecision(18, 2);

        builder.Property(st => st.Reason)
            .HasMaxLength(500);

        builder.Property(st => st.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(st => st.CompanyId)
            .HasDatabaseName("IX_StockTransfers_CompanyId");

        builder.HasIndex(st => new { st.CompanyId, st.TransferNumber })
            .IsUnique()
            .HasDatabaseName("IX_StockTransfers_CompanyId_TransferNumber");

        builder.HasIndex(st => st.FromWarehouseId)
            .HasDatabaseName("IX_StockTransfers_FromWarehouseId");

        builder.HasIndex(st => st.ToWarehouseId)
            .HasDatabaseName("IX_StockTransfers_ToWarehouseId");

        builder.HasIndex(st => st.Status)
            .HasDatabaseName("IX_StockTransfers_Status");

        builder.HasIndex(st => st.TransferDate)
            .HasDatabaseName("IX_StockTransfers_TransferDate");

        // Relationships
        builder.HasOne(st => st.Company)
            .WithMany()
            .HasForeignKey(st => st.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(st => st.FromWarehouse)
            .WithMany()
            .HasForeignKey(st => st.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(st => st.ToWarehouse)
            .WithMany()
            .HasForeignKey(st => st.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(st => st.Lines)
            .WithOne(l => l.Transfer)
            .HasForeignKey(l => l.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(st => !st.IsDeleted);
    }
}

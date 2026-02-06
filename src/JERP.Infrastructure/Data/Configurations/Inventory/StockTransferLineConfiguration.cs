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

public class StockTransferLineConfiguration : IEntityTypeConfiguration<StockTransferLine>
{
    public void Configure(EntityTypeBuilder<StockTransferLine> builder)
    {
        builder.ToTable("StockTransferLines");

        builder.HasKey(stl => stl.Id);

        builder.Property(stl => stl.UnitCost)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(stl => stl.TransferId)
            .HasDatabaseName("IX_StockTransferLines_TransferId");

        builder.HasIndex(stl => stl.ProductId)
            .HasDatabaseName("IX_StockTransferLines_ProductId");

        builder.HasIndex(stl => stl.BatchId)
            .HasDatabaseName("IX_StockTransferLines_BatchId");

        // Relationships
        builder.HasOne(stl => stl.Transfer)
            .WithMany(st => st.Lines)
            .HasForeignKey(stl => stl.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(stl => stl.Product)
            .WithMany()
            .HasForeignKey(stl => stl.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(stl => stl.Batch)
            .WithMany()
            .HasForeignKey(stl => stl.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(stl => !stl.IsDeleted);
    }
}

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

public class StockReceiptLineConfiguration : IEntityTypeConfiguration<StockReceiptLine>
{
    public void Configure(EntityTypeBuilder<StockReceiptLine> builder)
    {
        builder.ToTable("StockReceiptLines");

        builder.HasKey(srl => srl.Id);

        builder.Property(srl => srl.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.BatchNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(srl => srl.ActualTHC)
            .HasPrecision(5, 2);

        builder.Property(srl => srl.ActualCBD)
            .HasPrecision(5, 2);

        builder.Property(srl => srl.TestCertificateUrl)
            .HasMaxLength(500);

        builder.Property(srl => srl.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(srl => srl.ReceiptId)
            .HasDatabaseName("IX_StockReceiptLines_ReceiptId");

        builder.HasIndex(srl => srl.POLineId)
            .HasDatabaseName("IX_StockReceiptLines_POLineId");

        builder.HasIndex(srl => srl.ProductId)
            .HasDatabaseName("IX_StockReceiptLines_ProductId");

        // Relationships
        builder.HasOne(srl => srl.Receipt)
            .WithMany(sr => sr.Lines)
            .HasForeignKey(srl => srl.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(srl => srl.POLine)
            .WithMany()
            .HasForeignKey(srl => srl.POLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(srl => srl.Product)
            .WithMany()
            .HasForeignKey(srl => srl.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(srl => !srl.IsDeleted);
    }
}

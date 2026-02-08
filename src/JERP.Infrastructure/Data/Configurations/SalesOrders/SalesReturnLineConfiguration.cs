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

using JERP.Core.Entities.SalesOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.SalesOrders;

public class SalesReturnLineConfiguration : IEntityTypeConfiguration<SalesReturnLine>
{
    public void Configure(EntityTypeBuilder<SalesReturnLine> builder)
    {
        builder.ToTable("SalesReturnLines");

        builder.HasKey(srl => srl.Id);

        builder.Property(srl => srl.Description)
            .HasMaxLength(500);

        builder.Property(srl => srl.Quantity)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.LineTotal)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.RestockingFee)
            .HasPrecision(18, 2);

        builder.Property(srl => srl.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(srl => srl.SalesReturnId)
            .HasDatabaseName("IX_SalesReturnLines_SalesReturnId");

        builder.HasIndex(srl => srl.InventoryItemId)
            .HasDatabaseName("IX_SalesReturnLines_InventoryItemId");

        // Relationships
        // SalesReturn -> Lines uses Cascade (parent-child detail record)
        builder.HasOne(srl => srl.SalesReturn)
            .WithMany(sr => sr.LineItems)
            .HasForeignKey(srl => srl.SalesReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        // Other FKs use Restrict
        builder.HasOne(srl => srl.SalesOrderLine)
            .WithMany()
            .HasForeignKey(srl => srl.SalesOrderLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(srl => srl.InventoryItem)
            .WithMany()
            .HasForeignKey(srl => srl.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(srl => !srl.IsDeleted);
    }
}

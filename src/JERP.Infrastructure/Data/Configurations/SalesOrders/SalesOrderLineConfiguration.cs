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

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLines");

        builder.HasKey(sol => sol.Id);

        builder.Property(sol => sol.Description)
            .HasMaxLength(500);

        builder.Property(sol => sol.Quantity)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.DiscountPercent)
            .HasPrecision(5, 2);

        builder.Property(sol => sol.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.TaxPercent)
            .HasPrecision(5, 2);

        builder.Property(sol => sol.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.LineTotal)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.QuantityShipped)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.QuantityInvoiced)
            .HasPrecision(18, 2);

        builder.Property(sol => sol.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(sol => sol.SalesOrderId)
            .HasDatabaseName("IX_SalesOrderLines_SalesOrderId");

        builder.HasIndex(sol => sol.InventoryItemId)
            .HasDatabaseName("IX_SalesOrderLines_InventoryItemId");

        // Relationships
        // SalesOrder -> Lines uses Cascade (parent-child detail record)
        builder.HasOne(sol => sol.SalesOrder)
            .WithMany(so => so.LineItems)
            .HasForeignKey(sol => sol.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // InventoryItem and RevenueAccount use Restrict
        builder.HasOne(sol => sol.InventoryItem)
            .WithMany()
            .HasForeignKey(sol => sol.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sol => sol.RevenueAccount)
            .WithMany()
            .HasForeignKey(sol => sol.RevenueAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(sol => !sol.IsDeleted);
    }
}

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

public class SOShipmentLineConfiguration : IEntityTypeConfiguration<SOShipmentLine>
{
    public void Configure(EntityTypeBuilder<SOShipmentLine> builder)
    {
        builder.ToTable("SOShipmentLines");

        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.QuantityShipped)
            .HasPrecision(18, 2);

        builder.Property(sl => sl.SerialNumber)
            .HasMaxLength(50);

        builder.Property(sl => sl.BinLocation)
            .HasMaxLength(50);

        builder.Property(sl => sl.Notes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(sl => sl.ShipmentId)
            .HasDatabaseName("IX_SOShipmentLines_ShipmentId");

        builder.HasIndex(sl => sl.SalesOrderLineId)
            .HasDatabaseName("IX_SOShipmentLines_SalesOrderLineId");

        builder.HasIndex(sl => sl.InventoryItemId)
            .HasDatabaseName("IX_SOShipmentLines_InventoryItemId");

        // Relationships
        // SOShipment -> Lines uses Cascade (parent-child detail record)
        builder.HasOne(sl => sl.Shipment)
            .WithMany(s => s.LineItems)
            .HasForeignKey(sl => sl.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Other FKs use Restrict
        builder.HasOne(sl => sl.SalesOrderLine)
            .WithMany()
            .HasForeignKey(sl => sl.SalesOrderLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sl => sl.InventoryItem)
            .WithMany()
            .HasForeignKey(sl => sl.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sl => sl.BatchLot)
            .WithMany()
            .HasForeignKey(sl => sl.BatchLotId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(sl => !sl.IsDeleted);
    }
}

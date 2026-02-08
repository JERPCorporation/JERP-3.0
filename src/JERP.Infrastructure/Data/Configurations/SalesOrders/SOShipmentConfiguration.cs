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

public class SOShipmentConfiguration : IEntityTypeConfiguration<SOShipment>
{
    public void Configure(EntityTypeBuilder<SOShipment> builder)
    {
        builder.ToTable("SOShipments");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ShipmentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.ShippingMethod)
            .HasMaxLength(100);

        builder.Property(s => s.TrackingNumber)
            .HasMaxLength(100);

        builder.Property(s => s.Carrier)
            .HasMaxLength(100);

        builder.Property(s => s.ShippingCost)
            .HasPrecision(18, 2);

        builder.Property(s => s.MetrcManifestNumber)
            .HasMaxLength(100);

        builder.Property(s => s.PackedBy)
            .HasMaxLength(100);

        builder.Property(s => s.ShippedBy)
            .HasMaxLength(100);

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(s => s.CompanyId)
            .HasDatabaseName("IX_SOShipments_CompanyId");

        builder.HasIndex(s => s.SalesOrderId)
            .HasDatabaseName("IX_SOShipments_SalesOrderId");

        builder.HasIndex(s => s.CustomerId)
            .HasDatabaseName("IX_SOShipments_CustomerId");

        builder.HasIndex(s => new { s.CompanyId, s.ShipmentNumber })
            .IsUnique()
            .HasDatabaseName("IX_SOShipments_CompanyId_ShipmentNumber");

        builder.HasIndex(s => s.ShipDate)
            .HasDatabaseName("IX_SOShipments_ShipDate");

        // Relationships - ALL use Restrict to prevent cascade cycles
        builder.HasOne(s => s.Company)
            .WithMany()
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.SalesOrder)
            .WithMany(so => so.Shipments)
            .HasForeignKey(s => s.SalesOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Warehouse)
            .WithMany()
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // LineItems - Cascade OK for true parent-child detail records
        builder.HasMany(s => s.LineItems)
            .WithOne(li => li.Shipment)
            .HasForeignKey(li => li.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}

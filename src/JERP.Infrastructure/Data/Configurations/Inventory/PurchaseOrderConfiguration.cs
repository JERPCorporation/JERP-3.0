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

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(po => po.Id);

        builder.Property(po => po.PONumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(po => po.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(po => po.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(po => po.ShippingAmount)
            .HasPrecision(18, 2);

        builder.Property(po => po.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(po => po.Status)
            .HasConversion<string>();

        builder.Property(po => po.Notes)
            .HasMaxLength(500);

        builder.Property(po => po.VendorPONumber)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(po => po.CompanyId)
            .HasDatabaseName("IX_PurchaseOrders_CompanyId");

        builder.HasIndex(po => new { po.CompanyId, po.PONumber })
            .IsUnique()
            .HasDatabaseName("IX_PurchaseOrders_CompanyId_PONumber");

        builder.HasIndex(po => po.VendorId)
            .HasDatabaseName("IX_PurchaseOrders_VendorId");

        builder.HasIndex(po => po.WarehouseId)
            .HasDatabaseName("IX_PurchaseOrders_WarehouseId");

        builder.HasIndex(po => po.Status)
            .HasDatabaseName("IX_PurchaseOrders_Status");

        builder.HasIndex(po => po.OrderDate)
            .HasDatabaseName("IX_PurchaseOrders_OrderDate");

        // Relationships
        builder.HasOne(po => po.Company)
            .WithMany()
            .HasForeignKey(po => po.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.Vendor)
            .WithMany()
            .HasForeignKey(po => po.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.Warehouse)
            .WithMany()
            .HasForeignKey(po => po.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.VendorBill)
            .WithMany()
            .HasForeignKey(po => po.VendorBillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(po => po.Lines)
            .WithOne(l => l.PurchaseOrder)
            .HasForeignKey(l => l.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.Receipts)
            .WithOne(r => r.PurchaseOrder)
            .HasForeignKey(r => r.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(po => !po.IsDeleted);
    }
}

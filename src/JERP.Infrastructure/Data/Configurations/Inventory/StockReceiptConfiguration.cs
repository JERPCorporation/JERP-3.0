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

public class StockReceiptConfiguration : IEntityTypeConfiguration<StockReceipt>
{
    public void Configure(EntityTypeBuilder<StockReceipt> builder)
    {
        builder.ToTable("StockReceipts");

        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(sr => sr.Notes)
            .HasMaxLength(1000);

        builder.Property(sr => sr.QCNotes)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(sr => sr.CompanyId)
            .HasDatabaseName("IX_StockReceipts_CompanyId");

        builder.HasIndex(sr => new { sr.CompanyId, sr.ReceiptNumber })
            .IsUnique()
            .HasDatabaseName("IX_StockReceipts_CompanyId_ReceiptNumber");

        builder.HasIndex(sr => sr.PurchaseOrderId)
            .HasDatabaseName("IX_StockReceipts_PurchaseOrderId");

        builder.HasIndex(sr => sr.WarehouseId)
            .HasDatabaseName("IX_StockReceipts_WarehouseId");

        builder.HasIndex(sr => sr.ReceiptDate)
            .HasDatabaseName("IX_StockReceipts_ReceiptDate");

        // Relationships
        builder.HasOne(sr => sr.Company)
            .WithMany()
            .HasForeignKey(sr => sr.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.PurchaseOrder)
            .WithMany(po => po.Receipts)
            .HasForeignKey(sr => sr.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.Warehouse)
            .WithMany()
            .HasForeignKey(sr => sr.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.JournalEntry)
            .WithMany()
            .HasForeignKey(sr => sr.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sr => sr.Lines)
            .WithOne(l => l.Receipt)
            .HasForeignKey(l => l.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(sr => !sr.IsDeleted);
    }
}

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

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

public class VendorBillConfiguration : IEntityTypeConfiguration<VendorBill>
{
    public void Configure(EntityTypeBuilder<VendorBill> builder)
    {
        builder.ToTable("VendorBills");

        builder.HasKey(vb => vb.Id);

        builder.Property(vb => vb.BillNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(vb => vb.VendorInvoiceNumber)
            .HasMaxLength(50);

        builder.Property(vb => vb.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(vb => vb.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(vb => vb.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(vb => vb.AmountPaid)
            .HasPrecision(18, 2);

        builder.Property(vb => vb.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(vb => vb.CompanyId)
            .HasDatabaseName("IX_VendorBills_CompanyId");

        builder.HasIndex(vb => vb.VendorId)
            .HasDatabaseName("IX_VendorBills_VendorId");

        builder.HasIndex(vb => new { vb.CompanyId, vb.BillNumber })
            .IsUnique()
            .HasDatabaseName("IX_VendorBills_CompanyId_BillNumber");

        builder.HasIndex(vb => vb.DueDate)
            .HasDatabaseName("IX_VendorBills_DueDate");

        // Relationships
        builder.HasOne(vb => vb.Company)
            .WithMany()
            .HasForeignKey(vb => vb.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vb => vb.Vendor)
            .WithMany(v => v.Bills)
            .HasForeignKey(vb => vb.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vb => vb.JournalEntry)
            .WithMany()
            .HasForeignKey(vb => vb.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(vb => !vb.IsDeleted);
    }
}

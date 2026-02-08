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

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

public class BillPaymentConfiguration : IEntityTypeConfiguration<BillPayment>
{
    public void Configure(EntityTypeBuilder<BillPayment> builder)
    {
        builder.ToTable("BillPayments");

        builder.HasKey(bp => bp.Id);

        builder.Property(bp => bp.PaymentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(bp => bp.Amount)
            .HasPrecision(18, 2);

        builder.Property(bp => bp.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(bp => bp.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(bp => bp.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(bp => bp.CompanyId)
            .HasDatabaseName("IX_BillPayments_CompanyId");

        builder.HasIndex(bp => bp.BillId)
            .HasDatabaseName("IX_BillPayments_BillId");

        builder.HasIndex(bp => new { bp.CompanyId, bp.PaymentNumber })
            .IsUnique()
            .HasDatabaseName("IX_BillPayments_CompanyId_PaymentNumber");

        builder.HasIndex(bp => bp.PaymentDate)
            .HasDatabaseName("IX_BillPayments_PaymentDate");

        // Relationships - ALL use Restrict for financial records
        // This prevents cascade delete cycles through Company
        builder.HasOne(bp => bp.Company)
            .WithMany()
            .HasForeignKey(bp => bp.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Bill)
            .WithMany(b => b.Payments)
            .HasForeignKey(bp => bp.BillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.JournalEntry)
            .WithMany()
            .HasForeignKey(bp => bp.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(bp => !bp.IsDeleted);
    }
}

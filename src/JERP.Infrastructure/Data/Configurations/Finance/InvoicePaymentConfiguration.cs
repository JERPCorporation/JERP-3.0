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

public class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder.ToTable("InvoicePayments");

        builder.HasKey(ip => ip.Id);

        builder.Property(ip => ip.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(ip => ip.Amount)
            .HasPrecision(18, 2);

        builder.Property(ip => ip.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(ip => ip.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(ip => ip.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(ip => ip.CompanyId)
            .HasDatabaseName("IX_InvoicePayments_CompanyId");

        builder.HasIndex(ip => ip.InvoiceId)
            .HasDatabaseName("IX_InvoicePayments_InvoiceId");

        builder.HasIndex(ip => new { ip.CompanyId, ip.ReceiptNumber })
            .IsUnique()
            .HasDatabaseName("IX_InvoicePayments_CompanyId_ReceiptNumber");

        builder.HasIndex(ip => ip.PaymentDate)
            .HasDatabaseName("IX_InvoicePayments_PaymentDate");

        // Relationships - ALL use Restrict for financial records
        // This prevents cascade delete cycles through Company
        builder.HasOne(ip => ip.Company)
            .WithMany()
            .HasForeignKey(ip => ip.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ip => ip.Invoice)
            .WithMany(i => i.Payments)
            .HasForeignKey(ip => ip.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ip => ip.JournalEntry)
            .WithMany()
            .HasForeignKey(ip => ip.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(ip => !ip.IsDeleted);
    }
}

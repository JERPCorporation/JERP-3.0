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

public class CustomerInvoiceConfiguration : IEntityTypeConfiguration<CustomerInvoice>
{
    public void Configure(EntityTypeBuilder<CustomerInvoice> builder)
    {
        builder.ToTable("CustomerInvoices");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(ci => ci.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(ci => ci.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(ci => ci.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(ci => ci.AmountPaid)
            .HasPrecision(18, 2);

        builder.Property(ci => ci.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(ci => ci.CompanyId)
            .HasDatabaseName("IX_CustomerInvoices_CompanyId");

        builder.HasIndex(ci => ci.CustomerId)
            .HasDatabaseName("IX_CustomerInvoices_CustomerId");

        builder.HasIndex(ci => new { ci.CompanyId, ci.InvoiceNumber })
            .IsUnique()
            .HasDatabaseName("IX_CustomerInvoices_CompanyId_InvoiceNumber");

        builder.HasIndex(ci => ci.DueDate)
            .HasDatabaseName("IX_CustomerInvoices_DueDate");

        // Relationships - ALL use Restrict for financial records
        builder.HasOne(ci => ci.Company)
            .WithMany()
            .HasForeignKey(ci => ci.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(ci => ci.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.JournalEntry)
            .WithMany()
            .HasForeignKey(ci => ci.JournalEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        // LineItems - Cascade OK for true parent-child detail records
        builder.HasMany(ci => ci.LineItems)
            .WithOne(li => li.Invoice)
            .HasForeignKey(li => li.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Payments - Restrict because payments are independent financial records
        builder.HasMany(ci => ci.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(ci => !ci.IsDeleted);
    }
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Finance;

/// <summary>
/// Entity Framework configuration for the Payment entity
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CompanyId)
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(p => p.Memo)
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired();

        // Indexes
        builder.HasIndex(p => p.CompanyId);
        builder.HasIndex(p => p.BankAccountId);
        builder.HasIndex(p => p.PaymentDate);
        builder.HasIndex(p => p.Status);

        // Relationships
        builder.HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.BankAccount)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BankAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.JournalEntry)
            .WithMany()
            .HasForeignKey(p => p.JournalEntryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.BillPayments)
            .WithOne(bp => bp.Payment)
            .HasForeignKey(bp => bp.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.InvoicePayments)
            .WithOne(ip => ip.Payment)
            .HasForeignKey(ip => ip.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

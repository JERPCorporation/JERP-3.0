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
/// Entity Framework configuration for the CashReconciliation entity
/// </summary>
public class CashReconciliationConfiguration : IEntityTypeConfiguration<CashReconciliation>
{
    public void Configure(EntityTypeBuilder<CashReconciliation> builder)
    {
        builder.ToTable("CashReconciliations");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Location)
            .HasMaxLength(100);

        builder.Property(c => c.ReconciliationDate)
            .IsRequired();

        builder.Property(c => c.ExpectedBalance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.ActualBalance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.Variance)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(c => c.CompanyId);
        builder.HasIndex(c => c.ReconciliationDate);
        builder.HasIndex(c => c.Location);

        // Relationships
        builder.HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ReconciliatedBy)
            .WithMany()
            .HasForeignKey(c => c.ReconciliatedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.JournalEntry)
            .WithMany()
            .HasForeignKey(c => c.JournalEntryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Query filter for soft delete
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}

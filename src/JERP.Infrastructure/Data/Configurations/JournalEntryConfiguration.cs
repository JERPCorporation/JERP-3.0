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

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the JournalEntry entity
/// </summary>
public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("JournalEntries");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.JournalEntryNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(j => j.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(j => j.Source)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(j => j.TotalDebit)
            .HasPrecision(18, 2);

        builder.Property(j => j.TotalCredit)
            .HasPrecision(18, 2);

        builder.Property(j => j.TransactionDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(j => j.CompanyId)
            .HasDatabaseName("IX_JournalEntries_CompanyId");

        builder.HasIndex(j => new { j.CompanyId, j.JournalEntryNumber })
            .IsUnique()
            .HasDatabaseName("IX_JournalEntries_CompanyId_JournalEntryNumber");

        builder.HasIndex(j => j.TransactionDate)
            .HasDatabaseName("IX_JournalEntries_TransactionDate");

        builder.HasIndex(j => j.Status)
            .HasDatabaseName("IX_JournalEntries_Status");

        builder.HasIndex(j => j.Source)
            .HasDatabaseName("IX_JournalEntries_Source");

        // Relationships
        builder.HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.LedgerEntries)
            .WithOne(e => e.JournalEntry)
            .HasForeignKey(e => e.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(j => !j.IsDeleted);
    }
}

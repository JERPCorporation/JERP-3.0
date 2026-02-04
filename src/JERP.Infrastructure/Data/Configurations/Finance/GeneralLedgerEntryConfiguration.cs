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
/// Entity Framework configuration for the GeneralLedgerEntry entity
/// </summary>
public class GeneralLedgerEntryConfiguration : IEntityTypeConfiguration<GeneralLedgerEntry>
{
    public void Configure(EntityTypeBuilder<GeneralLedgerEntry> builder)
    {
        builder.ToTable("GeneralLedgerEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.JournalEntryId)
            .IsRequired();

        builder.Property(e => e.AccountId)
            .IsRequired();

        builder.Property(e => e.EntryType)
            .IsRequired();

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(e => e.JournalEntryId);
        builder.HasIndex(e => e.AccountId);
        builder.HasIndex(e => e.EntryType);

        // Relationships
        builder.HasOne(e => e.JournalEntry)
            .WithMany(j => j.LedgerEntries)
            .HasForeignKey(e => e.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Account)
            .WithMany(a => a.LedgerEntries)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

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
/// Entity Framework configuration for the JournalEntry entity
/// </summary>
public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("JournalEntries");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.CompanyId)
            .IsRequired();

        builder.Property(j => j.EntryDate)
            .IsRequired();

        builder.Property(j => j.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(j => j.Description)
            .HasMaxLength(500);

        builder.Property(j => j.Status)
            .IsRequired();

        builder.Property(j => j.SourceEntityType)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(j => j.CompanyId);
        builder.HasIndex(j => j.EntryDate);
        builder.HasIndex(j => j.Status);
        builder.HasIndex(j => j.ReferenceNumber);
        builder.HasIndex(j => new { j.SourceEntityType, j.SourceEntityId });

        // Relationships
        builder.HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.PostedBy)
            .WithMany()
            .HasForeignKey(j => j.PostedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(j => j.LedgerEntries)
            .WithOne(e => e.JournalEntry)
            .HasForeignKey(e => e.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(j => !j.IsDeleted);
    }
}

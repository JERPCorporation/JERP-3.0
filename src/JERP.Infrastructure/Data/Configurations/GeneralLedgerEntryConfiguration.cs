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
/// Entity Framework configuration for the GeneralLedgerEntry entity
/// </summary>
public class GeneralLedgerEntryConfiguration : IEntityTypeConfiguration<GeneralLedgerEntry>
{
    public void Configure(EntityTypeBuilder<GeneralLedgerEntry> builder)
    {
        builder.ToTable("GeneralLedgerEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.DebitAmount)
            .HasPrecision(18, 2);

        builder.Property(e => e.CreditAmount)
            .HasPrecision(18, 2);

        builder.Property(e => e.Source)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.TransactionDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.CompanyId)
            .HasDatabaseName("IX_GeneralLedgerEntries_CompanyId");

        builder.HasIndex(e => e.AccountId)
            .HasDatabaseName("IX_GeneralLedgerEntries_AccountId");

        builder.HasIndex(e => e.JournalEntryId)
            .HasDatabaseName("IX_GeneralLedgerEntries_JournalEntryId");

        builder.HasIndex(e => e.TransactionDate)
            .HasDatabaseName("IX_GeneralLedgerEntries_TransactionDate");

        builder.HasIndex(e => new { e.CompanyId, e.AccountId, e.TransactionDate })
            .HasDatabaseName("IX_GeneralLedgerEntries_CompanyId_AccountId_TransactionDate");

        builder.HasIndex(e => e.SourceEntityId)
            .HasDatabaseName("IX_GeneralLedgerEntries_SourceEntityId");

        // Relationships
        builder.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Account)
            .WithMany(a => a.LedgerEntries)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.JournalEntry)
            .WithMany(j => j.LedgerEntries)
            .HasForeignKey(e => e.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

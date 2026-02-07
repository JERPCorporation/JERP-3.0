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
/// Entity Framework configuration for the Account entity
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AccountNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.AccountName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.Balance)
            .HasPrecision(18, 2);

        builder.Property(a => a.TaxCategory)
            .HasMaxLength(100);

        builder.Property(a => a.FASBTopicId)
            .IsRequired();

        builder.Property(a => a.FASBSubtopicId)
            .IsRequired();

        builder.Property(a => a.FASBReference)
            .IsRequired()
            .HasMaxLength(20);

        // Indexes
        builder.HasIndex(a => a.CompanyId)
            .HasDatabaseName("IX_Accounts_CompanyId");

        builder.HasIndex(a => new { a.CompanyId, a.AccountNumber })
            .IsUnique()
            .HasDatabaseName("IX_Accounts_CompanyId_AccountNumber");

        builder.HasIndex(a => a.Type)
            .HasDatabaseName("IX_Accounts_Type");

        builder.HasIndex(a => a.FASBReference)
            .HasDatabaseName("IX_Accounts_FASBReference");

        builder.HasIndex(a => a.FASBTopicId)
            .HasDatabaseName("IX_Accounts_FASBTopicId");

        builder.HasIndex(a => a.FASBSubtopicId)
            .HasDatabaseName("IX_Accounts_FASBSubtopicId");

        // Relationships
        builder.HasOne(a => a.Company)
            .WithMany()
            .HasForeignKey(a => a.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.LedgerEntries)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.FASBTopic)
            .WithMany(t => t.Accounts)
            .HasForeignKey(a => a.FASBTopicId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(a => a.FASBSubtopic)
            .WithMany(s => s.Accounts)
            .HasForeignKey(a => a.FASBSubtopicId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Query filter for soft delete
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}

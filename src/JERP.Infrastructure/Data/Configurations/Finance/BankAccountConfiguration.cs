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
/// Entity Framework configuration for the BankAccount entity
/// </summary>
public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BankAccounts");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CompanyId)
            .IsRequired();

        builder.Property(b => b.AccountId)
            .IsRequired();

        builder.Property(b => b.BankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.AccountName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.AccountNumberLast4)
            .HasMaxLength(4);

        builder.Property(b => b.RoutingNumber)
            .HasMaxLength(9);

        builder.Property(b => b.AccountType)
            .HasMaxLength(50);

        builder.Property(b => b.Balance)
            .HasPrecision(18, 2);

        builder.Property(b => b.IsActive)
            .IsRequired();

        // Indexes
        builder.HasIndex(b => b.CompanyId);
        builder.HasIndex(b => b.AccountId);
        builder.HasIndex(b => b.IsActive);

        // Relationships
        builder.HasOne(b => b.Company)
            .WithMany()
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Account)
            .WithMany()
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Payments)
            .WithOne(p => p.BankAccount)
            .HasForeignKey(p => p.BankAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Query filter for soft delete
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}

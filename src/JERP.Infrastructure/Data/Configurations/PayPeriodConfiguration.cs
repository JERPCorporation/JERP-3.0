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

using JERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the PayPeriod entity
/// </summary>
public class PayPeriodConfiguration : IEntityTypeConfiguration<PayPeriod>
{
    public void Configure(EntityTypeBuilder<PayPeriod> builder)
    {
        builder.ToTable("PayPeriods");

        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.CompanyId)
            .IsRequired();

        builder.Property(pp => pp.StartDate)
            .IsRequired();

        builder.Property(pp => pp.EndDate)
            .IsRequired();

        builder.Property(pp => pp.PayDate)
            .IsRequired();

        builder.Property(pp => pp.Status)
            .IsRequired();

        builder.Property(pp => pp.Frequency)
            .IsRequired();

        builder.Property(pp => pp.TotalGrossPay)
            .HasPrecision(18, 2);

        builder.Property(pp => pp.TotalNetPay)
            .HasPrecision(18, 2);

        builder.Property(pp => pp.TotalTaxes)
            .HasPrecision(18, 2);

        builder.Property(pp => pp.TotalDeductions)
            .HasPrecision(18, 2);

        builder.Property(pp => pp.ProcessedAt)
            .IsRequired(false);

        builder.Property(pp => pp.ApprovedAt)
            .IsRequired(false);

        builder.Property(pp => pp.ApprovedById)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(pp => pp.CompanyId);

        builder.HasIndex(pp => pp.StartDate);

        builder.HasIndex(pp => pp.EndDate);

        builder.HasIndex(pp => pp.Status);

        // Relationships
        builder.HasOne(pp => pp.Company)
            .WithMany()
            .HasForeignKey(pp => pp.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pp => pp.PayrollRecords)
            .WithOne(pr => pr.PayPeriod)
            .HasForeignKey(pr => pr.PayPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pp => pp.ApprovedBy)
            .WithMany()
            .HasForeignKey(pp => pp.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(pp => !pp.IsDeleted);
    }
}

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
/// Entity Framework configuration for the TaxWithholding entity
/// </summary>
public class TaxWithholdingConfiguration : IEntityTypeConfiguration<TaxWithholding>
{
    public void Configure(EntityTypeBuilder<TaxWithholding> builder)
    {
        builder.ToTable("TaxWithholdings");

        builder.HasKey(tw => tw.Id);

        builder.Property(tw => tw.EmployeeId)
            .IsRequired();

        builder.Property(tw => tw.TaxYear)
            .IsRequired();

        builder.Property(tw => tw.FilingStatus)
            .IsRequired();

        builder.Property(tw => tw.FederalAllowances)
            .IsRequired();

        builder.Property(tw => tw.FederalExtraWithholding)
            .HasPrecision(18, 2);

        builder.Property(tw => tw.StateAllowances)
            .IsRequired();

        builder.Property(tw => tw.StateExtraWithholding)
            .HasPrecision(18, 2);

        builder.Property(tw => tw.IsExemptFederal)
            .IsRequired();

        builder.Property(tw => tw.IsExemptState)
            .IsRequired();

        builder.Property(tw => tw.EffectiveDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(tw => tw.EmployeeId);

        builder.HasIndex(tw => tw.TaxYear);

        // Relationships
        builder.HasOne(tw => tw.Employee)
            .WithMany(e => e.TaxWithholdings)
            .HasForeignKey(tw => tw.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(tw => !tw.IsDeleted);
    }
}

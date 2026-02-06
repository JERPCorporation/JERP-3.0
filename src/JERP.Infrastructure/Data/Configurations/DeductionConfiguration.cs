/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the Deduction entity
/// </summary>
public class DeductionConfiguration : IEntityTypeConfiguration<Deduction>
{
    public void Configure(EntityTypeBuilder<Deduction> builder)
    {
        builder.ToTable("Deductions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.EmployeeId)
            .IsRequired();

        builder.Property(d => d.DeductionType)
            .IsRequired();

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Amount)
            .HasPrecision(18, 2);

        builder.Property(d => d.IsPercentage)
            .IsRequired();

        builder.Property(d => d.IsPreTax)
            .IsRequired();

        builder.Property(d => d.Priority)
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired();

        builder.Property(d => d.StartDate)
            .IsRequired();

        builder.Property(d => d.EndDate)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(d => d.EmployeeId);

        builder.HasIndex(d => d.IsActive);

        // Relationships
        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Deductions)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}

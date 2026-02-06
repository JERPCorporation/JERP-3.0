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
/// Entity Framework configuration for the ComplianceViolation entity
/// </summary>
public class ComplianceViolationConfiguration : IEntityTypeConfiguration<ComplianceViolation>
{
    public void Configure(EntityTypeBuilder<ComplianceViolation> builder)
    {
        builder.ToTable("ComplianceViolations");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.ViolationType)
            .IsRequired();

        builder.Property(cv => cv.Severity)
            .IsRequired();

        builder.Property(cv => cv.Status)
            .IsRequired();

        builder.Property(cv => cv.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cv => cv.EntityId)
            .IsRequired();

        builder.Property(cv => cv.RuleName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cv => cv.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cv => cv.Details)
            .HasColumnType("nvarchar(max)");

        builder.Property(cv => cv.FinancialImpact)
            .HasPrecision(18, 2);

        builder.Property(cv => cv.DetectedAt)
            .IsRequired();

        builder.Property(cv => cv.ResolvedAt)
            .IsRequired(false);

        builder.Property(cv => cv.ResolvedById)
            .IsRequired(false);

        builder.Property(cv => cv.ResolutionNotes)
            .HasMaxLength(2000);

        // Indexes
        builder.HasIndex(cv => cv.ViolationType);

        builder.HasIndex(cv => cv.Severity);

        builder.HasIndex(cv => cv.Status);

        builder.HasIndex(cv => cv.DetectedAt);

        // Relationships
        builder.HasOne(cv => cv.ResolvedBy)
            .WithMany()
            .HasForeignKey(cv => cv.ResolvedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(cv => !cv.IsDeleted);
    }
}

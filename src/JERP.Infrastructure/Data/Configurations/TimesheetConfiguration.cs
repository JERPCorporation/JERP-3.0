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

using JERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the Timesheet entity
/// </summary>
public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
    {
        builder.ToTable("Timesheets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.EmployeeId)
            .IsRequired();

        builder.Property(t => t.WorkDate)
            .IsRequired();

        builder.Property(t => t.ClockIn)
            .IsRequired(false);

        builder.Property(t => t.ClockOut)
            .IsRequired(false);

        builder.Property(t => t.BreakMinutes)
            .IsRequired();

        builder.Property(t => t.TotalHours)
            .HasPrecision(18, 2);

        builder.Property(t => t.RegularHours)
            .HasPrecision(18, 2);

        builder.Property(t => t.OvertimeHours)
            .HasPrecision(18, 2);

        builder.Property(t => t.DoubleTimeHours)
            .HasPrecision(18, 2);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Notes)
            .HasMaxLength(1000);

        builder.Property(t => t.SubmittedAt)
            .IsRequired(false);

        builder.Property(t => t.ApprovedAt)
            .IsRequired(false);

        builder.Property(t => t.ApprovedById)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(t => t.EmployeeId);

        builder.HasIndex(t => t.WorkDate);

        builder.HasIndex(t => t.Status);

        // Relationships
        builder.HasOne(t => t.Employee)
            .WithMany(e => e.Timesheets)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ApprovedBy)
            .WithMany()
            .HasForeignKey(t => t.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}

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
/// Entity Framework configuration for the AuditLog entity
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(al => al.Id);

        // Required fields for hash-chain audit log
        builder.Property(al => al.CompanyId)
            .IsRequired();

        builder.Property(al => al.Timestamp)
            .IsRequired();

        builder.Property(al => al.UserId)
            .IsRequired();

        builder.Property(al => al.UserEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(al => al.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(al => al.Resource)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(al => al.Details)
            .HasMaxLength(2000);

        builder.Property(al => al.IpAddress)
            .HasMaxLength(50);

        builder.Property(al => al.PreviousHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(al => al.CurrentHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(al => al.SequenceNumber)
            .IsRequired();

        // Legacy fields - optional
        builder.Property(al => al.EntityType)
            .HasMaxLength(100);

        builder.Property(al => al.OldValues)
            .HasColumnType("jsonb");

        builder.Property(al => al.NewValues)
            .HasColumnType("jsonb");

        builder.Property(al => al.UserAgent)
            .HasMaxLength(500);

        // Indexes for efficient querying
        builder.HasIndex(al => new { al.CompanyId, al.SequenceNumber })
            .IsUnique();

        builder.HasIndex(al => al.Timestamp);

        builder.HasIndex(al => al.Action);

        builder.HasIndex(al => al.UserId);

        // Relationships
        builder.HasOne(al => al.Company)
            .WithMany()
            .HasForeignKey(al => al.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(al => !al.IsDeleted);
    }
}

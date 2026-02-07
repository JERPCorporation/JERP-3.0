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
/// Entity Framework configuration for the FASBSubtopic entity
/// </summary>
public class FASBSubtopicConfiguration : IEntityTypeConfiguration<FASBSubtopic>
{
    public void Configure(EntityTypeBuilder<FASBSubtopic> builder)
    {
        builder.ToTable("FASBSubtopics");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SubtopicCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(s => s.SubtopicName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.FullReference)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(s => s.FullReference)
            .IsUnique()
            .HasDatabaseName("IX_FASBSubtopics_FullReference");

        builder.HasIndex(s => s.FASBTopicId)
            .HasDatabaseName("IX_FASBSubtopics_FASBTopicId");

        builder.HasIndex(s => new { s.FASBTopicId, s.SubtopicCode })
            .IsUnique()
            .HasDatabaseName("IX_FASBSubtopics_TopicId_SubtopicCode");

        // Relationships
        builder.HasOne(s => s.Topic)
            .WithMany(t => t.Subtopics)
            .HasForeignKey(s => s.FASBTopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Accounts)
            .WithOne(a => a.FASBSubtopic)
            .HasForeignKey(a => a.FASBSubtopicId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}

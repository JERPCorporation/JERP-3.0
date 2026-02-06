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

using JERP.Core.Entities.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the FASBTopic entity
/// </summary>
public class FASBTopicConfiguration : IEntityTypeConfiguration<FASBTopic>
{
    public void Configure(EntityTypeBuilder<FASBTopic> builder)
    {
        builder.ToTable("FASBTopics");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TopicCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.TopicName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.SupersededBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(t => t.TopicCode)
            .IsUnique()
            .HasDatabaseName("IX_FASBTopics_TopicCode");

        builder.HasIndex(t => t.Category)
            .HasDatabaseName("IX_FASBTopics_Category");

        // Relationships
        builder.HasMany(t => t.Subtopics)
            .WithOne(s => s.Topic)
            .HasForeignKey(s => s.FASBTopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Accounts)
            .WithOne(a => a.FASBTopic)
            .HasForeignKey(a => a.FASBTopicId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}

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

using JERP.Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations.Inventory;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pc => pc.Description)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(pc => pc.CompanyId)
            .HasDatabaseName("IX_ProductCategories_CompanyId");

        builder.HasIndex(pc => new { pc.CompanyId, pc.Name })
            .IsUnique()
            .HasDatabaseName("IX_ProductCategories_CompanyId_Name");

        builder.HasIndex(pc => pc.ParentCategoryId)
            .HasDatabaseName("IX_ProductCategories_ParentCategoryId");

        // Relationships
        builder.HasOne(pc => pc.Company)
            .WithMany()
            .HasForeignKey(pc => pc.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pc => pc.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(pc => pc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pc => pc.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(pc => !pc.IsDeleted);
    }
}

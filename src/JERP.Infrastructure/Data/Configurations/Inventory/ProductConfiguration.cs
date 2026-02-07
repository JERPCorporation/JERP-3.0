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

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Brand)
            .HasMaxLength(100);

        builder.Property(p => p.UnitOfMeasure)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Barcode)
            .HasMaxLength(50);

        builder.Property(p => p.THCPercentage)
            .HasPrecision(5, 2);

        builder.Property(p => p.CBDPercentage)
            .HasPrecision(5, 2);

        builder.Property(p => p.StrainType)
            .HasMaxLength(50);

        builder.Property(p => p.CannabisLicense)
            .HasMaxLength(50);

        builder.Property(p => p.StandardCost)
            .HasPrecision(18, 2);

        builder.Property(p => p.RetailPrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.WholesalePrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.ValuationMethod)
            .HasConversion<string>();

        builder.Property(p => p.StorageConditions)
            .HasMaxLength(100);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(p => p.CompanyId)
            .HasDatabaseName("IX_Products_CompanyId");

        builder.HasIndex(p => new { p.CompanyId, p.SKU })
            .IsUnique()
            .HasDatabaseName("IX_Products_CompanyId_SKU");

        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("IX_Products_CategoryId");

        builder.HasIndex(p => p.Barcode)
            .HasDatabaseName("IX_Products_Barcode");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");

        // Relationships
        builder.HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.InventoryAssetAccount)
            .WithMany()
            .HasForeignKey(p => p.InventoryAssetAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.COGSAccount)
            .WithMany()
            .HasForeignKey(p => p.COGSAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.RevenueAccount)
            .WithMany()
            .HasForeignKey(p => p.RevenueAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.DefaultWarehouse)
            .WithMany()
            .HasForeignKey(p => p.DefaultWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.InventoryLevels)
            .WithOne(il => il.Product)
            .HasForeignKey(il => il.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Batches)
            .WithOne(b => b.Product)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter for soft delete
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}

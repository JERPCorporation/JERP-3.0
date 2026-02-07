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
/// Entity Framework configuration for the PayrollRecord entity
/// </summary>
public class PayrollRecordConfiguration : IEntityTypeConfiguration<PayrollRecord>
{
    public void Configure(EntityTypeBuilder<PayrollRecord> builder)
    {
        builder.ToTable("PayrollRecords");

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.PayPeriodId)
            .IsRequired();

        builder.Property(pr => pr.EmployeeId)
            .IsRequired();

        builder.Property(pr => pr.Status)
            .IsRequired();

        // Hours
        builder.Property(pr => pr.RegularHours)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.OvertimeHours)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.DoubleTimeHours)
            .HasPrecision(18, 2);

        // Earnings
        builder.Property(pr => pr.GrossPay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.RegularPay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.OvertimePay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.DoubleTimePay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.BonusPay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.CommissionPay)
            .HasPrecision(18, 2);

        // Taxes
        builder.Property(pr => pr.FederalTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.StateTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.SocialSecurityTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.MedicareTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.TotalTaxes)
            .HasPrecision(18, 2);

        // Deductions
        builder.Property(pr => pr.PreTaxDeductions)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.PostTaxDeductions)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.TotalDeductions)
            .HasPrecision(18, 2);

        // Net Pay
        builder.Property(pr => pr.NetPay)
            .HasPrecision(18, 2);

        // Year-to-Date Totals
        builder.Property(pr => pr.YTDGrossPay)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.YTDFederalTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.YTDStateTax)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.YTDSocialSecurity)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.YTDMedicare)
            .HasPrecision(18, 2);

        builder.Property(pr => pr.YTDNetPay)
            .HasPrecision(18, 2);

        // Timestamps
        builder.Property(pr => pr.CalculatedAt)
            .IsRequired(false);

        builder.Property(pr => pr.ApprovedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(pr => pr.PayPeriodId);

        builder.HasIndex(pr => pr.EmployeeId);

        builder.HasIndex(pr => pr.Status);

        // Relationships
        builder.HasOne(pr => pr.PayPeriod)
            .WithMany(pp => pp.PayrollRecords)
            .HasForeignKey(pr => pr.PayPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.Employee)
            .WithMany(e => e.PayrollRecords)
            .HasForeignKey(pr => pr.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pr => pr.Details)
            .WithOne(d => d.PayrollRecord)
            .HasForeignKey(d => d.PayrollRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(pr => !pr.IsDeleted);
    }
}

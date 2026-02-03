using JERP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JERP.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for the PayrollRecordDetail entity
/// </summary>
public class PayrollRecordDetailConfiguration : IEntityTypeConfiguration<PayrollRecordDetail>
{
    public void Configure(EntityTypeBuilder<PayrollRecordDetail> builder)
    {
        builder.ToTable("PayrollRecordDetails");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.PayrollRecordId)
            .IsRequired();

        builder.Property(d => d.Type)
            .IsRequired();

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Amount)
            .HasPrecision(18, 2);

        builder.Property(d => d.IsYTD)
            .IsRequired();

        // Indexes
        builder.HasIndex(d => d.PayrollRecordId);

        // Relationships
        builder.HasOne(d => d.PayrollRecord)
            .WithMany(pr => pr.Details)
            .HasForeignKey(d => d.PayrollRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query filter for soft delete
        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}

using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class FixedBillConfiguration : IEntityTypeConfiguration<FixedBill>
{
    public void Configure(EntityTypeBuilder<FixedBill> builder)
    {
        builder.ToTable("FixedBills");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.DueDay)
            .IsRequired();
            
        builder.Property(x => x.Month)
            .IsRequired();
            
        builder.Property(x => x.Year)
            .IsRequired();
            
        builder.Property(x => x.IsPaid)
            .IsRequired();
            
        builder.Property(x => x.IsRecurring)
            .IsRequired();
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt);
            
        builder.HasIndex(x => new { x.Month, x.Year });
    }
}

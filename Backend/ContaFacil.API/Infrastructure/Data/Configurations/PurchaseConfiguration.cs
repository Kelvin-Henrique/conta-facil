using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchases");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Date)
            .IsRequired();
            
        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.Installments)
            .IsRequired();
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt);
            
        builder.HasOne(x => x.CreditCard)
            .WithMany(x => x.Purchases)
            .HasForeignKey(x => x.CreditCardId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => x.CreditCardId);
        builder.HasIndex(x => x.Date);
    }
}

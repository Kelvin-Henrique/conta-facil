using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.ToTable("CreditCards");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.DueDay)
            .IsRequired();
            
        builder.Property(x => x.ClosingDay)
            .IsRequired();
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt);
            
        builder.HasMany(x => x.Purchases)
            .WithOne(x => x.CreditCard)
            .HasForeignKey(x => x.CreditCardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

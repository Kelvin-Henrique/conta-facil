using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BankAccounts");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.BankName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Balance)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt);
            
        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.BankAccount)
            .HasForeignKey(x => x.BankAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.ToTable("AccountTransactions");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Date)
            .IsRequired();
            
        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.CreatedAt)
            .IsRequired();
            
        builder.Property(x => x.UpdatedAt);
            
        builder.HasOne(x => x.BankAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.BankAccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => x.BankAccountId);
        builder.HasIndex(x => x.Date);
    }
}

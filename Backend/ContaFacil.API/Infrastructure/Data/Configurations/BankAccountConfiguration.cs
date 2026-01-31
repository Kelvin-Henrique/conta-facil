using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.ToTable("BankAccounts");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.NomeBanco)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Saldo)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.CriadoEm)
            .IsRequired();
            
        builder.Property(x => x.AtualizadoEm);
            
        builder.HasMany(x => x.Transacoes)
            .WithOne(x => x.ContaBancaria)
            .HasForeignKey(x => x.ContaBancariaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class AccountTransactionConfiguration : IEntityTypeConfiguration<TransacaoConta>
{
    public void Configure(EntityTypeBuilder<TransacaoConta> builder)
    {
        builder.ToTable("AccountTransactions");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Categoria)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Data)
            .IsRequired();
            
        builder.Property(x => x.Valor)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.CriadoEm)
            .IsRequired();
            
        builder.Property(x => x.AtualizadoEm);
            
        builder.HasOne(x => x.ContaBancaria)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.ContaBancariaId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => x.ContaBancariaId);
        builder.HasIndex(x => x.Data);
    }
}

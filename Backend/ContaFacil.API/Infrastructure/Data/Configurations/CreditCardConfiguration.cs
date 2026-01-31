using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CartaoCredito>
{
    public void Configure(EntityTypeBuilder<CartaoCredito> builder)
    {
        builder.ToTable("CreditCards");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.DiaVencimento)
            .IsRequired();
            
        builder.Property(x => x.DiaFechamento)
            .IsRequired();
            
        builder.Property(x => x.CriadoEm)
            .IsRequired();
            
        builder.Property(x => x.AtualizadoEm);
            
        builder.HasMany(x => x.Compras)
            .WithOne(x => x.CartaoCredito)
            .HasForeignKey(x => x.CartaoCreditoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

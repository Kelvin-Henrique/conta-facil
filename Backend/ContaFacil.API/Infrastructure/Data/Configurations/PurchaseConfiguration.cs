using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Compra>
{
    public void Configure(EntityTypeBuilder<Compra> builder)
    {
        builder.ToTable("Purchases");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Categoria)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Data)
            .IsRequired();
            
        builder.Property(x => x.ValorTotal)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.Parcelas)
            .IsRequired();
            
        builder.Property(x => x.CriadoEm)
            .IsRequired();
            
        builder.Property(x => x.AtualizadoEm);
            
        builder.HasOne(x => x.CartaoCredito)
            .WithMany(x => x.Compras)
            .HasForeignKey(x => x.CartaoCreditoId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => x.CartaoCreditoId);
        builder.HasIndex(x => x.Data);
    }
}

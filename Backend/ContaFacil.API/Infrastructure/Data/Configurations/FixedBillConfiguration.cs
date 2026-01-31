using ContaFacil.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContaFacil.API.Infrastructure.Data.Configurations;

public class FixedBillConfiguration : IEntityTypeConfiguration<ContaFixa>
{
    public void Configure(EntityTypeBuilder<ContaFixa> builder)
    {
        builder.ToTable("FixedBills");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Categoria)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Valor)
            .HasPrecision(18, 2);
            
        builder.Property(x => x.DiaVencimento)
            .IsRequired();
            
        builder.Property(x => x.Mes)
            .IsRequired();
            
        builder.Property(x => x.Ano)
            .IsRequired();
            
        builder.Property(x => x.Pago)
            .IsRequired();
            
        builder.Property(x => x.Recorrente)
            .IsRequired();
            
        builder.Property(x => x.CriadoEm)
            .IsRequired();
            
        builder.Property(x => x.AtualizadoEm);
            
        builder.HasIndex(x => new { x.Mes, x.Ano });
    }
}

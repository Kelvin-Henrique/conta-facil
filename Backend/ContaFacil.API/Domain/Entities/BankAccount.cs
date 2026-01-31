namespace ContaFacil.API.Domain.Entities;

public class ContaBancaria
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NomeBanco { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    
    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
    public ICollection<TransacaoConta> Transacoes { get; set; } = new List<TransacaoConta>();
}

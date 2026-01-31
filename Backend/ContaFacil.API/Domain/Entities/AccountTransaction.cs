namespace ContaFacil.API.Domain.Entities;

public class TransacaoConta
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public Guid ContaBancariaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    
    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
    public ContaBancaria ContaBancaria { get; set; } = null!;
}

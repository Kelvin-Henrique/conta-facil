namespace ContaFacil.API.Domain.Entities;

public class ContaFixa
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DiaVencimento { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public bool Pago { get; set; }
    public bool Recorrente { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    
    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
}

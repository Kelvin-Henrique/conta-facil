namespace ContaFacil.API.Domain.Entities;

public class CartaoCredito
{
    public Guid Id { get; set; }
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int DiaVencimento { get; set; }
    public int DiaFechamento { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    
    // Navigation Properties
    public Usuario Usuario { get; set; } = null!;
    public ICollection<Compra> Compras { get; set; } = new List<Compra>();
}

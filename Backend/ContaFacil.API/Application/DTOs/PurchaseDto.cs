namespace ContaFacil.API.Application.DTOs;

public class CompraDto
{
    public Guid Id { get; set; }
    public Guid CartaoCreditoId { get; set; }
    public string NomeCartaoCredito { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public int Parcelas { get; set; }
}

public class CriarCompraDto
{
    public Guid CartaoCreditoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public int Parcelas { get; set; }
}

public class AtualizarCompraDto
{
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public int Parcelas { get; set; }
}

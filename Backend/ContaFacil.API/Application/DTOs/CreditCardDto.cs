namespace ContaFacil.API.Application.DTOs;

public class CartaoCreditoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int DiaVencimento { get; set; }
    public int DiaFechamento { get; set; }
}

public class CriarCartaoCreditoDto
{
    public string Nome { get; set; } = string.Empty;
    public int DiaVencimento { get; set; }
    public int DiaFechamento { get; set; }
}

public class AtualizarCartaoCreditoDto
{
    public string Nome { get; set; } = string.Empty;
    public int DiaVencimento { get; set; }
    public int DiaFechamento { get; set; }
}

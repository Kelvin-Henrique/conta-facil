namespace ContaFacil.API.Application.DTOs;

public class ContaFixaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DiaVencimento { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public bool Pago { get; set; }
    public bool Recorrente { get; set; }
}

public class CriarContaFixaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DiaVencimento { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public bool Pago { get; set; }
    public bool Recorrente { get; set; }
}

public class AtualizarContaFixaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int DiaVencimento { get; set; }
    public bool Pago { get; set; }
    public bool Recorrente { get; set; }
}

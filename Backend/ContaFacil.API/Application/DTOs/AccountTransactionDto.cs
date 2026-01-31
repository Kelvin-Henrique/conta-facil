namespace ContaFacil.API.Application.DTOs;

public class TransacaoContaDto
{
    public Guid Id { get; set; }
    public Guid ContaBancariaId { get; set; }
    public string NomeContaBancaria { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
}

public class CriarTransacaoContaDto
{
    public Guid ContaBancariaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
}

public class AtualizarTransacaoContaDto
{
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
}

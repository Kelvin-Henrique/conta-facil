namespace ContaFacil.API.Application.DTOs;

public class ContaBancariaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NomeBanco { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
}

public class CriarContaBancariaDto
{
    public string Nome { get; set; } = string.Empty;
    public string NomeBanco { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
}

public class AtualizarContaBancariaDto
{
    public string Nome { get; set; } = string.Empty;
    public string NomeBanco { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
}

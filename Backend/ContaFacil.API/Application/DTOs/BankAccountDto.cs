namespace ContaFacil.API.Application.DTOs;

public class BankAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class CreateBankAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class UpdateBankAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

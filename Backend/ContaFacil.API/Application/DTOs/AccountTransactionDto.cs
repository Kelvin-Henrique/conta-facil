namespace ContaFacil.API.Application.DTOs;

public class AccountTransactionDto
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public string BankAccountName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}

public class CreateAccountTransactionDto
{
    public Guid BankAccountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}

public class UpdateAccountTransactionDto
{
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}

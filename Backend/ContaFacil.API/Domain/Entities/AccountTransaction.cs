namespace ContaFacil.API.Domain.Entities;

public class AccountTransaction
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public Guid BankAccountId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public BankAccount BankAccount { get; set; } = null!;
}

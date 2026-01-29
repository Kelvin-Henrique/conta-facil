namespace ContaFacil.API.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<AccountTransaction> Transactions { get; set; } = new List<AccountTransaction>();
}

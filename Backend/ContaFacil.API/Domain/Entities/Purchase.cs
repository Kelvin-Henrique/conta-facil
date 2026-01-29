namespace ContaFacil.API.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public Guid CreditCardId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public User User { get; set; } = null!;
    public CreditCard CreditCard { get; set; } = null!;
}

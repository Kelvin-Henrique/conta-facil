namespace ContaFacil.API.Domain.Entities;

public class CreditCard
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueDay { get; set; }
    public int ClosingDay { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}

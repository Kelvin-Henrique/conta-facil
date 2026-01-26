namespace ContaFacil.API.Domain.Entities;

public class FixedBill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int DueDay { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IsPaid { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

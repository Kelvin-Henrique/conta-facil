namespace ContaFacil.API.Application.DTOs;

public class PurchaseDto
{
    public Guid Id { get; set; }
    public Guid CreditCardId { get; set; }
    public string CreditCardName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
}

public class CreatePurchaseDto
{
    public Guid CreditCardId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
}

public class UpdatePurchaseDto
{
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int Installments { get; set; }
}

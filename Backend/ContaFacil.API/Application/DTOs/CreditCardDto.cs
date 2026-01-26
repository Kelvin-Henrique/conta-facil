namespace ContaFacil.API.Application.DTOs;

public class CreditCardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DueDay { get; set; }
    public int ClosingDay { get; set; }
}

public class CreateCreditCardDto
{
    public string Name { get; set; } = string.Empty;
    public int DueDay { get; set; }
    public int ClosingDay { get; set; }
}

public class UpdateCreditCardDto
{
    public string Name { get; set; } = string.Empty;
    public int DueDay { get; set; }
    public int ClosingDay { get; set; }
}

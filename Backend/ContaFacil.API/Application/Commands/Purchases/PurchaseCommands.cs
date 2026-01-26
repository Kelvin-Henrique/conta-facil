using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.Purchases;

public record CreatePurchaseCommand(
    Guid CreditCardId,
    string Description,
    string Category,
    DateTime Date,
    decimal TotalAmount,
    int Installments
) : IRequest<PurchaseDto>;

public record UpdatePurchaseCommand(
    Guid Id,
    string Description,
    string Category,
    DateTime Date,
    decimal TotalAmount,
    int Installments
) : IRequest<PurchaseDto>;

public record DeletePurchaseCommand(Guid Id) : IRequest<bool>;

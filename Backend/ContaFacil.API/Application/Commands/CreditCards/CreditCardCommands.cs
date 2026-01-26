using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.CreditCards;

public record CreateCreditCardCommand(
    string Name,
    int DueDay,
    int ClosingDay
) : IRequest<CreditCardDto>;

public record UpdateCreditCardCommand(
    Guid Id,
    string Name,
    int DueDay,
    int ClosingDay
) : IRequest<CreditCardDto>;

public record DeleteCreditCardCommand(Guid Id) : IRequest<bool>;

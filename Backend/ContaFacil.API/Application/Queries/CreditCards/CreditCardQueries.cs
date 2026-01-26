using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.CreditCards;

public record GetAllCreditCardsQuery : IRequest<List<CreditCardDto>>;

public record GetCreditCardByIdQuery(Guid Id) : IRequest<CreditCardDto?>;

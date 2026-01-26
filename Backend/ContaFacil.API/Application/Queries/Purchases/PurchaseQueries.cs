using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.Purchases;

public record GetAllPurchasesQuery : IRequest<List<PurchaseDto>>;

public record GetPurchaseByIdQuery(Guid Id) : IRequest<PurchaseDto?>;

public record GetPurchasesByCreditCardQuery(Guid CreditCardId) : IRequest<List<PurchaseDto>>;

using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.CreditCards;

public record ObterTodosCartoesCreditoQuery : IRequest<List<CartaoCreditoDto>>;

public record ObterCartaoCreditoPorIdQuery(Guid Id) : IRequest<CartaoCreditoDto?>;

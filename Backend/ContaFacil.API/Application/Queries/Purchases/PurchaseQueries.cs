using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.Purchases;

public record ObterTodasComprasQuery : IRequest<List<CompraDto>>;

public record ObterCompraPorIdQuery(Guid Id) : IRequest<CompraDto?>;

public record ObterComprasPorCartaoCreditoQuery(Guid CartaoCreditoId) : IRequest<List<CompraDto>>;

using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.FixedBills;

public record ObterTodasContasFixasQuery : IRequest<List<ContaFixaDto>>;

public record ObterContaFixaPorIdQuery(Guid Id) : IRequest<ContaFixaDto?>;

public record ObterContasFixasPorMesAnoQuery(int Mes, int Ano) : IRequest<List<ContaFixaDto>>;

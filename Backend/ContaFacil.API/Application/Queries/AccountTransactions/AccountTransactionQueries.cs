using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.AccountTransactions;

public record ObterTodasTransacoesContaQuery : IRequest<List<TransacaoContaDto>>;

public record ObterTransacaoContaPorIdQuery(Guid Id) : IRequest<TransacaoContaDto?>;

public record ObterTransacoesContaPorContaBancariaQuery(Guid ContaBancariaId) : IRequest<List<TransacaoContaDto>>;

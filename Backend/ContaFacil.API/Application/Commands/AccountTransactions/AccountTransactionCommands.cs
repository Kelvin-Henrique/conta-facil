using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.AccountTransactions;

public record CriarTransacaoContaCommand(
    Guid ContaBancariaId,
    string Descricao,
    string Categoria,
    DateTime Data,
    decimal Valor
) : IRequest<TransacaoContaDto>;

public record AtualizarTransacaoContaCommand(
    Guid Id,
    string Descricao,
    string Categoria,
    DateTime Data,
    decimal Valor
) : IRequest<TransacaoContaDto>;

public record ExcluirTransacaoContaCommand(Guid Id) : IRequest<bool>;

using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.BankAccounts;

public record CriarContaBancariaCommand(
    string Nome,
    string NomeBanco,
    decimal Saldo
) : IRequest<ContaBancariaDto>;

public record AtualizarContaBancariaCommand(
    Guid Id,
    string Nome,
    string NomeBanco,
    decimal Saldo
) : IRequest<ContaBancariaDto>;

public record ExcluirContaBancariaCommand(Guid Id) : IRequest<bool>;

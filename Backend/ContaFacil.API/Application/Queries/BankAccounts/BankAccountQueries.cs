using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.BankAccounts;

public record ObterTodasContasBancariasQuery : IRequest<List<ContaBancariaDto>>;

public record ObterContaBancariaPorIdQuery(Guid Id) : IRequest<ContaBancariaDto?>;

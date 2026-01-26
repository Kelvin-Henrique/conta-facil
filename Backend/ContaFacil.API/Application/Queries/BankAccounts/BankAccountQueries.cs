using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.BankAccounts;

public record GetAllBankAccountsQuery : IRequest<List<BankAccountDto>>;

public record GetBankAccountByIdQuery(Guid Id) : IRequest<BankAccountDto?>;

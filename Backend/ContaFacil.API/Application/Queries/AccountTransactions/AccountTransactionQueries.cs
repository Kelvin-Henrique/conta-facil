using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.AccountTransactions;

public record GetAllAccountTransactionsQuery : IRequest<List<AccountTransactionDto>>;

public record GetAccountTransactionByIdQuery(Guid Id) : IRequest<AccountTransactionDto?>;

public record GetAccountTransactionsByBankAccountQuery(Guid BankAccountId) : IRequest<List<AccountTransactionDto>>;

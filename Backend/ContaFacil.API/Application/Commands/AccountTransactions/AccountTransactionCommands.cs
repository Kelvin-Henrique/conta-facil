using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.AccountTransactions;

public record CreateAccountTransactionCommand(
    Guid BankAccountId,
    string Description,
    string Category,
    DateTime Date,
    decimal Amount
) : IRequest<AccountTransactionDto>;

public record UpdateAccountTransactionCommand(
    Guid Id,
    string Description,
    string Category,
    DateTime Date,
    decimal Amount
) : IRequest<AccountTransactionDto>;

public record DeleteAccountTransactionCommand(Guid Id) : IRequest<bool>;

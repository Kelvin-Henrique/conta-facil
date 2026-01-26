using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.BankAccounts;

public record CreateBankAccountCommand(
    string Name,
    string BankName,
    decimal Balance
) : IRequest<BankAccountDto>;

public record UpdateBankAccountCommand(
    Guid Id,
    string Name,
    string BankName,
    decimal Balance
) : IRequest<BankAccountDto>;

public record DeleteBankAccountCommand(Guid Id) : IRequest<bool>;

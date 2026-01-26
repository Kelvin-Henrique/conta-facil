using AutoMapper;
using ContaFacil.API.Application.Commands.AccountTransactions;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.AccountTransactions;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.AccountTransactions;

public class CreateAccountTransactionCommandHandler : IRequestHandler<CreateAccountTransactionCommand, AccountTransactionDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateAccountTransactionCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountTransactionDto> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new AccountTransaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = request.BankAccountId,
            Description = request.Description,
            Category = request.Category,
            Date = request.Date,
            Amount = request.Amount
        };

        _context.AccountTransactions.Add(transaction);
        
        // Atualizar saldo da conta
        var account = await _context.BankAccounts.FindAsync(new object[] { request.BankAccountId }, cancellationToken);
        if (account != null)
        {
            account.Balance -= request.Amount; // Subtrai pois é uma despesa
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .FirstAsync(t => t.Id == transaction.Id, cancellationToken);

        return _mapper.Map<AccountTransactionDto>(result);
    }
}

public class UpdateAccountTransactionCommandHandler : IRequestHandler<UpdateAccountTransactionCommand, AccountTransactionDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAccountTransactionCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountTransactionDto> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (transaction == null)
            throw new KeyNotFoundException($"Transação com ID {request.Id} não encontrada.");

        // Reverter valor anterior no saldo
        transaction.BankAccount.Balance += transaction.Amount;

        transaction.Description = request.Description;
        transaction.Category = request.Category;
        transaction.Date = request.Date;
        transaction.Amount = request.Amount;

        // Aplicar novo valor no saldo
        transaction.BankAccount.Balance -= request.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountTransactionDto>(transaction);
    }
}

public class DeleteAccountTransactionCommandHandler : IRequestHandler<DeleteAccountTransactionCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteAccountTransactionCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (transaction == null)
            return false;

        // Reverter valor no saldo antes de deletar
        transaction.BankAccount.Balance += transaction.Amount;

        _context.AccountTransactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class GetAllAccountTransactionsQueryHandler : IRequestHandler<GetAllAccountTransactionsQuery, List<AccountTransactionDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAccountTransactionsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountTransactionDto>> Handle(GetAllAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<AccountTransactionDto>>(transactions);
    }
}

public class GetAccountTransactionByIdQueryHandler : IRequestHandler<GetAccountTransactionByIdQuery, AccountTransactionDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountTransactionByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccountTransactionDto?> Handle(GetAccountTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        return transaction == null ? null : _mapper.Map<AccountTransactionDto>(transaction);
    }
}

public class GetAccountTransactionsByBankAccountQueryHandler : IRequestHandler<GetAccountTransactionsByBankAccountQuery, List<AccountTransactionDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountTransactionsByBankAccountQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AccountTransactionDto>> Handle(GetAccountTransactionsByBankAccountQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _context.AccountTransactions
            .Include(t => t.BankAccount)
            .Where(t => t.BankAccountId == request.BankAccountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<AccountTransactionDto>>(transactions);
    }
}

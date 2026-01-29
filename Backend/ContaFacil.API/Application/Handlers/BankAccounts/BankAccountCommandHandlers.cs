using AutoMapper;
using ContaFacil.API.Application.Commands.BankAccounts;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.BankAccounts;

public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateBankAccountCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BankAccountDto> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var account = new BankAccount
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            BankName = request.BankName,
            Balance = request.Balance,
            UserId = user.Id
        };

        _context.BankAccounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BankAccountDto>(account);
    }
}

public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, BankAccountDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBankAccountCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BankAccountDto> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (account == null)
            throw new KeyNotFoundException($"Conta bancária com ID {request.Id} não encontrada.");

        account.Name = request.Name;
        account.BankName = request.BankName;
        account.Balance = request.Balance;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BankAccountDto>(account);
    }
}

public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteBankAccountCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (account == null)
            return false;

        _context.BankAccounts.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class GetAllBankAccountsQueryHandler : IRequestHandler<Queries.BankAccounts.GetAllBankAccountsQuery, List<BankAccountDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBankAccountsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BankAccountDto>> Handle(Queries.BankAccounts.GetAllBankAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _context.BankAccounts
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<BankAccountDto>>(accounts);
    }
}

public class GetBankAccountByIdQueryHandler : IRequestHandler<Queries.BankAccounts.GetBankAccountByIdQuery, BankAccountDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBankAccountByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BankAccountDto?> Handle(Queries.BankAccounts.GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts.FindAsync(new object[] { request.Id }, cancellationToken);

        return account == null ? null : _mapper.Map<BankAccountDto>(account);
    }
}

using AutoMapper;
using ContaFacil.API.Application.Commands.BankAccounts;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.BankAccounts;

public class CriarContaBancariaCommandHandler : IRequestHandler<CriarContaBancariaCommand, ContaBancariaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CriarContaBancariaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaBancariaDto> Handle(CriarContaBancariaCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var account = new ContaBancaria
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            NomeBanco = request.NomeBanco,
            Saldo = request.Saldo,
            UsuarioId = user.Id
        };

        _context.ContasBancarias.Add(account);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContaBancariaDto>(account);
    }
}

public class AtualizarContaBancariaCommandHandler : IRequestHandler<AtualizarContaBancariaCommand, ContaBancariaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AtualizarContaBancariaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaBancariaDto> Handle(AtualizarContaBancariaCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.ContasBancarias.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (account == null)
            throw new KeyNotFoundException($"Conta bancária com ID {request.Id} não encontrada.");

        account.Nome = request.Nome;
        account.NomeBanco = request.NomeBanco;
        account.Saldo = request.Saldo;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContaBancariaDto>(account);
    }
}

public class ExcluirContaBancariaCommandHandler : IRequestHandler<ExcluirContaBancariaCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ExcluirContaBancariaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ExcluirContaBancariaCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.ContasBancarias.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (account == null)
            return false;

        _context.ContasBancarias.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class ObterTodasContasBancariasQueryHandler : IRequestHandler<Queries.BankAccounts.ObterTodasContasBancariasQuery, List<ContaBancariaDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTodasContasBancariasQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ContaBancariaDto>> Handle(Queries.BankAccounts.ObterTodasContasBancariasQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _context.ContasBancarias
            .OrderBy(a => a.Nome)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ContaBancariaDto>>(accounts);
    }
}

public class ObterContaBancariaPorIdQueryHandler : IRequestHandler<Queries.BankAccounts.ObterContaBancariaPorIdQuery, ContaBancariaDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterContaBancariaPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaBancariaDto?> Handle(Queries.BankAccounts.ObterContaBancariaPorIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.ContasBancarias.FindAsync(new object[] { request.Id }, cancellationToken);

        return account == null ? null : _mapper.Map<ContaBancariaDto>(account);
    }
}

using AutoMapper;
using ContaFacil.API.Application.Commands.AccountTransactions;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.AccountTransactions;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.AccountTransactions;

public class CriarTransacaoContaCommandHandler : IRequestHandler<CriarTransacaoContaCommand, TransacaoContaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CriarTransacaoContaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TransacaoContaDto> Handle(CriarTransacaoContaCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var transaction = new TransacaoConta
        {
            Id = Guid.NewGuid(),
            ContaBancariaId = request.ContaBancariaId,
            Descricao = request.Descricao,
            Categoria = request.Categoria,
            Data = DateTime.SpecifyKind(request.Data, DateTimeKind.Utc),
            Valor = request.Valor,
            UsuarioId = user.Id
        };

        _context.TransacoesConta.Add(transaction);
        
        // Atualizar saldo da conta
        var account = await _context.ContasBancarias.FindAsync(new object[] { request.ContaBancariaId }, cancellationToken);
        if (account != null)
        {
            account.Saldo -= request.Valor; // Subtrai pois é uma despesa
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .FirstAsync(t => t.Id == transaction.Id, cancellationToken);

        return _mapper.Map<TransacaoContaDto>(result);
    }
}

public class AtualizarTransacaoContaCommandHandler : IRequestHandler<AtualizarTransacaoContaCommand, TransacaoContaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AtualizarTransacaoContaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TransacaoContaDto> Handle(AtualizarTransacaoContaCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (transaction == null)
            throw new KeyNotFoundException($"Transação com ID {request.Id} não encontrada.");

        // Reverter valor anterior no saldo
        transaction.ContaBancaria.Saldo += transaction.Valor;

        transaction.Descricao = request.Descricao;
        transaction.Categoria = request.Categoria;
        transaction.Data = request.Data;
        transaction.Valor = request.Valor;

        // Aplicar novo valor no saldo
        transaction.ContaBancaria.Saldo -= request.Valor;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransacaoContaDto>(transaction);
    }
}

public class ExcluirTransacaoContaCommandHandler : IRequestHandler<ExcluirTransacaoContaCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ExcluirTransacaoContaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ExcluirTransacaoContaCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        
        if (transaction == null)
            return false;

        // Reverter valor no saldo antes de deletar
        transaction.ContaBancaria.Saldo += transaction.Valor;

        _context.TransacoesConta.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class ObterTodasTransacoesContaQueryHandler : IRequestHandler<ObterTodasTransacoesContaQuery, List<TransacaoContaDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTodasTransacoesContaQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TransacaoContaDto>> Handle(ObterTodasTransacoesContaQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .OrderByDescending(t => t.Data)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<TransacaoContaDto>>(transactions);
    }
}

public class ObterTransacaoContaPorIdQueryHandler : IRequestHandler<ObterTransacaoContaPorIdQuery, TransacaoContaDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTransacaoContaPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TransacaoContaDto?> Handle(ObterTransacaoContaPorIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        return transaction == null ? null : _mapper.Map<TransacaoContaDto>(transaction);
    }
}

public class ObterTransacoesContaPorContaBancariaQueryHandler : IRequestHandler<ObterTransacoesContaPorContaBancariaQuery, List<TransacaoContaDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTransacoesContaPorContaBancariaQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TransacaoContaDto>> Handle(ObterTransacoesContaPorContaBancariaQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _context.TransacoesConta
            .Include(t => t.ContaBancaria)
            .Where(t => t.ContaBancariaId == request.ContaBancariaId)
            .OrderByDescending(t => t.Data)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<TransacaoContaDto>>(transactions);
    }
}

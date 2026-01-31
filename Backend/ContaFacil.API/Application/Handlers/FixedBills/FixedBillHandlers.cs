using AutoMapper;
using ContaFacil.API.Application.Commands.FixedBills;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.FixedBills;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.FixedBills;

public class CriarContaFixaCommandHandler : IRequestHandler<CriarContaFixaCommand, ContaFixaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CriarContaFixaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaFixaDto> Handle(CriarContaFixaCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var bill = new ContaFixa
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Categoria = request.Categoria,
            Valor = request.Valor,
            DiaVencimento = request.DiaVencimento,
            Mes = request.Mes,
            Ano = request.Ano,
            Pago = request.Pago,
            Recorrente = request.Recorrente,
            UsuarioId = user.Id
        };

        _context.ContasFixas.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContaFixaDto>(bill);
    }
}

public class AtualizarContaFixaCommandHandler : IRequestHandler<AtualizarContaFixaCommand, ContaFixaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AtualizarContaFixaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaFixaDto> Handle(AtualizarContaFixaCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.ContasFixas.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            throw new KeyNotFoundException($"Conta fixa com ID {request.Id} não encontrada.");

        bill.Nome = request.Nome;
        bill.Categoria = request.Categoria;
        bill.Valor = request.Valor;
        bill.DiaVencimento = request.DiaVencimento;
        bill.Pago = request.Pago;
        bill.Recorrente = request.Recorrente;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContaFixaDto>(bill);
    }
}

public class ExcluirContaFixaCommandHandler : IRequestHandler<ExcluirContaFixaCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ExcluirContaFixaCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ExcluirContaFixaCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.ContasFixas.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            return false;

        _context.ContasFixas.Remove(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class AlternarContaFixaPagaCommandHandler : IRequestHandler<AlternarContaFixaPagaCommand, ContaFixaDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AlternarContaFixaPagaCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaFixaDto> Handle(AlternarContaFixaPagaCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.ContasFixas.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            throw new KeyNotFoundException($"Conta fixa com ID {request.Id} não encontrada.");

        bill.Pago = !bill.Pago;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContaFixaDto>(bill);
    }
}

public class ObterTodasContasFixasQueryHandler : IRequestHandler<ObterTodasContasFixasQuery, List<ContaFixaDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTodasContasFixasQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ContaFixaDto>> Handle(ObterTodasContasFixasQuery request, CancellationToken cancellationToken)
    {
        var bills = await _context.ContasFixas
            .OrderBy(b => b.Ano)
            .ThenBy(b => b.Mes)
            .ThenBy(b => b.DiaVencimento)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ContaFixaDto>>(bills);
    }
}

public class ObterContaFixaPorIdQueryHandler : IRequestHandler<ObterContaFixaPorIdQuery, ContaFixaDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterContaFixaPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContaFixaDto?> Handle(ObterContaFixaPorIdQuery request, CancellationToken cancellationToken)
    {
        var bill = await _context.ContasFixas.FindAsync(new object[] { request.Id }, cancellationToken);

        return bill == null ? null : _mapper.Map<ContaFixaDto>(bill);
    }
}

public class ObterContasFixasPorMesAnoQueryHandler : IRequestHandler<ObterContasFixasPorMesAnoQuery, List<ContaFixaDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterContasFixasPorMesAnoQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ContaFixaDto>> Handle(ObterContasFixasPorMesAnoQuery request, CancellationToken cancellationToken)
    {
        var bills = await _context.ContasFixas
            .Where(b => b.Mes == request.Mes && b.Ano == request.Ano)
            .OrderBy(b => b.DiaVencimento)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ContaFixaDto>>(bills);
    }
}

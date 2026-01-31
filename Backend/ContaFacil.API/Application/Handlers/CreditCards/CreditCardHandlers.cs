using AutoMapper;
using ContaFacil.API.Application.Commands.CreditCards;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.CreditCards;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.CreditCards;

public class CriarCartaoCreditoCommandHandler : IRequestHandler<CriarCartaoCreditoCommand, CartaoCreditoDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CriarCartaoCreditoCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartaoCreditoDto> Handle(CriarCartaoCreditoCommand request, CancellationToken cancellationToken)
    {
        // TODO: Pegar UserId do contexto de autenticação
        // Por enquanto, usar o primeiro usuário do banco
        var user = await _context.Usuarios.FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            throw new Exception("Nenhum usuário encontrado no sistema");
        }

        var card = new CartaoCredito
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            DiaVencimento = request.DiaVencimento,
            DiaFechamento = request.DiaFechamento,
            UsuarioId = user.Id
        };

        _context.CartoesCredito.Add(card);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CartaoCreditoDto>(card);
    }
}

public class AtualizarCartaoCreditoCommandHandler : IRequestHandler<AtualizarCartaoCreditoCommand, CartaoCreditoDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AtualizarCartaoCreditoCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartaoCreditoDto> Handle(AtualizarCartaoCreditoCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.CartoesCredito.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (card == null)
            throw new KeyNotFoundException($"Cartão de crédito com ID {request.Id} não encontrado.");

        card.Nome = request.Nome;
        card.DiaVencimento = request.DiaVencimento;
        card.DiaFechamento = request.DiaFechamento;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CartaoCreditoDto>(card);
    }
}

public class ExcluirCartaoCreditoCommandHandler : IRequestHandler<ExcluirCartaoCreditoCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ExcluirCartaoCreditoCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ExcluirCartaoCreditoCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.CartoesCredito.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (card == null)
            return false;

        _context.CartoesCredito.Remove(card);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class ObterTodosCartoesCreditoQueryHandler : IRequestHandler<ObterTodosCartoesCreditoQuery, List<CartaoCreditoDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTodosCartoesCreditoQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CartaoCreditoDto>> Handle(ObterTodosCartoesCreditoQuery request, CancellationToken cancellationToken)
    {
        var cards = await _context.CartoesCredito
            .OrderBy(c => c.Nome)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CartaoCreditoDto>>(cards);
    }
}

public class ObterCartaoCreditoPorIdQueryHandler : IRequestHandler<ObterCartaoCreditoPorIdQuery, CartaoCreditoDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterCartaoCreditoPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartaoCreditoDto?> Handle(ObterCartaoCreditoPorIdQuery request, CancellationToken cancellationToken)
    {
        var card = await _context.CartoesCredito.FindAsync(new object[] { request.Id }, cancellationToken);

        return card == null ? null : _mapper.Map<CartaoCreditoDto>(card);
    }
}

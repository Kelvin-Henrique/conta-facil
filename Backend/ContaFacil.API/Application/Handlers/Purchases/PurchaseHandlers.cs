using AutoMapper;
using ContaFacil.API.Application.Commands.Purchases;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Purchases;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.Purchases;

public class CriarCompraCommandHandler : IRequestHandler<CriarCompraCommand, CompraDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CriarCompraCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompraDto> Handle(CriarCompraCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var purchase = new Compra
        {
            Id = Guid.NewGuid(),
            CartaoCreditoId = request.CartaoCreditoId,
            Descricao = request.Descricao,
            Categoria = request.Categoria,
            Data = DateTime.SpecifyKind(request.Data, DateTimeKind.Utc),
            ValorTotal = request.ValorTotal,
            Parcelas = request.Parcelas,
            UsuarioId = user.Id
        };

        _context.Compras.Add(purchase);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.Compras
            .Include(p => p.CartaoCredito)
            .FirstAsync(p => p.Id == purchase.Id, cancellationToken);

        return _mapper.Map<CompraDto>(result);
    }
}

public class AtualizarCompraCommandHandler : IRequestHandler<AtualizarCompraCommand, CompraDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AtualizarCompraCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompraDto> Handle(AtualizarCompraCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Compras
            .Include(p => p.CartaoCredito)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (purchase == null)
            throw new KeyNotFoundException($"Compra com ID {request.Id} não encontrada.");

        purchase.Descricao = request.Descricao;
        purchase.Categoria = request.Categoria;
        purchase.Data = request.Data;
        purchase.ValorTotal = request.ValorTotal;
        purchase.Parcelas = request.Parcelas;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CompraDto>(purchase);
    }
}

public class ExcluirCompraCommandHandler : IRequestHandler<ExcluirCompraCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ExcluirCompraCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ExcluirCompraCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Compras.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (purchase == null)
            return false;

        _context.Compras.Remove(purchase);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class ObterTodasComprasQueryHandler : IRequestHandler<ObterTodasComprasQuery, List<CompraDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterTodasComprasQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompraDto>> Handle(ObterTodasComprasQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _context.Compras
            .Include(p => p.CartaoCredito)
            .OrderByDescending(p => p.Data)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CompraDto>>(purchases);
    }
}

public class ObterCompraPorIdQueryHandler : IRequestHandler<ObterCompraPorIdQuery, CompraDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterCompraPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompraDto?> Handle(ObterCompraPorIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Compras
            .Include(p => p.CartaoCredito)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        return purchase == null ? null : _mapper.Map<CompraDto>(purchase);
    }
}

public class ObterComprasPorCartaoCreditoQueryHandler : IRequestHandler<ObterComprasPorCartaoCreditoQuery, List<CompraDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ObterComprasPorCartaoCreditoQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompraDto>> Handle(ObterComprasPorCartaoCreditoQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _context.Compras
            .Include(p => p.CartaoCredito)
            .Where(p => p.CartaoCreditoId == request.CartaoCreditoId)
            .OrderByDescending(p => p.Data)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CompraDto>>(purchases);
    }
}

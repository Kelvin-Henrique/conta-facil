using AutoMapper;
using ContaFacil.API.Application.Commands.Purchases;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Purchases;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.Purchases;

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, PurchaseDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePurchaseCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurchaseDto> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = new Purchase
        {
            Id = Guid.NewGuid(),
            CreditCardId = request.CreditCardId,
            Description = request.Description,
            Category = request.Category,
            Date = request.Date,
            TotalAmount = request.TotalAmount,
            Installments = request.Installments
        };

        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.Purchases
            .Include(p => p.CreditCard)
            .FirstAsync(p => p.Id == purchase.Id, cancellationToken);

        return _mapper.Map<PurchaseDto>(result);
    }
}

public class UpdatePurchaseCommandHandler : IRequestHandler<UpdatePurchaseCommand, PurchaseDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePurchaseCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurchaseDto> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Purchases
            .Include(p => p.CreditCard)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (purchase == null)
            throw new KeyNotFoundException($"Compra com ID {request.Id} não encontrada.");

        purchase.Description = request.Description;
        purchase.Category = request.Category;
        purchase.Date = request.Date;
        purchase.TotalAmount = request.TotalAmount;
        purchase.Installments = request.Installments;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PurchaseDto>(purchase);
    }
}

public class DeletePurchaseCommandHandler : IRequestHandler<DeletePurchaseCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeletePurchaseCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Purchases.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (purchase == null)
            return false;

        _context.Purchases.Remove(purchase);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class GetAllPurchasesQueryHandler : IRequestHandler<GetAllPurchasesQuery, List<PurchaseDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPurchasesQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PurchaseDto>> Handle(GetAllPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _context.Purchases
            .Include(p => p.CreditCard)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<PurchaseDto>>(purchases);
    }
}

public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, PurchaseDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurchaseByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PurchaseDto?> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _context.Purchases
            .Include(p => p.CreditCard)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        return purchase == null ? null : _mapper.Map<PurchaseDto>(purchase);
    }
}

public class GetPurchasesByCreditCardQueryHandler : IRequestHandler<GetPurchasesByCreditCardQuery, List<PurchaseDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurchasesByCreditCardQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PurchaseDto>> Handle(GetPurchasesByCreditCardQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _context.Purchases
            .Include(p => p.CreditCard)
            .Where(p => p.CreditCardId == request.CreditCardId)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<PurchaseDto>>(purchases);
    }
}

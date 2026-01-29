using AutoMapper;
using ContaFacil.API.Application.Commands.CreditCards;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.CreditCards;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.CreditCards;

public class CreateCreditCardCommandHandler : IRequestHandler<CreateCreditCardCommand, CreditCardDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCreditCardCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreditCardDto> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        // TODO: Pegar UserId do contexto de autenticação
        // Por enquanto, usar o primeiro usuário do banco
        var user = await _context.Users.FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            throw new Exception("Nenhum usuário encontrado no sistema");
        }

        var card = new CreditCard
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DueDay = request.DueDay,
            ClosingDay = request.ClosingDay,
            UserId = user.Id
        };

        _context.CreditCards.Add(card);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreditCardDto>(card);
    }
}

public class UpdateCreditCardCommandHandler : IRequestHandler<UpdateCreditCardCommand, CreditCardDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCreditCardCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreditCardDto> Handle(UpdateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.CreditCards.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (card == null)
            throw new KeyNotFoundException($"Cartão de crédito com ID {request.Id} não encontrado.");

        card.Name = request.Name;
        card.DueDay = request.DueDay;
        card.ClosingDay = request.ClosingDay;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreditCardDto>(card);
    }
}

public class DeleteCreditCardCommandHandler : IRequestHandler<DeleteCreditCardCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteCreditCardCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.CreditCards.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (card == null)
            return false;

        _context.CreditCards.Remove(card);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class GetAllCreditCardsQueryHandler : IRequestHandler<GetAllCreditCardsQuery, List<CreditCardDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCreditCardsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CreditCardDto>> Handle(GetAllCreditCardsQuery request, CancellationToken cancellationToken)
    {
        var cards = await _context.CreditCards
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CreditCardDto>>(cards);
    }
}

public class GetCreditCardByIdQueryHandler : IRequestHandler<GetCreditCardByIdQuery, CreditCardDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCreditCardByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreditCardDto?> Handle(GetCreditCardByIdQuery request, CancellationToken cancellationToken)
    {
        var card = await _context.CreditCards.FindAsync(new object[] { request.Id }, cancellationToken);

        return card == null ? null : _mapper.Map<CreditCardDto>(card);
    }
}

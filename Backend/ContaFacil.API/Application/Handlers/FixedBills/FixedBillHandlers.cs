using AutoMapper;
using ContaFacil.API.Application.Commands.FixedBills;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.FixedBills;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.FixedBills;

public class CreateFixedBillCommandHandler : IRequestHandler<CreateFixedBillCommand, FixedBillDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateFixedBillCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FixedBillDto> Handle(CreateFixedBillCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new Exception("Nenhum usuário encontrado");

        var bill = new FixedBill
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Category = request.Category,
            Amount = request.Amount,
            DueDay = request.DueDay,
            Month = request.Month,
            Year = request.Year,
            IsPaid = request.IsPaid,
            IsRecurring = request.IsRecurring,
            UserId = user.Id
        };

        _context.FixedBills.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FixedBillDto>(bill);
    }
}

public class UpdateFixedBillCommandHandler : IRequestHandler<UpdateFixedBillCommand, FixedBillDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateFixedBillCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FixedBillDto> Handle(UpdateFixedBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.FixedBills.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            throw new KeyNotFoundException($"Conta fixa com ID {request.Id} não encontrada.");

        bill.Name = request.Name;
        bill.Category = request.Category;
        bill.Amount = request.Amount;
        bill.DueDay = request.DueDay;
        bill.IsPaid = request.IsPaid;
        bill.IsRecurring = request.IsRecurring;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FixedBillDto>(bill);
    }
}

public class DeleteFixedBillCommandHandler : IRequestHandler<DeleteFixedBillCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteFixedBillCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteFixedBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.FixedBills.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            return false;

        _context.FixedBills.Remove(bill);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

public class ToggleFixedBillPaidCommandHandler : IRequestHandler<ToggleFixedBillPaidCommand, FixedBillDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ToggleFixedBillPaidCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FixedBillDto> Handle(ToggleFixedBillPaidCommand request, CancellationToken cancellationToken)
    {
        var bill = await _context.FixedBills.FindAsync(new object[] { request.Id }, cancellationToken);
        
        if (bill == null)
            throw new KeyNotFoundException($"Conta fixa com ID {request.Id} não encontrada.");

        bill.IsPaid = !bill.IsPaid;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FixedBillDto>(bill);
    }
}

public class GetAllFixedBillsQueryHandler : IRequestHandler<GetAllFixedBillsQuery, List<FixedBillDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFixedBillsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FixedBillDto>> Handle(GetAllFixedBillsQuery request, CancellationToken cancellationToken)
    {
        var bills = await _context.FixedBills
            .OrderBy(b => b.Year)
            .ThenBy(b => b.Month)
            .ThenBy(b => b.DueDay)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FixedBillDto>>(bills);
    }
}

public class GetFixedBillByIdQueryHandler : IRequestHandler<GetFixedBillByIdQuery, FixedBillDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFixedBillByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FixedBillDto?> Handle(GetFixedBillByIdQuery request, CancellationToken cancellationToken)
    {
        var bill = await _context.FixedBills.FindAsync(new object[] { request.Id }, cancellationToken);

        return bill == null ? null : _mapper.Map<FixedBillDto>(bill);
    }
}

public class GetFixedBillsByMonthYearQueryHandler : IRequestHandler<GetFixedBillsByMonthYearQuery, List<FixedBillDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFixedBillsByMonthYearQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FixedBillDto>> Handle(GetFixedBillsByMonthYearQuery request, CancellationToken cancellationToken)
    {
        var bills = await _context.FixedBills
            .Where(b => b.Month == request.Month && b.Year == request.Year)
            .OrderBy(b => b.DueDay)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FixedBillDto>>(bills);
    }
}

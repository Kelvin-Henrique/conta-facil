using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.FixedBills;

public record CreateFixedBillCommand(
    string Name,
    string Category,
    decimal Amount,
    int DueDay,
    int Month,
    int Year,
    bool IsPaid,
    bool IsRecurring
) : IRequest<FixedBillDto>;

public record UpdateFixedBillCommand(
    Guid Id,
    string Name,
    string Category,
    decimal Amount,
    int DueDay,
    bool IsPaid,
    bool IsRecurring
) : IRequest<FixedBillDto>;

public record DeleteFixedBillCommand(Guid Id) : IRequest<bool>;

public record ToggleFixedBillPaidCommand(Guid Id) : IRequest<FixedBillDto>;

using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.FixedBills;

public record GetAllFixedBillsQuery : IRequest<List<FixedBillDto>>;

public record GetFixedBillByIdQuery(Guid Id) : IRequest<FixedBillDto?>;

public record GetFixedBillsByMonthYearQuery(int Month, int Year) : IRequest<List<FixedBillDto>>;

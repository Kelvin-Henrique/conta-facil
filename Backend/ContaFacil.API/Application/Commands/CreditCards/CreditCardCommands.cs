using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.CreditCards;

public record CriarCartaoCreditoCommand(
    string Nome,
    int DiaVencimento,
    int DiaFechamento
) : IRequest<CartaoCreditoDto>;

public record AtualizarCartaoCreditoCommand(
    Guid Id,
    string Nome,
    int DiaVencimento,
    int DiaFechamento
) : IRequest<CartaoCreditoDto>;

public record ExcluirCartaoCreditoCommand(Guid Id) : IRequest<bool>;

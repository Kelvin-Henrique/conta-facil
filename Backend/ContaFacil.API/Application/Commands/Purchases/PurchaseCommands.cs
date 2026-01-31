using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.Purchases;

public record CriarCompraCommand(
    Guid CartaoCreditoId,
    string Descricao,
    string Categoria,
    DateTime Data,
    decimal ValorTotal,
    int Parcelas
) : IRequest<CompraDto>;

public record AtualizarCompraCommand(
    Guid Id,
    string Descricao,
    string Categoria,
    DateTime Data,
    decimal ValorTotal,
    int Parcelas
) : IRequest<CompraDto>;

public record ExcluirCompraCommand(Guid Id) : IRequest<bool>;

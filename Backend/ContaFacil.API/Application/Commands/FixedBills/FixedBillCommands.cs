using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.FixedBills;

public record CriarContaFixaCommand(
    string Nome,
    string Categoria,
    decimal Valor,
    int DiaVencimento,
    int Mes,
    int Ano,
    bool Pago,
    bool Recorrente
) : IRequest<ContaFixaDto>;

public record AtualizarContaFixaCommand(
    Guid Id,
    string Nome,
    string Categoria,
    decimal Valor,
    int DiaVencimento,
    bool Pago,
    bool Recorrente
) : IRequest<ContaFixaDto>;

public record ExcluirContaFixaCommand(Guid Id) : IRequest<bool>;

public record AlternarContaFixaPagaCommand(Guid Id) : IRequest<ContaFixaDto>;

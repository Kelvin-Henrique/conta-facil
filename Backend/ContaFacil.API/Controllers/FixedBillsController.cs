using ContaFacil.API.Application.Commands.FixedBills;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.FixedBills;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasFixasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContasFixasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ContaFixaDto>>> ObterTodas()
    {
        var resultado = await _mediator.Send(new ObterTodasContasFixasQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaFixaDto>> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterContaFixaPorIdQuery(id));
        
        if (resultado == null)
            return NotFound(new { message = "Conta fixa não encontrada" });

        return Ok(resultado);
    }

    [HttpGet("by-month")]
    public async Task<ActionResult<List<ContaFixaDto>>> ObterPorMesAno([FromQuery] int month, [FromQuery] int year)
    {
        var resultado = await _mediator.Send(new ObterContasFixasPorMesAnoQuery(month, year));
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<ContaFixaDto>> Criar([FromBody] CriarContaFixaDto dto)
    {
        var comando = new CriarContaFixaCommand(
            dto.Nome,
            dto.Categoria,
            dto.Valor,
            dto.DiaVencimento,
            dto.Mes,
            dto.Ano,
            dto.Pago,
            dto.Recorrente
        );
        var resultado = await _mediator.Send(comando);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContaFixaDto>> Atualizar(Guid id, [FromBody] AtualizarContaFixaDto dto)
    {
        try
        {
            var comando = new AtualizarContaFixaCommand(
                id,
                dto.Nome,
                dto.Categoria,
                dto.Valor,
                dto.DiaVencimento,
                dto.Pago,
                dto.Recorrente
            );
            var resultado = await _mediator.Send(comando);
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/toggle-paid")]
    public async Task<ActionResult<ContaFixaDto>> AlternarPago(Guid id)
    {
        try
        {
            var resultado = await _mediator.Send(new AlternarContaFixaPagaCommand(id));
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(Guid id)
    {
        var resultado = await _mediator.Send(new ExcluirContaFixaCommand(id));
        
        if (!resultado)
            return NotFound(new { message = "Conta fixa não encontrada" });

        return NoContent();
    }
}

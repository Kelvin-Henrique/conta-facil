using ContaFacil.API.Application.Commands.Purchases;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Purchases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComprasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CompraDto>>> ObterTodas()
    {
        var resultado = await _mediator.Send(new ObterTodasComprasQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompraDto>> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterCompraPorIdQuery(id));
        
        if (resultado == null)
            return NotFound(new { message = "Compra não encontrada" });

        return Ok(resultado);
    }

    [HttpGet("by-card/{creditCardId}")]
    public async Task<ActionResult<List<CompraDto>>> ObterPorCartaoCredito(Guid creditCardId)
    {
        var resultado = await _mediator.Send(new ObterComprasPorCartaoCreditoQuery(creditCardId));
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<CompraDto>> Criar([FromBody] CriarCompraDto dto)
    {
        var comando = new CriarCompraCommand(
            dto.CartaoCreditoId,
            dto.Descricao,
            dto.Categoria,
            dto.Data,
            dto.ValorTotal,
            dto.Parcelas
        );
        var resultado = await _mediator.Send(comando);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CompraDto>> Atualizar(Guid id, [FromBody] AtualizarCompraDto dto)
    {
        try
        {
            var comando = new AtualizarCompraCommand(
                id,
                dto.Descricao,
                dto.Categoria,
                dto.Data,
                dto.ValorTotal,
                dto.Parcelas
            );
            var resultado = await _mediator.Send(comando);
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
        var resultado = await _mediator.Send(new ExcluirCompraCommand(id));
        
        if (!resultado)
            return NotFound(new { message = "Compra não encontrada" });

        return NoContent();
    }
}

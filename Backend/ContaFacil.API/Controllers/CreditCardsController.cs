using ContaFacil.API.Application.Commands.CreditCards;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.CreditCards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartoesCreditoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartoesCreditoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CartaoCreditoDto>>> ObterTodos()
    {
        var resultado = await _mediator.Send(new ObterTodosCartoesCreditoQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartaoCreditoDto>> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterCartaoCreditoPorIdQuery(id));
        
        if (resultado == null)
            return NotFound(new { message = "Cartão de crédito não encontrado" });

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<CartaoCreditoDto>> Criar([FromBody] CriarCartaoCreditoDto dto)
    {
        var comando = new CriarCartaoCreditoCommand(dto.Nome, dto.DiaVencimento, dto.DiaFechamento);
        var resultado = await _mediator.Send(comando);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartaoCreditoDto>> Atualizar(Guid id, [FromBody] AtualizarCartaoCreditoDto dto)
    {
        try
        {
            var comando = new AtualizarCartaoCreditoCommand(id, dto.Nome, dto.DiaVencimento, dto.DiaFechamento);
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
        var resultado = await _mediator.Send(new ExcluirCartaoCreditoCommand(id));
        
        if (!resultado)
            return NotFound(new { message = "Cartão de crédito não encontrado" });

        return NoContent();
    }
}

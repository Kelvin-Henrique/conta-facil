using ContaFacil.API.Application.Commands.AccountTransactions;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.AccountTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesContaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransacoesContaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransacaoContaDto>>> ObterTodas()
    {
        var resultado = await _mediator.Send(new ObterTodasTransacoesContaQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransacaoContaDto>> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterTransacaoContaPorIdQuery(id));
        
        if (resultado == null)
            return NotFound(new { message = "Transação não encontrada" });

        return Ok(resultado);
    }

    [HttpGet("by-account/{bankAccountId}")]
    public async Task<ActionResult<List<TransacaoContaDto>>> ObterPorContaBancaria(Guid bankAccountId)
    {
        var resultado = await _mediator.Send(new ObterTransacoesContaPorContaBancariaQuery(bankAccountId));
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<TransacaoContaDto>> Criar([FromBody] CriarTransacaoContaDto dto)
    {
        var comando = new CriarTransacaoContaCommand(
            dto.ContaBancariaId,
            dto.Descricao,
            dto.Categoria,
            dto.Data,
            dto.Valor
        );
        var resultado = await _mediator.Send(comando);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TransacaoContaDto>> Atualizar(Guid id, [FromBody] AtualizarTransacaoContaDto dto)
    {
        try
        {
            var comando = new AtualizarTransacaoContaCommand(
                id,
                dto.Descricao,
                dto.Categoria,
                dto.Data,
                dto.Valor
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
        var resultado = await _mediator.Send(new ExcluirTransacaoContaCommand(id));
        
        if (!resultado)
            return NotFound(new { message = "Transação não encontrada" });

        return NoContent();
    }
}

using ContaFacil.API.Application.Commands.BankAccounts;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.BankAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasBancariasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContasBancariasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ContaBancariaDto>>> ObterTodas()
    {
        var resultado = await _mediator.Send(new ObterTodasContasBancariasQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaBancariaDto>> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterContaBancariaPorIdQuery(id));
        
        if (resultado == null)
            return NotFound(new { message = "Conta bancária não encontrada" });

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<ContaBancariaDto>> Criar([FromBody] CriarContaBancariaDto dto)
    {
        var comando = new CriarContaBancariaCommand(dto.Nome, dto.NomeBanco, dto.Saldo);
        var resultado = await _mediator.Send(comando);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContaBancariaDto>> Atualizar(Guid id, [FromBody] AtualizarContaBancariaDto dto)
    {
        try
        {
            var comando = new AtualizarContaBancariaCommand(id, dto.Nome, dto.NomeBanco, dto.Saldo);
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
        var resultado = await _mediator.Send(new ExcluirContaBancariaCommand(id));
        
        if (!resultado)
            return NotFound(new { message = "Conta bancária não encontrada" });

        return NoContent();
    }
}

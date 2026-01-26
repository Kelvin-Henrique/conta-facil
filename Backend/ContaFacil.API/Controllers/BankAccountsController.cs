using ContaFacil.API.Application.Commands.BankAccounts;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.BankAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BankAccountDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllBankAccountsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccountDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetBankAccountByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = "Conta bancária não encontrada" });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BankAccountDto>> Create([FromBody] CreateBankAccountDto dto)
    {
        var command = new CreateBankAccountCommand(dto.Name, dto.BankName, dto.Balance);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BankAccountDto>> Update(Guid id, [FromBody] UpdateBankAccountDto dto)
    {
        try
        {
            var command = new UpdateBankAccountCommand(id, dto.Name, dto.BankName, dto.Balance);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBankAccountCommand(id));
        
        if (!result)
            return NotFound(new { message = "Conta bancária não encontrada" });

        return NoContent();
    }
}

using ContaFacil.API.Application.Commands.AccountTransactions;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.AccountTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountTransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountTransactionDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAccountTransactionsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountTransactionDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAccountTransactionByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = "Transação não encontrada" });

        return Ok(result);
    }

    [HttpGet("by-account/{bankAccountId}")]
    public async Task<ActionResult<List<AccountTransactionDto>>> GetByBankAccount(Guid bankAccountId)
    {
        var result = await _mediator.Send(new GetAccountTransactionsByBankAccountQuery(bankAccountId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AccountTransactionDto>> Create([FromBody] CreateAccountTransactionDto dto)
    {
        var command = new CreateAccountTransactionCommand(
            dto.BankAccountId,
            dto.Description,
            dto.Category,
            dto.Date,
            dto.Amount
        );
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountTransactionDto>> Update(Guid id, [FromBody] UpdateAccountTransactionDto dto)
    {
        try
        {
            var command = new UpdateAccountTransactionCommand(
                id,
                dto.Description,
                dto.Category,
                dto.Date,
                dto.Amount
            );
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
        var result = await _mediator.Send(new DeleteAccountTransactionCommand(id));
        
        if (!result)
            return NotFound(new { message = "Transação não encontrada" });

        return NoContent();
    }
}

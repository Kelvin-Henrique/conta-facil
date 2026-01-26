using ContaFacil.API.Application.Commands.CreditCards;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.CreditCards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreditCardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CreditCardDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCreditCardsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditCardDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCreditCardByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = "Cartão de crédito não encontrado" });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreditCardDto>> Create([FromBody] CreateCreditCardDto dto)
    {
        var command = new CreateCreditCardCommand(dto.Name, dto.DueDay, dto.ClosingDay);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CreditCardDto>> Update(Guid id, [FromBody] UpdateCreditCardDto dto)
    {
        try
        {
            var command = new UpdateCreditCardCommand(id, dto.Name, dto.DueDay, dto.ClosingDay);
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
        var result = await _mediator.Send(new DeleteCreditCardCommand(id));
        
        if (!result)
            return NotFound(new { message = "Cartão de crédito não encontrado" });

        return NoContent();
    }
}

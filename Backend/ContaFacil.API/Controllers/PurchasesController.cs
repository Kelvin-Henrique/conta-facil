using ContaFacil.API.Application.Commands.Purchases;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Purchases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PurchaseDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllPurchasesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPurchaseByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = "Compra não encontrada" });

        return Ok(result);
    }

    [HttpGet("by-card/{creditCardId}")]
    public async Task<ActionResult<List<PurchaseDto>>> GetByCreditCard(Guid creditCardId)
    {
        var result = await _mediator.Send(new GetPurchasesByCreditCardQuery(creditCardId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseDto>> Create([FromBody] CreatePurchaseDto dto)
    {
        var command = new CreatePurchaseCommand(
            dto.CreditCardId,
            dto.Description,
            dto.Category,
            dto.Date,
            dto.TotalAmount,
            dto.Installments
        );
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PurchaseDto>> Update(Guid id, [FromBody] UpdatePurchaseDto dto)
    {
        try
        {
            var command = new UpdatePurchaseCommand(
                id,
                dto.Description,
                dto.Category,
                dto.Date,
                dto.TotalAmount,
                dto.Installments
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
        var result = await _mediator.Send(new DeletePurchaseCommand(id));
        
        if (!result)
            return NotFound(new { message = "Compra não encontrada" });

        return NoContent();
    }
}

using ContaFacil.API.Application.Commands.FixedBills;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.FixedBills;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FixedBillsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FixedBillsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<FixedBillDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllFixedBillsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FixedBillDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetFixedBillByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = "Conta fixa não encontrada" });

        return Ok(result);
    }

    [HttpGet("by-month")]
    public async Task<ActionResult<List<FixedBillDto>>> GetByMonthYear([FromQuery] int month, [FromQuery] int year)
    {
        var result = await _mediator.Send(new GetFixedBillsByMonthYearQuery(month, year));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<FixedBillDto>> Create([FromBody] CreateFixedBillDto dto)
    {
        var command = new CreateFixedBillCommand(
            dto.Name,
            dto.Category,
            dto.Amount,
            dto.DueDay,
            dto.Month,
            dto.Year,
            dto.IsPaid,
            dto.IsRecurring
        );
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FixedBillDto>> Update(Guid id, [FromBody] UpdateFixedBillDto dto)
    {
        try
        {
            var command = new UpdateFixedBillCommand(
                id,
                dto.Name,
                dto.Category,
                dto.Amount,
                dto.DueDay,
                dto.IsPaid,
                dto.IsRecurring
            );
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/toggle-paid")]
    public async Task<ActionResult<FixedBillDto>> TogglePaid(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new ToggleFixedBillPaidCommand(id));
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
        var result = await _mediator.Send(new DeleteFixedBillCommand(id));
        
        if (!result)
            return NotFound(new { message = "Conta fixa não encontrada" });

        return NoContent();
    }
}

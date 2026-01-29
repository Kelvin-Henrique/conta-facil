using ContaFacil.API.Application.Commands.Users;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("firebase/{firebaseUid}")]
        public async Task<ActionResult<UserDto>> GetByFirebaseUid(string firebaseUid)
        {
            var user = await _mediator.Send(new GetUserByFirebaseUidQuery(firebaseUid));
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(email));
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _mediator.Send(new CreateUserCommand(
                    dto.FirebaseUid,
                    dto.Email,
                    dto.Name
                ));

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var user = await _mediator.Send(new UpdateUserCommand(id, dto.Name));
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("login/{firebaseUid}")]
        public async Task<ActionResult> UpdateLastLogin(string firebaseUid)
        {
            await _mediator.Send(new UpdateLastLoginCommand(firebaseUid));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

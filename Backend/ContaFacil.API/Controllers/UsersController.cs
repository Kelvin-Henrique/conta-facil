using ContaFacil.API.Application.Commands.Users;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContaFacil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> ObterTodos()
        {
            var usuarios = await _mediator.Send(new ObterTodosUsuariosQuery());
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorId(int id)
        {
            var usuario = await _mediator.Send(new ObterUsuarioPorIdQuery(id));
            
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet("firebase/{firebaseUid}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorFirebaseUid(string firebaseUid)
        {
            var usuario = await _mediator.Send(new ObterUsuarioPorFirebaseUidQuery(firebaseUid));
            
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UsuarioDto>> ObterPorEmail(string email)
        {
            var usuario = await _mediator.Send(new ObterUsuarioPorEmailQuery(email));
            
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Criar([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                var usuario = await _mediator.Send(new CriarUsuarioCommand(
                    dto.FirebaseUid,
                    dto.Email,
                    dto.Nome
                ));

                return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDto>> Atualizar(int id, [FromBody] AtualizarUsuarioDto dto)
        {
            try
            {
                var usuario = await _mediator.Send(new AtualizarUsuarioCommand(id, dto.Nome));
                return Ok(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("login/{firebaseUid}")]
        public async Task<ActionResult> AtualizarUltimoLogin(string firebaseUid)
        {
            await _mediator.Send(new AtualizarUltimoLoginCommand(firebaseUid));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(int id)
        {
            try
            {
                await _mediator.Send(new ExcluirUsuarioCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

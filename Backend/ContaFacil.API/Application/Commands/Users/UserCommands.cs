using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.Users
{
    // Criar Usuario
    public record CriarUsuarioCommand(string FirebaseUid, string Email, string Nome) : IRequest<UsuarioDto>;

    // Atualizar Usuario
    public record AtualizarUsuarioCommand(int Id, string Nome) : IRequest<UsuarioDto>;

    // Atualizar Ultimo Login
    public record AtualizarUltimoLoginCommand(string FirebaseUid) : IRequest<Unit>;

    // Excluir Usuario
    public record ExcluirUsuarioCommand(int Id) : IRequest<Unit>;
}

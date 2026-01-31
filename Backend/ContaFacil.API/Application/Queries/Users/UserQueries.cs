using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.Users
{
    // Obter todos usuarios
    public record ObterTodosUsuariosQuery : IRequest<List<UsuarioDto>>;

    // Obter usuario por ID
    public record ObterUsuarioPorIdQuery(int Id) : IRequest<UsuarioDto?>;

    // Obter usuario por Firebase UID
    public record ObterUsuarioPorFirebaseUidQuery(string FirebaseUid) : IRequest<UsuarioDto?>;

    // Obter usuario por Email
    public record ObterUsuarioPorEmailQuery(string Email) : IRequest<UsuarioDto?>;
}

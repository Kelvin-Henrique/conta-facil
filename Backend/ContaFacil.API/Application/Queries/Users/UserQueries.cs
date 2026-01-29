using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Queries.Users
{
    // Get all users
    public record GetAllUsersQuery : IRequest<List<UserDto>>;

    // Get user by ID
    public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;

    // Get user by Firebase UID
    public record GetUserByFirebaseUidQuery(string FirebaseUid) : IRequest<UserDto?>;

    // Get user by Email
    public record GetUserByEmailQuery(string Email) : IRequest<UserDto?>;
}

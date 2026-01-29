using ContaFacil.API.Application.DTOs;
using MediatR;

namespace ContaFacil.API.Application.Commands.Users
{
    // Create User
    public record CreateUserCommand(string FirebaseUid, string Email, string Name) : IRequest<UserDto>;

    // Update User
    public record UpdateUserCommand(int Id, string Name) : IRequest<UserDto>;

    // Update Last Login
    public record UpdateLastLoginCommand(string FirebaseUid) : IRequest<Unit>;

    // Delete User
    public record DeleteUserCommand(int Id) : IRequest<Unit>;
}

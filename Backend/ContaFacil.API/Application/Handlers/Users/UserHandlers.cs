using AutoMapper;
using ContaFacil.API.Application.Commands.Users;
using ContaFacil.API.Application.DTOs;
using ContaFacil.API.Application.Queries.Users;
using ContaFacil.API.Domain.Entities;
using ContaFacil.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContaFacil.API.Application.Handlers.Users
{
    // Create User Handler
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Verificar se já existe usuário com esse FirebaseUid ou Email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid || u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Usuário já existe");
            }

            var user = new User
            {
                FirebaseUid = request.FirebaseUid,
                Email = request.Email,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }

    // Update User Handler
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");
            }

            user.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }

    // Update Last Login Handler
    public class UpdateLastLoginCommandHandler : IRequestHandler<UpdateLastLoginCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public UpdateLastLoginCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateLastLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid, cancellationToken);

            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }

    // Delete User Handler
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    // Query Handlers
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UserDto>>(users);
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
    }

    public class GetUserByFirebaseUidQueryHandler : IRequestHandler<GetUserByFirebaseUidQuery, UserDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByFirebaseUidQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByFirebaseUidQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid, cancellationToken);
            
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
    }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByEmailQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }
    }
}

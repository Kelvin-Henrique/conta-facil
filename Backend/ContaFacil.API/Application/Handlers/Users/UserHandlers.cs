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
    // Criar Usuario Handler
    public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, UsuarioDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CriarUsuarioCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Verificar se já existe usuário com esse FirebaseUid ou Email
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid || u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Usuário já existe");
            }

            var user = new Usuario
            {
                FirebaseUid = request.FirebaseUid,
                Email = request.Email,
                Nome = request.Nome,
                CriadoEm = DateTime.UtcNow,
                Ativo = true
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UsuarioDto>(user);
        }
    }

    // Atualizar Usuario Handler
    public class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, UsuarioDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AtualizarUsuarioCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");
            }

            user.Nome = request.Nome;
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UsuarioDto>(user);
        }
    }

    // Atualizar Ultimo Login Handler
    public class AtualizarUltimoLoginCommandHandler : IRequestHandler<AtualizarUltimoLoginCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public AtualizarUltimoLoginCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AtualizarUltimoLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid, cancellationToken);

            if (user != null)
            {
                user.UltimoLoginEm = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }

    // Excluir Usuario Handler
    public class ExcluirUsuarioCommandHandler : IRequestHandler<ExcluirUsuarioCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public ExcluirUsuarioCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ExcluirUsuarioCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {request.Id} não encontrado");
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    // Query Handlers
    public class ObterTodosUsuariosQueryHandler : IRequestHandler<ObterTodosUsuariosQuery, List<UsuarioDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ObterTodosUsuariosQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDto>> Handle(ObterTodosUsuariosQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Usuarios
                .Where(u => u.Ativo)
                .OrderByDescending(u => u.CriadoEm)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UsuarioDto>>(users);
        }
    }

    public class ObterUsuarioPorIdQueryHandler : IRequestHandler<ObterUsuarioPorIdQuery, UsuarioDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ObterUsuarioPorIdQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Handle(ObterUsuarioPorIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios.FindAsync(new object[] { request.Id }, cancellationToken);
            return user != null ? _mapper.Map<UsuarioDto>(user) : null;
        }
    }

    public class ObterUsuarioPorFirebaseUidQueryHandler : IRequestHandler<ObterUsuarioPorFirebaseUidQuery, UsuarioDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ObterUsuarioPorFirebaseUidQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Handle(ObterUsuarioPorFirebaseUidQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid, cancellationToken);
            
            return user != null ? _mapper.Map<UsuarioDto>(user) : null;
        }
    }

    public class ObterUsuarioPorEmailQueryHandler : IRequestHandler<ObterUsuarioPorEmailQuery, UsuarioDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ObterUsuarioPorEmailQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Handle(ObterUsuarioPorEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            
            return user != null ? _mapper.Map<UsuarioDto>(user) : null;
        }
    }
}

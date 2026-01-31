namespace ContaFacil.API.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string FirebaseUid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public DateTime? UltimoLoginEm { get; set; }
        public bool Ativo { get; set; }
    }

    public class CriarUsuarioDto
    {
        public string FirebaseUid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }

    public class AtualizarUsuarioDto
    {
        public string Nome { get; set; } = string.Empty;
    }
}

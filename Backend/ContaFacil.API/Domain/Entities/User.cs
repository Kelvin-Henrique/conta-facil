namespace ContaFacil.API.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string FirebaseUid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLoginEm { get; set; }
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public ICollection<ContaBancaria> ContasBancarias { get; set; } = new List<ContaBancaria>();
        public ICollection<CartaoCredito> CartoesCredito { get; set; } = new List<CartaoCredito>();
        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public ICollection<TransacaoConta> TransacoesConta { get; set; } = new List<TransacaoConta>();
        public ICollection<ContaFixa> ContasFixas { get; set; } = new List<ContaFixa>();
    }
}

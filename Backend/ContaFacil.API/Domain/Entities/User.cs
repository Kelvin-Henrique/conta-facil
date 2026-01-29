namespace ContaFacil.API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirebaseUid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Relacionamentos
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();
        public ICollection<FixedBill> FixedBills { get; set; } = new List<FixedBill>();
    }
}

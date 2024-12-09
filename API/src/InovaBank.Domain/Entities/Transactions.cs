namespace InovaBank.Domain.Entities
{
    public class Transactions : EntityBase
    {
        public decimal Value { get; set; }
        public string Type { get; set; } = string.Empty;
        public string AccountDestiny { get; set; } = String.Empty;
        public string? AccountOrigin { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public Account? AccountDestinyEntity { get; set; }
        public Account? AccountOriginEntity { get; set; }

    }
}

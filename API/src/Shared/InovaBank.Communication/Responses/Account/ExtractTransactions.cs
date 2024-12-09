namespace InovaBank.Communication.Responses.Account
{
    public class ExtractTransactions
    {
        public DateTime Date { get; set; }
        public string OriginAccount { get; set; } = string.Empty;
        public string OriginName { get; set; } = string.Empty;
        public string DestinyAccount { get; set; } = string.Empty;
        public string DestinyName { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string TransactionType { get; set; }
    }
}

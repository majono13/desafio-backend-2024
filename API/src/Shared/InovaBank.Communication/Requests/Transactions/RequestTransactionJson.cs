namespace InovaBank.Communication.Requests.Transactions
{
    public class RequestTransactionJson
    {
        public decimal Value { get; set; }
        public string Account {  get; set; } = string.Empty;
        public string Digit { get; set; } = string.Empty;
        public string Agency { get; set; } = string.Empty;
    }
}

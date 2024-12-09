namespace InovaBank.Communication.Responses.Account
{
    public class ResponseExtractJson
    {
        public decimal Balance { get; set; }
        public decimal TotalOutputs { get; set; }
        public decimal TotalInputs { get; set; }
        public required IEnumerable<ExtractTransactions> Outputs { get; set; }
        public required IEnumerable<ExtractTransactions> Inputs { get; set; }
    }
}

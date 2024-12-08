namespace InovaBank.Communication.Responses.Account
{
    public class ResponseAccountJson
    {
        public string TradeName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Cnpj { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string Digit { get; set; } = "";
        public string Agency { get; set; } = "";
        public string Document { get; set; } = "";
        public long Balance { get; set; }
        public string Id { get; set; } = "";
    }
}

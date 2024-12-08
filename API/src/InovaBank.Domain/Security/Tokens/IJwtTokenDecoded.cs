namespace InovaBank.Domain.Security.Tokens
{
    public interface IJwtTokenDecoded
    {
        public string Decoded(string userIdentifier);
    }
}

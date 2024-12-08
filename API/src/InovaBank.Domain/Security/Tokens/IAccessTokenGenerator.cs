namespace InovaBank.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        public string Generate(string userIdentifier);
    }
}

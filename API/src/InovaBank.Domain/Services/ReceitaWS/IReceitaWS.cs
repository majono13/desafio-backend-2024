namespace InovaBank.Domain.Services.ReceitaWS
{
    public interface IReceitaWS
    {
        public Task<ReceitaWsResponse?> GetCnpjInfo(string cnpj);
    }
}

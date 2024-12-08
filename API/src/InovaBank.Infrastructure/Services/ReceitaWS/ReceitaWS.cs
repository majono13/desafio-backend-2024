using FluentMigrator.Infrastructure;
using InovaBank.Domain.Services.ReceitaWS;
using Newtonsoft.Json;

namespace InovaBank.Infrastructure.Services.ReceitaWS
{
    public class ReceitaWS : IReceitaWS
    {
        private readonly HttpClient _httpClient;

        public ReceitaWS()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ReceitaWsResponse?> GetCnpjInfo(string cnpj)
        {
                var url = $"https://www.receitaws.com.br/v1/cnpj/{cnpj}";
                var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Lê a resposta como uma string
                var content = await response.Content.ReadAsStringAsync();

                // Desserializa a resposta JSON em um objeto
                var companyInfo = JsonConvert.DeserializeObject<dynamic>(content);

                // Exibe as informações (exemplo)
                if (companyInfo != null)
                {
                    return new ReceitaWsResponse
                    {
                        TradeName = companyInfo.fantasia,
                        Name = companyInfo.nome,
                    };
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                throw new Exception("Erro 429: Muitas requisições. Tente novamente mais tarde.");
            }

            return null;
        }
    }
}

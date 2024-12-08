using Microsoft.AspNetCore.Http;

namespace InovaBank.Communication.Requests.Account
{
    public class RequesteRegisterAccountJson
    {
        public string Cnpj { get; set; } = "";
        public IFormFile Document { get; set; }
    }
}

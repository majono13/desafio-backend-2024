using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;
using Microsoft.AspNetCore.Http;

namespace InovaBank.Application.UseCases.Account.Register
{
    public interface IRegisterAccountUseCase
    {
        public Task<ResponseRegisterAccountJson> Execute(RequesteRegisterAccountJson request);
    }
}

using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;

namespace InovaBank.Application.UseCases.Account.Get
{
    public interface IGetAccountUseCase
    {
        public Task<ResponseAccountJson> GetByAccountNumber(RequestAccountJson request);
    }
}

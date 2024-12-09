using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Requests.Transactions;
using InovaBank.Communication.Responses.Account;

namespace InovaBank.Application.UseCases.Account.Get
{
    public interface IGetAccountUseCase
    {
        public Task<ResponseAccountJson> GetByAccountNumber(RequestAccountJson request);
        public Task<ResponseExtractJson> GetExtract(RequestAccountJson request);
    }
}

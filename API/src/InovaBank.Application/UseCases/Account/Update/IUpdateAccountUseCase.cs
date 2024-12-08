using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;

namespace InovaBank.Application.UseCases.Account.Update
{
    public interface IUpdateAccountUseCase
    {
        public Task<ResponseRegisterAccountJson> Execute(RequesteRegisterAccountJson request);
        public Task MarkAsDeleted(RequestAccountJson request);
    }
}

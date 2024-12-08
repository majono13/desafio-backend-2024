using InovaBank.Communication.Requests.User;
using InovaBank.Communication.Responses.User;

namespace InovaBank.Application.UseCases.User.Login
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
    }
}

using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;

namespace InovaBank.Application.UseCases.User.Login
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
    }
}

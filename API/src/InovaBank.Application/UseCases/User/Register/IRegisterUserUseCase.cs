using InovaBank.Communication.Requests.User;
using InovaBank.Communication.Responses.User;

namespace InovaBank.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}

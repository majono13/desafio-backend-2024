using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;

namespace InovaBank.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}

using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.Services.Cryptography;
using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;
using InovaBank.Domain.Repositories.User;
using InovaBank.Domain.Security.Tokens;

namespace InovaBank.Application.UseCases.User.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public DoLoginUseCase(
            IUserReadOnlyRepository repository, 
            PasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
            var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

            return new ResponseRegisteredUserJson
            {
                Email = user.Email,
                Token = _accessTokenGenerator.Generate(user.Id)
            };
        }
    }
}

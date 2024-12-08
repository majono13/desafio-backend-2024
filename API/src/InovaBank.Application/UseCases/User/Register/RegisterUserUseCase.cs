using System.Text.RegularExpressions;
using AutoMapper;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.Services.Cryptography;
using InovaBank.Communication.Requests;
using InovaBank.Communication.Responses;
using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.User;
using InovaBank.Domain.Security.Tokens;

namespace InovaBank.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUserUseCase(
            IUserWriteOnlyRepository userWriteOnlyRepository, 
            IUserReadOnlyRepository userReadOnlyRepository,
            IMapper mapper,
            PasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.Id = Guid.NewGuid().ToString();

            await _userWriteOnlyRepository.Create(user);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Email = request.Email,
                Token = _accessTokenGenerator.Generate(user.Id)
            };
        }

        private async Task Validate(RequestRegisterUserJson request) {
            var validator = new RegisterUserValidator();

            var validatorResult = validator.Validate(request);

            await ValidateFieldAsync(request.Email, _userReadOnlyRepository.ExistUserWithEmail, ErrorsMessages.EMAIL_REGISTERED, validatorResult);

            if (validatorResult.IsValid == false) 
            {
                var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
         
        }

        private async Task ValidateFieldAsync(string fieldValue, Func<string, Task<bool>> validationMethod, string errorMessage, FluentValidation.Results.ValidationResult validatorResult)
        {
            var isValid = await validationMethod(fieldValue);

            if (isValid)
            {
                validatorResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", errorMessage));
            }
        }
    }
}

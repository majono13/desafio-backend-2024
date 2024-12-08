using System.Text.RegularExpressions;
using AutoMapper;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.Services.Account;
using InovaBank.Application.UserSession;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;
using InovaBank.Domain.Enums;
using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.Account;
using InovaBank.Domain.Services.ReceitaWS;

namespace InovaBank.Application.UseCases.Account.Register
{
    public class RegisterAccountUseCase : IRegisterAccountUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository _accountWriteOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserContext _userContext;
        private ReceitaWsResponse _cnpjInfo;
        private readonly IReceitaWS _receitaWS;
        private readonly ReplaceCnpj _replaceCnpj;

        public RegisterAccountUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            UserContext userContext,
            IReceitaWS receitaWS
            )
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _accountWriteOnlyRepository = accountWriteOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _receitaWS = receitaWS;
            _replaceCnpj = new ReplaceCnpj();
            _cnpjInfo = new ReceitaWsResponse();
        }

        public async Task<ResponseRegisterAccountJson> Execute(RequesteRegisterAccountJson request)
        {
            await Validate(request);

            var account = _mapper.Map<Domain.Entities.Account>(request);
            await SetAccountInfo(account);
            account.Document = await new ConvertFileToBase64().ConvertToString(request.Document);

            await _accountWriteOnlyRepository.Create(account);
            await _unitOfWork.Commit();

            return new ResponseRegisterAccountJson
            {
                AccountNumber = account.AccountNumber,
                Digit = account.Digit,
                Agency = account.Agency,
                Cnpj = account.Cnpj,
            };
        }

        private async Task SetAccountInfo(Domain.Entities.Account account)
        {
            var generateRandomAccount = new GenerateRandomAccount();
            account.Name = _cnpjInfo.Name;
            account.TradeName = _cnpjInfo.TradeName;
            account.Digit = generateRandomAccount.GenerateRandomDigit();
            account.Agency = ((int)Agency.AGENCY).ToString("D4");
            account.Balance = 0;
            account.Cnpj = _replaceCnpj.RemoveSpecialCharacters(account.Cnpj);
            account.UserId = _userContext.UserId;

            do
            {
                account.AccountNumber = generateRandomAccount.GenerateRandomValue();
            }
            while (await _accountReadOnlyRepository.GetByAccount(account.AccountNumber) is not null);

        }

        private async Task Validate(RequesteRegisterAccountJson request)
        {
            var validator = new RegisterAccountValidator();

            var validationResult = validator.Validate(request);

            await ValidateUser(validationResult);
            await ValidateCnpj(_replaceCnpj.RemoveSpecialCharacters(request.Cnpj), validationResult);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }

        private async Task ValidateCnpj(string cnpj, FluentValidation.Results.ValidationResult validatorResult)
        {
            var cnpjIfo = await new CnpjValidate(_receitaWS).Validate(cnpj, validatorResult, _accountReadOnlyRepository);

            if (cnpjIfo != null)
            {
                _cnpjInfo = cnpjIfo;
            }
        }

        private async Task ValidateUser(FluentValidation.Results.ValidationResult validatorResult)
        {
            var userAccount = await _accountReadOnlyRepository.GetActiveAccountUser(_userContext.UserId);

            if (userAccount is not null)
            {
                validatorResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", ErrorsMessages.USER_aCCOUNT_REGISTERED));
            }
        }
    }
}

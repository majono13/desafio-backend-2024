using AutoMapper;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.Services.Account;
using InovaBank.Application.UseCases.Account.Register;
using InovaBank.Application.UserSession;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;
using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.Account;
using InovaBank.Domain.Services.ReceitaWS;

namespace InovaBank.Application.UseCases.Account.Update
{
    public class UpdateAccountUseCase : IUpdateAccountUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository _accountWriteOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserContext _userContext;
        private ReceitaWsResponse _cnpjInfo;
        private readonly IReceitaWS _receitaWS;
        private readonly ReplaceCnpj _replaceCnpj;

        public UpdateAccountUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            UserContext userContext,
            IReceitaWS receitaWS)
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _accountWriteOnlyRepository = accountWriteOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _receitaWS = receitaWS;
            _cnpjInfo = new ReceitaWsResponse();
            _replaceCnpj = new ReplaceCnpj();

        }
        public async Task<ResponseRegisterAccountJson> Execute(RequesteRegisterAccountJson request)
        {
            var account = await Validate(request);
            account.Cnpj = _replaceCnpj.RemoveSpecialCharacters(request.Cnpj) ?? account.Cnpj;
            account.Document = await new ConvertFileToBase64().ConvertToString(request.Document);

            await _accountWriteOnlyRepository.Update(account);
            await _unitOfWork.Commit();

            return new ResponseRegisterAccountJson
            {
                AccountNumber = account.AccountNumber,
                Digit = account.Digit,
                Agency = account.Agency,
                Cnpj = account.Cnpj,
            };
        }

        private async Task<Domain.Entities.Account> Validate(RequesteRegisterAccountJson request)
        {
            var validator = new RegisterAccountValidator();
            var validationResult = validator.Validate(request);

            var account = await _accountReadOnlyRepository.GetActiveAccountUser(_userContext.UserId);

            if (account != null)
            {
                if (account.Cnpj != _replaceCnpj.RemoveSpecialCharacters(request.Cnpj))
                {
                    await ValidateCnpj(_replaceCnpj.RemoveSpecialCharacters(request.Cnpj), validationResult);
                    account.Name = _cnpjInfo.Name;
                    account.TradeName = _cnpjInfo.TradeName;
                }
            } 
            else
            {
                throw new AccountNotFoundException();
            }

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }

            return account!;
        }

        private async Task ValidateCnpj(string cnpj, FluentValidation.Results.ValidationResult validatorResult)
        {
            var cnpjIfo = await new CnpjValidate(_receitaWS).Validate(cnpj, validatorResult, _accountReadOnlyRepository);

            if (cnpjIfo != null)
            {
                _cnpjInfo = cnpjIfo;
            }
        }

        public async Task MarkAsDeleted(RequestAccountJson request)
        {
            if (!String.IsNullOrEmpty(request?.AccountNumber)) 
            { 
                var account = await _accountReadOnlyRepository.GetActiveAccountUserByAccountNumber(_userContext.UserId, request.AccountNumber);

                if (account != null)
                {
                   account.Active = false;

                    await _accountWriteOnlyRepository.Update(account);
                    await _unitOfWork.Commit();
                    return;
                } 
            }
                throw new AccountNotFoundException();
        }
    }
}

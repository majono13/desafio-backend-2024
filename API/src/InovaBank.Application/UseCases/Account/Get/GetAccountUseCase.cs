using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.UserSession;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Responses.Account;
using InovaBank.Domain.Repositories.Account;

namespace InovaBank.Application.UseCases.Account.Get
{
    public class GetAccountUseCase : IGetAccountUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly UserContext _userContext;

        public GetAccountUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            UserContext userContext
            )
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _userContext = userContext;
        }
        public async Task<ResponseAccountJson> GetByAccountNumber(RequestAccountJson request)
        {
            var account = await _accountReadOnlyRepository.GetActiveAccountUserByAccountNumber(_userContext.UserId, request.AccountNumber);

            if (account != null)
            {
                return new ResponseAccountJson()
                {

                    TradeName = account.TradeName,
                    Name = account.Name,
                    Cnpj = account.Cnpj,
                    AccountNumber = account.AccountNumber,
                    Digit = account.Digit,
                    Agency = account.Agency,
                    Document = account.Document,
                    Balance = account.Balance,
                    Id = account.Id,
                };
            }

            throw new AccountNotFoundException();
        }
    }
}

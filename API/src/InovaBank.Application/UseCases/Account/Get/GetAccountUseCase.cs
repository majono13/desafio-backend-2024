using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.UseCases.Transaction.Get;
using InovaBank.Application.UserSession;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Requests.Transactions;
using InovaBank.Communication.Responses.Account;
using InovaBank.Domain.Constants;
using InovaBank.Domain.Repositories.Account;

namespace InovaBank.Application.UseCases.Account.Get
{
    public class GetAccountUseCase : IGetAccountUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly UserContext _userContext;
        private readonly IGetTransactionsUseCase _getTransactionsUseCase;

        public GetAccountUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            UserContext userContext,
            IGetTransactionsUseCase getTransactionsUseCase
            )
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _userContext = userContext;
            _getTransactionsUseCase = getTransactionsUseCase;
        }
        public async Task<ResponseAccountJson> GetByAccountNumber(RequestAccountJson request)
        {
            if (String.IsNullOrEmpty(request.AccountNumber)) 
            {
                throw new ErrorOnValidationException([ErrorsMessages.ACCOUNT_REQUIRED]);
            }
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

        public async Task<ResponseExtractJson> GetExtract(RequestAccountJson request)
        {
            if (String.IsNullOrEmpty(request.AccountNumber))
            {
                throw new ErrorOnValidationException([ErrorsMessages.ACCOUNT_REQUIRED]);
            }

            var account = await _accountReadOnlyRepository.GetActiveAccountUserByAccountNumber(_userContext.UserId, request.AccountNumber);

            if (account != null)
            {
                var transactions = await _getTransactionsUseCase.GetTransactionsByAccount(account.Id);

                var outputs = GetOutputTransactions(transactions, account.Id);
                var inputs = GetInputTransactions(transactions, account.Id);

                return new ResponseExtractJson
                {
                    Balance = account.Balance,
                    TotalInputs = inputs.Aggregate(0m, (acc, item) => acc + item.Value),
                    TotalOutputs = outputs.Aggregate(0m, (acc, item) => acc + item.Value),
                    Outputs = outputs,
                    Inputs = inputs
                };
            }

            throw new AccountNotFoundException();
        }

        private IEnumerable<ExtractTransactions> GetOutputTransactions(IEnumerable<Domain.Entities.Transactions> transactions, string accountId)
        {
            var filteredTransactions = transactions.Where(transaction => transaction.Type == TransactionTypes.TypeOutput &&
            (transaction.AccountOrigin == accountId || (transaction.AccountDestiny == accountId && transaction.TransactionType == TransactionTypes.TypeWithdrawal)));
            return MapEntities(filteredTransactions);
                
        }

        private IEnumerable<ExtractTransactions> GetInputTransactions(IEnumerable<Domain.Entities.Transactions> transactions, string accountId)
        {

            var filteredTransactions = transactions.Where(transaction => transaction.Type == TransactionTypes.TypeInput && transaction.AccountDestiny == accountId);
            return MapEntities(filteredTransactions);
        }

        private IEnumerable<ExtractTransactions> MapEntities(IEnumerable<Domain.Entities.Transactions> transactions)
        {
            return transactions.Select(transaction =>
             {
                 return new ExtractTransactions()
                 {
                     Date = transaction.CreatedAt,
                     OriginAccount = transaction.AccountOriginEntity != null
                     ? $"{transaction.AccountOriginEntity.AccountNumber}-{transaction.AccountOriginEntity.Digit}"
                     : "",
                     DestinyAccount = transaction.AccountDestinyEntity != null
                     ? $"{transaction.AccountDestinyEntity.AccountNumber}-{transaction.AccountDestinyEntity.Digit}"
                     : "",
                     DestinyName = transaction?.AccountDestinyEntity?.Name ?? "",
                     OriginName = transaction?.AccountOriginEntity?.Name ?? "",
                     Value = transaction!.Value,
                     TransactionType = transaction.TransactionType,
                 };
             });
        }
    }
}

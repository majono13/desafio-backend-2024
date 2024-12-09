using System.Data.Common;
using System.Security.Principal;
using System.Transactions;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Application.Services.Account;
using InovaBank.Application.UseCases.Account.Register;
using InovaBank.Application.UserSession;
using InovaBank.Communication.Requests.Account;
using InovaBank.Communication.Requests.Transactions;
using InovaBank.Domain.Constants;
using InovaBank.Domain.Entities;
using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.Account;
using InovaBank.Domain.Repositories.Transactions;

namespace InovaBank.Application.UseCases.Transaction.Register
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly IAccountReadOnlyRepository _accountReadOnlyRepository;
        private readonly IAccountWriteOnlyRepository _accountWriteOnlyRepository;
        private readonly ITransactionsWriteOnlyRepository _transactionsWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserContext _userContext;

        public TransactionUseCase(
            IAccountReadOnlyRepository accountReadOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository,
            ITransactionsWriteOnlyRepository transactionsWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            UserContext userContext
            )
        {
            _accountReadOnlyRepository = accountReadOnlyRepository;
            _accountWriteOnlyRepository = accountWriteOnlyRepository;
            _transactionsWriteOnlyRepository = transactionsWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task Deposit(RequestTransactionJson request)
        {
            Validate(request);

            var account = await GetAccount(request);
            var transaction = CreateTransaction(request.Value, TransactionTypes.TypeInput, null, account.Id, TransactionTypes.TypeDeposit);

            account.Balance += request.Value;

            await SaveTransaction(account, transaction);

        }

        private void Validate(RequestTransactionJson request)
        {
            var validator = new TransactionValidator();
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }

        private async Task<Domain.Entities.Account> GetAccount(RequestTransactionJson request)
        {
            var account = await _accountReadOnlyRepository.GetAccountByInfo(request.Account, request.Digit, request.Agency);

            return account == null ? throw new AccountNotFoundException() : account;
        }

        private async Task SaveTransaction(Domain.Entities.Account account, Transactions transaction)
        {
            await _transactionsWriteOnlyRepository.Create(transaction);
            await _accountWriteOnlyRepository.Update(account);
            await _unitOfWork.Commit();
        }

        public async Task Withdrawal(RequestTransactionJson request)
        {
            Validate(request);
            var account = await GetAccount(request);

            if (account.UserId != _userContext?.UserId)
            {
                throw new AccountNotFoundException();
            }


            if (account?.Balance < request.Value)
            {
                throw new ErrorOnValidationException([ErrorsMessages.INSUFFICIENT_BALANCE]);
            }

            var transaction = CreateTransaction(request.Value, TransactionTypes.TypeOutput, null, account.Id, TransactionTypes.TypeWithdrawal);

            account.Balance -= request.Value;
            await SaveTransaction(account, transaction);
        }

        public async Task Transfer(RequestTransactionJson request)
        {
            Validate(request);
            var originAccount = await _accountReadOnlyRepository.GetActiveAccountUser(_userContext.UserId);
            var destinyAccount = await GetAccount(request);

            if (originAccount?.Balance < request.Value)
            {
                throw new ErrorOnValidationException([ErrorsMessages.INSUFFICIENT_BALANCE]);
            }

            if (originAccount?.Id == destinyAccount.Id)
            {
                throw new ErrorOnValidationException([ErrorsMessages.INVALID_DESTINY]);
            }

            var originTransaction = CreateTransaction(request.Value, TransactionTypes.TypeOutput, originAccount!.Id, destinyAccount.Id, TransactionTypes.TypeTransfer);
            var destinyTransaction = CreateTransaction(request.Value, TransactionTypes.TypeInput, originAccount.Id, destinyAccount.Id, TransactionTypes.TypeTransfer);

            originAccount.Balance -= request.Value;
            destinyAccount.Balance += request.Value;

            await _transactionsWriteOnlyRepository.Create(originTransaction);
            await _accountWriteOnlyRepository.Update(originAccount);
            await _transactionsWriteOnlyRepository.Create(destinyTransaction);
            await _accountWriteOnlyRepository.Update(destinyAccount);
            await _unitOfWork.Commit();

        }

        private Transactions CreateTransaction(decimal value, string type, string? accountOrigin, string accountDestiny, string transactionType)
        {
            return new Transactions
            {
                Value = value,
                Type = type,
                AccountOrigin = accountOrigin,
                AccountDestiny = accountDestiny,
                TransactionType = transactionType
            };
        }
    }
}

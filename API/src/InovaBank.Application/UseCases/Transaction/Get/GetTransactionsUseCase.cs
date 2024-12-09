using InovaBank.Communication.Requests.Transactions;
using InovaBank.Domain.Entities;
using InovaBank.Domain.Repositories.Transactions;

namespace InovaBank.Application.UseCases.Transaction.Get
{
    public class GetTransactionsUseCase : IGetTransactionsUseCase
    {
        private readonly ItransactionsReadOnlyRepository _transactionsReadOnlyRepository;

        public GetTransactionsUseCase(ItransactionsReadOnlyRepository transactionsReadOnlyRepository)
        {
            _transactionsReadOnlyRepository = transactionsReadOnlyRepository;
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByAccount(string accountId)
        {
            return await _transactionsReadOnlyRepository.GetTransactionsByAccount(accountId);

        }
    }
}

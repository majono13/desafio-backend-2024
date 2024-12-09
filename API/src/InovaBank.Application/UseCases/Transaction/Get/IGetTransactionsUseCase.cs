using InovaBank.Communication.Requests.Transactions;

namespace InovaBank.Application.UseCases.Transaction.Get
{
    public interface IGetTransactionsUseCase
    {
        public Task<IEnumerable<Domain.Entities.Transactions>> GetTransactionsByAccount(string accountId);
    }
}

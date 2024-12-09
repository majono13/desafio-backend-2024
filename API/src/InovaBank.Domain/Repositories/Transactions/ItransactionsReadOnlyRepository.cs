namespace InovaBank.Domain.Repositories.Transactions
{
    public interface ItransactionsReadOnlyRepository
    {
        public Task<IEnumerable<Entities.Transactions>> GetTransactionsByAccount(string accountId);
    }
}

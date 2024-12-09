namespace InovaBank.Domain.Repositories.Transactions
{
    public interface ITransactionsWriteOnlyRepository
    {
        public Task Create(Entities.Transactions transaction);
    }
}

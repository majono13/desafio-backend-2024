using InovaBank.Domain.Entities;
using InovaBank.Domain.Repositories.Transactions;
using Microsoft.EntityFrameworkCore;

namespace InovaBank.Infrastructure.DataAccess.Repositories
{
    public class TransactionRepository : ItransactionsReadOnlyRepository, ITransactionsWriteOnlyRepository
    {
        private readonly InovaBankDbContext _dbContext;

        public TransactionRepository(InovaBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Transactions transaction)
        {
            transaction.Id = Guid.NewGuid().ToString();
            await _dbContext.Transactions.AddAsync(transaction);
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByAccount(string accountId)
        {
            return await _dbContext.Transactions
                .Where(transaction => transaction.AccountOrigin == accountId || transaction.AccountDestiny == accountId)
                .Include(transaction => transaction.AccountDestinyEntity)
                .Include(transaction => transaction.AccountOriginEntity)
                .ToListAsync();
        }
    }
}

using InovaBank.Domain.Entities;
using InovaBank.Domain.Repositories.Account;
using Microsoft.EntityFrameworkCore;

namespace InovaBank.Infrastructure.DataAccess.Repositories
{
    public class AccountReposiory : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
    {
        private readonly InovaBankDbContext _dbContext;

        public AccountReposiory(InovaBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Account account)
        {
            account.Id = Guid.NewGuid().ToString();
            await _dbContext.Accounts.AddAsync(account);
        }

        public Task Delete(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAccountWithCnpj(string cnpj)
        {
            return await _dbContext.Accounts.AnyAsync(account => account.Cnpj == cnpj);
        }

        public async Task<Account?> GetAccountByInfo(string accountNumber, string digit, string agency)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(account => 
            account.AccountNumber == accountNumber  && account.Active);
        }

        public async Task<Account?> GetActiveAccountUser(string userId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(account => account.UserId == userId && account.Active);
        }

        public async Task<Account?> GetActiveAccountUserByAccountNumber(string userId, string accountNumber)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(account => 
            account.UserId == userId && account.Active && account.AccountNumber == accountNumber);
        }

        public async Task<Account?> GetByAccount(string accountNumber)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(account => account.AccountNumber == accountNumber);
        }

        public async Task<Account?> GetById(string id)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(account => account.Id == id);
        }

        public async Task<IEnumerable<Account>> GetByUser(string userId)
        {
            return await _dbContext.Accounts.Where(account => account.UserId == userId)
            .ToListAsync();
        }

        public async Task Update(Account account)
        {
            _dbContext.Accounts.Update(account);
        }
    }
}

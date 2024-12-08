namespace InovaBank.Domain.Repositories.Account
{
    public interface IAccountReadOnlyRepository
    {
        public Task<IEnumerable<Entities.Account>> GetByUser(string userId);
        public Task<Entities.Account?> GetActiveAccountUser(string userId);
        public Task<Entities.Account?> GetActiveAccountUserByAccountNumber(string userId, string accountNumber);
        public Task<Entities.Account?> GetById(string id);
        public Task<bool> ExistsAccountWithCnpj(string cnpj);
        public Task<Entities.Account?> GetByAccount(string accountNumber);
    }
}

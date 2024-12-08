namespace InovaBank.Domain.Repositories.Account
{
    public interface IAccountWriteOnlyRepository
    {
        public Task Create(Entities.Account account);
        public Task Delete(Entities.Account account);
        public Task Update(Entities.Account account);
    }
}

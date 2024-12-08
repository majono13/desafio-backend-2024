namespace InovaBank.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        public Task<bool> ExistUserWithEmail(string email);
        public Task<Entities.User?> GetByEmailAndPassword(string email, string password);
        public Task<Entities.User?> GetById(string id);
    }
}

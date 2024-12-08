using InovaBank.Domain.Entities;
using InovaBank.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace InovaBank.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly InovaBankDbContext _dbContext;

        public UserRepository(InovaBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email == email);
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password));
        }

        public async Task<User?> GetById(string id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}

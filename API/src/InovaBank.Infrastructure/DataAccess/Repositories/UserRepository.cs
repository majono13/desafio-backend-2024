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
    }
}

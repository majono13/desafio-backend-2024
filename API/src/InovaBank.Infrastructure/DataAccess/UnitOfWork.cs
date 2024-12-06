using InovaBank.Domain.Repositories;

namespace InovaBank.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InovaBankDbContext _dbContext;

        public UnitOfWork(InovaBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

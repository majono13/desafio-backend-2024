using InovaBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InovaBank.Infrastructure.DataAccess
{
    public class InovaBankDbContext : DbContext
    {
        public InovaBankDbContext(DbContextOptions options) : base (options) 
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InovaBankDbContext).Assembly);
        }
    }
}
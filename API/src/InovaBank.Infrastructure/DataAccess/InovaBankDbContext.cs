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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasOne(t => t.AccountDestinyEntity)
                      .WithMany()
                      .HasForeignKey(t => t.AccountDestiny);

                entity.HasOne(t => t.AccountOriginEntity)
                      .WithMany()
                      .HasForeignKey(t => t.AccountOrigin);
            });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InovaBankDbContext).Assembly);

        }
    }
}
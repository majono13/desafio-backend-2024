using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.User;
using InovaBank.Infrastructure.DataAccess;
using InovaBank.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InovaBank.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddDbContext(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InovaBankDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("Connection"); ;
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
                opt.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

using System.Reflection;
using FluentMigrator.Runner;
using InovaBank.Domain.Repositories;
using InovaBank.Domain.Repositories.User;
using InovaBank.Domain.Security.Tokens;
using InovaBank.Infrastructure.DataAccess;
using InovaBank.Infrastructure.DataAccess.Repositories;
using InovaBank.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace InovaBank.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddToken(services, configuration);
            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
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

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentMigratorCore().ConfigureRunner(opt =>
            {
                var connectionString = configuration.GetConnectionString("Connection");
                opt.AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("InovaBank.Infrastructure")).For.All();
            });
        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration) 
        {
            var expirationTimeMinutes = int.Parse(configuration.GetSection("Settings:Jwt:ExpirationTimeMinutes").Value!);
            var signingKey = configuration.GetSection("Settings:Jwt:SigningKey").Value;

            services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IJwtTokenDecoded, JwtTokenDecoded>();
        }
    }
}

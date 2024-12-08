using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace InovaBank.Infrastructure.Migrations
{
    public static class DatabaseMigration
    {
        public static void Migrate(string connectionString, IServiceProvider serviceProvider)
        {
            EnsureDatabaseCreated(connectionString);

            MigrationDatabase(serviceProvider);
        }

        private static void EnsureDatabaseCreated(string connectionString) //Garante que o schema seja criado no banco
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.Database;

            connectionStringBuilder.Remove("Database"); //remove conexão com o schema (para o caso de ele não existir) antes de realizar a conexão 

            var parameters = new DynamicParameters();
            parameters.Add("databaseName", databaseName);

            using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

            var records = dbConnection.Execute($"CREATE DATABASE IF NOT EXISTS {databaseName}");
        }

        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();
            runner.MigrateUp();
        }
    }
}

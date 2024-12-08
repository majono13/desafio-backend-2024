using FluentMigrator;

namespace InovaBank.Infrastructure.Migrations.Versions
{
    [Migration(1, "Create table users")]
    public class Version01 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsString(36).PrimaryKey().NotNullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("email").AsString(255).NotNullable()
                .WithColumn("password").AsString(2500).NotNullable();

        }
    }
}

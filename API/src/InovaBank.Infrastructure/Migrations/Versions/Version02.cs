using FluentMigrator;

namespace InovaBank.Infrastructure.Migrations.Versions
{
    [Migration(2, "Create table account")]
    public class Version02 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("accounts")
                .WithColumn("id").AsString(36).PrimaryKey().NotNullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("tradeName").AsString(255)
                .WithColumn("name").AsString(255).Nullable()
                .WithColumn("cnpj").AsString(14).Nullable()
                .WithColumn("accountNumber").AsString(8).Nullable()
                .WithColumn("digit").AsString(1).Nullable()
                .WithColumn("agency").AsString(4).Nullable()
                .WithColumn("document").AsCustom("LONGBLOB ").Nullable()
                .WithColumn("balance").AsInt64().Nullable()
                .WithColumn("active").AsBoolean().Nullable()
                .WithColumn("userId").AsString(36).NotNullable().ForeignKey("FK_Account_User_Id", "users", "id");

        }
    }
}

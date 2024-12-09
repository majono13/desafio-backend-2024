using FluentMigrator;

namespace InovaBank.Infrastructure.Migrations.Versions
{
    [Migration(3, "Create table account")]
    public class Version03 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("transactions")
                .WithColumn("id").AsString(36).PrimaryKey().NotNullable()
                .WithColumn("createdAt").AsDateTime().NotNullable()
                .WithColumn("value").AsDecimal(10, 2).Nullable()
                .WithColumn("type").AsString(20).Nullable()
                .WithColumn("accountDestiny").AsString(36).NotNullable().ForeignKey("FK_Transaction_Account_Destiny_Id", "accounts", "id")
                .WithColumn("accountOrigin").AsString(36).Nullable().ForeignKey("FK_Transaction_Account_Origin_Id", "accounts", "id")
                .WithColumn("transactionType").AsString(20).Nullable();


        }
    }
}

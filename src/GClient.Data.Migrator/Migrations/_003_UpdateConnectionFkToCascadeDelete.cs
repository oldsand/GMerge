using System.Data;
using FluentMigrator;

namespace GClient.Data.Migrator.Migrations
{
    [Migration(3)]
    public class _003_UpdateConnectionFkToCascadeDelete : Migration
    {
        public override void Up()
        {
            Rename.Table("Connection").To("Connection_Old");
            
            Create.Table("Connection")
                .WithColumn("ResourceId").AsInt32()
                    .PrimaryKey().NotNullable()
                    .ForeignKey("Resource", "ResourceId").OnDelete(Rule.Cascade)
                .WithColumn("NodeName").AsString().NotNullable()
                .WithColumn("GalaxyName").AsString().NotNullable()
                .WithColumn("Version").AsString().Nullable();
            
            Execute.Sql("insert into Connection select * from Connection_Old");

            Delete.Table("Connection_Old");
        }

        public override void Down()
        {
            Rename.Table("Connection").To("Connection_Old");
            
            Create.Table("Connection")
                .WithColumn("ResourceId").AsInt32()
                .PrimaryKey().NotNullable()
                .ForeignKey("Resource", "ResourceId")
                .WithColumn("NodeName").AsString().NotNullable()
                .WithColumn("GalaxyName").AsString().NotNullable()
                .WithColumn("Version").AsString().Nullable();
            
            Execute.Sql("insert into Connection select * from Connection_Old");

            Delete.Table("Connection_Old");
        }
    }
}
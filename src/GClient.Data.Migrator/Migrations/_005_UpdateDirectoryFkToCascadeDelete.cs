using System.Data;
using FluentMigrator;

namespace GClient.Data.Migrator.Migrations
{
    [Migration(5)]
    public class _005_UpdateDirectoryFkToCascadeDelete : Migration
    {
        public override void Up()
        {
            Rename.Table("Directory").To("Directory_Old");
            
            Create.Table("Directory")
                .WithColumn("ResourceId").AsInt32()
                    .PrimaryKey().NotNullable()
                    .ForeignKey("Resource", "ResourceId").OnDelete(Rule.Cascade)
                .WithColumn("DirectoryName").AsString().NotNullable();
            
            Execute.Sql("insert into Directory select * from Directory_Old");

            Delete.Table("Directory_Old");
        }

        public override void Down()
        {
            Rename.Table("Directory").To("Directory_Old");
            
            Create.Table("Directory")
                .WithColumn("ResourceId").AsInt32()
                .PrimaryKey().NotNullable()
                .ForeignKey("Resource", "ResourceId")
                .WithColumn("DirectoryName").AsString().NotNullable();
            
            Execute.Sql("insert into Directory select * from Directory_Old");

            Delete.Table("Directory_Old");
        }
    }
}
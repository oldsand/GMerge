using FluentMigrator;

namespace GalaxyMerge.Client.Data.MigrationRunner.Migrations
{
    [Migration(20210610122600)]
    public class UpdateResourceTypeConversion : Migration 
    {
        public override void Up()
        {
            Delete.Table("Resource");
            
            Create.Table("Resource")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("ResourceName").AsString().NotNullable()
                .WithColumn("ResourceType").AsString().NotNullable()
                .WithColumn("NodeName").AsString()
                .WithColumn("GalaxyName").AsString()
                .WithColumn("FileName").AsString()
                .WithColumn("AddedOn").AsString();
        }

        public override void Down()
        {
            Delete.Table("Resource");
            
            Create.Table("Resource")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("ResourceName").AsString().NotNullable()
                .WithColumn("ResourceType").AsInt32().NotNullable()
                .WithColumn("NodeName").AsString()
                .WithColumn("GalaxyName").AsString()
                .WithColumn("FileName").AsString()
                .WithColumn("AddedOn").AsString();
        }
    }
}
using FluentMigrator;

namespace GalaxyMerge.Client.Data.Migrator.Migrations
{
    [Migration(2)]
    public class _002_MakeResourceNameUnique : Migration
    {
        public override void Up()
        {
            Create.Index().OnTable("Resource")
                .OnColumn("ResourceName").Ascending()
                .WithOptions().Unique();
        }

        public override void Down()
        {
            Delete.Index().OnTable("Resource")
                .OnColumn("ResourceName");
        }
    }
}
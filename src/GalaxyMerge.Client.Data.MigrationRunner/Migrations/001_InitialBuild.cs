using FluentMigrator;

namespace GalaxyMerge.Client.Data.MigrationRunner.Migrations
{
    [Migration(1)]
    public class InitialBuild : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("GalaxyMerge.Client.Data.MigrationRunner.Scripts.001_InitialBuildUp.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("GalaxyMerge.Client.Data.MigrationRunner.Scripts.001_InitialBuildDown.sql");
        }
    }
}
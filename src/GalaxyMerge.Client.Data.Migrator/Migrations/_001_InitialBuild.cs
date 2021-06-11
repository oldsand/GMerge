using FluentMigrator;

namespace GalaxyMerge.Client.Data.Migrator.Migrations
{
    [Migration(1)]
    public class _001_InitialBuild : Migration
    {
        public override void Up()
        {
            Create.Table("Log")
                .WithColumn("LogId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Logged").AsString().NotNullable()
                .WithColumn("LevelId").AsInt32().NotNullable()
                .WithColumn("Level").AsString().NotNullable()
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("Logger").AsString().Nullable()
                .WithColumn("Properties").AsString().Nullable()
                .WithColumn("Callsite").AsString().Nullable()
                .WithColumn("FileName").AsString().Nullable()
                .WithColumn("LineNumber").AsString().Nullable()
                .WithColumn("Stacktrace").AsString().Nullable()
                .WithColumn("MachineName").AsString().Nullable()
                .WithColumn("Identity").AsString().Nullable()
                .WithColumn("Exception").AsString().Nullable();

            Create.Table("Resource")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("ResourceName").AsString().NotNullable()
                .WithColumn("ResourceDescription").AsString().Nullable()
                .WithColumn("ResourceType").AsString().NotNullable()
                .WithColumn("AddedOn").AsString().NotNullable()
                .WithColumn("AddedBy").AsString().NotNullable();

            Create.Table("Connection")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable().ForeignKey("Resource", "ResourceId")
                .WithColumn("NodeName").AsString().NotNullable()
                .WithColumn("GalaxyName").AsString().NotNullable()
                .WithColumn("Version").AsString().Nullable();
            
            Create.Table("Archive")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable().ForeignKey("Resource", "ResourceId")
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("OriginatingMachine").AsString().Nullable()
                .WithColumn("OriginatingGalaxy").AsString().Nullable()
                .WithColumn("OriginatingVersion").AsString().Nullable();

            Create.Table("Directory")
                .WithColumn("ResourceId").AsInt32().PrimaryKey().NotNullable().ForeignKey("Resource", "ResourceId")
                .WithColumn("DirectoryName").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Log");
            Delete.Table("Resource");
            Delete.Table("Connection");
            Delete.Table("Archive");
            Delete.Table("Directory");
        }
    }
}
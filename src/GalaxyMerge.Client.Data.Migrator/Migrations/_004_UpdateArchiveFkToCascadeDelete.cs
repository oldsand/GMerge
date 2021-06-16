using FluentMigrator;
using System.Data;

namespace GalaxyMerge.Client.Data.Migrator.Migrations
{
    [Migration(4)]
    public class _004_UpdateArchiveFkToCascadeDelete : Migration
    {
        public override void Up()
        {
            Rename.Table("Archive").To("Archive_Old");
            
            Create.Table("Archive")
                .WithColumn("ResourceId").AsInt32()
                    .PrimaryKey().NotNullable()
                    .ForeignKey("Resource", "ResourceId").OnDelete(Rule.Cascade)
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("OriginatingMachine").AsString().Nullable()
                .WithColumn("OriginatingGalaxy").AsString().Nullable()
                .WithColumn("OriginatingVersion").AsString().Nullable();
            
            Execute.Sql("insert into Archive select * from Archive_Old");

            Delete.Table("Archive_Old");
        }

        public override void Down()
        {
            Rename.Table("Archive").To("Archive_Old");
            
            Create.Table("Archive")
                .WithColumn("ResourceId").AsInt32()
                .PrimaryKey().NotNullable()
                .ForeignKey("Resource", "ResourceId")
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("OriginatingMachine").AsString().Nullable()
                .WithColumn("OriginatingGalaxy").AsString().Nullable()
                .WithColumn("OriginatingVersion").AsString().Nullable();
            
            Execute.Sql("insert into Archive select * from Archive_Old");

            Delete.Table("Archive_Old");
        }
    }
}
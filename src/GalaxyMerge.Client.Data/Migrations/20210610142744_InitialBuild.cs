using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GalaxyMerge.Client.Data.Migrations
{
    public partial class InitialBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Logged = table.Column<DateTime>(nullable: false),
                    LevelId = table.Column<int>(nullable: false),
                    Level = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Logger = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    Callsite = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    LineNumber = table.Column<int>(nullable: false),
                    Stacktrace = table.Column<string>(nullable: true),
                    MachineName = table.Column<string>(nullable: true),
                    Identity = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    ResourceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResourceName = table.Column<string>(nullable: false),
                    ResourceType = table.Column<int>(nullable: false),
                    NodeName = table.Column<string>(nullable: true),
                    GalaxyName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.ResourceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Resource");
        }
    }
}

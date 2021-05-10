using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GalaxyMerge.Archive.Migrations
{
    public partial class InitialBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchiveObject",
                columns: table => new
                {
                    ObjectId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveObject", x => x.ObjectId);
                });

            migrationBuilder.CreateTable(
                name: "EventSetting",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperationId = table.Column<int>(nullable: false),
                    OperationName = table.Column<string>(nullable: false),
                    OperationType = table.Column<string>(nullable: false),
                    IsArchiveTrigger = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSetting", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "GalaxyInfo",
                columns: table => new
                {
                    GalaxyName = table.Column<string>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: true),
                    CdiVersion = table.Column<string>(nullable: true),
                    IsaVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalaxyInfo", x => x.GalaxyName);
                });

            migrationBuilder.CreateTable(
                name: "InclusionSetting",
                columns: table => new
                {
                    InclusionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemplateId = table.Column<int>(nullable: false),
                    TemplateName = table.Column<string>(nullable: false),
                    InclusionOption = table.Column<string>(nullable: false),
                    IncludesInstances = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InclusionSetting", x => x.InclusionId);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveEntry",
                columns: table => new
                {
                    EntryId = table.Column<Guid>(nullable: false),
                    ObjectId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    ArchivedOn = table.Column<DateTime>(nullable: false),
                    OriginalSize = table.Column<long>(nullable: false),
                    CompressedSize = table.Column<long>(nullable: false),
                    CompressedData = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveEntry", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_ArchiveEntry_ArchiveObject_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "ArchiveObject",
                        principalColumn: "ObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveEntry_ObjectId",
                table: "ArchiveEntry",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSetting_OperationId",
                table: "EventSetting",
                column: "OperationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventSetting_OperationName",
                table: "EventSetting",
                column: "OperationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InclusionSetting_TemplateId",
                table: "InclusionSetting",
                column: "TemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InclusionSetting_TemplateName",
                table: "InclusionSetting",
                column: "TemplateName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchiveEntry");

            migrationBuilder.DropTable(
                name: "EventSetting");

            migrationBuilder.DropTable(
                name: "GalaxyInfo");

            migrationBuilder.DropTable(
                name: "InclusionSetting");

            migrationBuilder.DropTable(
                name: "ArchiveObject");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Red.Infrastructure.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ProductCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nsuids = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pictures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayModes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EshopUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Developer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgeRating = table.Column<int>(type: "int", nullable: true),
                    DownloadSize = table.Column<int>(type: "int", nullable: true),
                    MinPlayers = table.Column<int>(type: "int", nullable: true),
                    MaxPlayers = table.Column<int>(type: "int", nullable: true),
                    Coop = table.Column<bool>(type: "bit", nullable: true),
                    DemoAvailable = table.Column<bool>(type: "bit", nullable: true),
                    SupportsCloudSave = table.Column<bool>(type: "bit", nullable: true),
                    RemovedFromEshop = table.Column<bool>(type: "bit", nullable: true),
                    VoucherPossible = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchGame_ProductCodeRegion", x => new { x.ProductCode, x.Region });
                });

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameProductCode",
                table: "Games",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameSlug",
                table: "Games",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameTitle",
                table: "Games",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}

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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegularPrice = table.Column<float>(type: "real", nullable: true),
                    AllTimeLow = table.Column<float>(type: "real", nullable: true),
                    AllTimeHigh = table.Column<float>(type: "real", nullable: true),
                    PriceHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nsuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Developer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeRating = table.Column<int>(type: "int", nullable: true),
                    DownloadSize = table.Column<int>(type: "int", nullable: true),
                    MinPlayers = table.Column<int>(type: "int", nullable: true),
                    MaxPlayers = table.Column<int>(type: "int", nullable: true),
                    Coop = table.Column<bool>(type: "bit", nullable: true),
                    DemoAvailable = table.Column<bool>(type: "bit", nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayModes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportsCloudSave = table.Column<bool>(type: "bit", nullable: true),
                    RemovedFromEshop = table.Column<bool>(type: "bit", nullable: true),
                    VoucherPossible = table.Column<bool>(type: "bit", nullable: true),
                    Screenshots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Popularity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchGameId", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameSlug",
                table: "Games",
                column: "Slug",
                unique: true);

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

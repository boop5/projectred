using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class BetterIndexesBruh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Trend_GameId_Country_Created",
                schema: "Nintendo",
                table: "Trend",
                columns: new[] { "GameId", "Country", "Created" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trend_GameId_Country_Created",
                schema: "Nintendo",
                table: "Trend");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class RemoveRegularPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegularPrice",
                schema: "Nintendo");

            migrationBuilder.DropColumn(
                name: "Discount",
                schema: "Nintendo",
                table: "Trend");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Discount",
                schema: "Nintendo",
                table: "Trend",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "RegularPrice",
                schema: "Nintendo",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegularPrice", x => new { x.GameId, x.Country });
                    table.ForeignKey(
                        name: "FK_RegularPrice_Game_GameId",
                        column: x => x.GameId,
                        principalSchema: "Nintendo",
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}

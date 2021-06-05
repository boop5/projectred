using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class RemoveStupidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategory_Category_CategoryId",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Nintendo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategory",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.DropIndex(
                name: "IX_GameCategory_CategoryId",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "Nintendo",
                table: "GameCategory",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategory",
                schema: "Nintendo",
                table: "GameCategory",
                columns: new[] { "GameId", "Category" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategory",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.DropColumn(
                name: "Category",
                schema: "Nintendo",
                table: "GameCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "Nintendo",
                table: "GameCategory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategory",
                schema: "Nintendo",
                table: "GameCategory",
                columns: new[] { "GameId", "CategoryId" });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Nintendo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategory_CategoryId",
                schema: "Nintendo",
                table: "GameCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                schema: "Nintendo",
                table: "Category",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategory_Category_CategoryId",
                schema: "Nintendo",
                table: "GameCategory",
                column: "CategoryId",
                principalSchema: "Nintendo",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class RemoveFsIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FsId",
                schema: "Nintendo",
                table: "Game");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FsId",
                schema: "Nintendo",
                table: "Game",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

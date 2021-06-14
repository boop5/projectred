using Microsoft.EntityFrameworkCore.Migrations;

namespace Red.Infrastructure.Persistence.Migrations
{
    public partial class AddContentRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeRating",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "ContentRating",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentRating",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "AgeRating",
                table: "Games",
                type: "int",
                nullable: true);
        }
    }
}

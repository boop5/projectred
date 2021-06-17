using Microsoft.EntityFrameworkCore.Migrations;

namespace Red.Infrastructure.Persistence.Migrations
{
    public partial class AddMoreCountryDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "{}");
        }
    }
}

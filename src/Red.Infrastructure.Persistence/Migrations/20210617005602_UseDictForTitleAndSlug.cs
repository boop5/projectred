using Microsoft.EntityFrameworkCore.Migrations;

namespace Red.Infrastructure.Persistence.Migrations
{
    public partial class UseDictForTitleAndSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FsId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Categories",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameFsId",
                table: "Games",
                column: "FsId",
                unique: true,
                filter: "[FsId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SwitchGameFsId",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "FsId",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Categories",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "[]");
        }
    }
}

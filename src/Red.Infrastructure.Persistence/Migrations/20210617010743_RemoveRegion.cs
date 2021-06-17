using Microsoft.EntityFrameworkCore.Migrations;

namespace Red.Infrastructure.Persistence.Migrations
{
    public partial class RemoveRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SwitchGame_ProductCodeRegion",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_SwitchGameFsId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "FsId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SwitchGame_FsID",
                table: "Games",
                column: "FsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SwitchGame_FsID",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "{}");

            migrationBuilder.AlterColumn<string>(
                name: "FsId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SwitchGame_ProductCodeRegion",
                table: "Games",
                columns: new[] { "ProductCode", "Region" });

            migrationBuilder.CreateIndex(
                name: "IX_SwitchGameFsId",
                table: "Games",
                column: "FsId",
                unique: true,
                filter: "[FsId] IS NOT NULL");
        }
    }
}

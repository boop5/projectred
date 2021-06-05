using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class UseNsuIdClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NsUid_EU",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NsUid_JP",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NsUid_US",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.AddColumn<long>(
                name: "NsUid_EUId",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NsUid_JPId",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NsUid_USId",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NsuId",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NsuId", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_NsUid_EUId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_EUId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_NsUid_JPId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_JPId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_NsUid_USId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_USId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_NsuId_NsUid_EUId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_EUId",
                principalTable: "NsuId",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_NsuId_NsUid_JPId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_JPId",
                principalTable: "NsuId",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_NsuId_NsUid_USId",
                schema: "Nintendo",
                table: "Game",
                column: "NsUid_USId",
                principalTable: "NsuId",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_NsuId_NsUid_EUId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_NsuId_NsUid_JPId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_NsuId_NsUid_USId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropTable(
                name: "NsuId");

            migrationBuilder.DropIndex(
                name: "IX_Game_NsUid_EUId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_NsUid_JPId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_NsUid_USId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NsUid_EUId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NsUid_JPId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "NsUid_USId",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.AddColumn<long>(
                name: "NsUid_EU",
                schema: "Nintendo",
                table: "Game",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NsUid_JP",
                schema: "Nintendo",
                table: "Game",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NsUid_US",
                schema: "Nintendo",
                table: "Game",
                type: "bigint",
                nullable: true);
        }
    }
}

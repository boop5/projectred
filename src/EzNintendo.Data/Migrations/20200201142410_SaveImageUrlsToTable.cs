using Microsoft.EntityFrameworkCore.Migrations;

namespace EzNintendo.Data.Migrations
{
    public partial class SaveImageUrlsToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_giftfinder_detailpage",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_giftfinder_wishlist",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_h2x1",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_sq",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_wishlist_email_banner640w",
                schema: "Nintendo",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_wishlist_email_square",
                schema: "Nintendo",
                table: "Game",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_giftfinder_detailpage",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_giftfinder_wishlist",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_h2x1",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_sq",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_wishlist_email_banner640w",
                schema: "Nintendo",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "image_wishlist_email_square",
                schema: "Nintendo",
                table: "Game");
        }
    }
}

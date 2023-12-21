using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemWypozyczalniGier.Migrations
{
    public partial class GamesPhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainPhotoName",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Photo1Name",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Photo2Name",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo3Name",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo4Name",
                table: "Games",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoName",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Photo1Name",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Photo2Name",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Photo3Name",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Photo4Name",
                table: "Games");
        }
    }
}

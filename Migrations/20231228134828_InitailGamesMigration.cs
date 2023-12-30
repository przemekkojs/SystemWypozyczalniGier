using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemWypozyczalniGier.Migrations
{
    public partial class InitailGamesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "Accessibility", "Description", "Discount", "MainPhotoName", "MaxQuantity", "Pegi", "Photo1Name", "Photo2Name", "Photo3Name", "Photo4Name", "Price", "PublisherId", "QuantityInStock", "Title" },
                values: new object[] { 5, 0, "Komputerowa gra logiczna stworzona przez Aleksieja Pażytnowa i jego współpracowników, Dimitrija Pawłowskiego i Wadima Geriasimowa. Pojawiła się na rynku po raz pierwszy 6 czerwca 1984 roku w Związku Radzieckim.", 0.0, "5_0.png", 5, 0, "5_1.png", null, null, null, 19.989999999999998, 1, 5, "Tetris" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 5);
        }
    }
}

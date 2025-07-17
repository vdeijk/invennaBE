using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeographicalData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Openbareruimte = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Huisnummer = table.Column<int>(type: "INTEGER", nullable: false),
                    Huisletter = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    Huisnummertoevoeging = table.Column<int>(type: "INTEGER", nullable: true),
                    Postcode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Woonplaats = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Gemeente = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Provincie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Nummeraanduiding = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Verblijfsobjectgebruiksdoel = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Oppervlakteverblijfsobject = table.Column<int>(type: "INTEGER", nullable: false),
                    Verblijfsobjectstatus = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ObjectId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ObjectType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nevenadres = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Pandid = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Pandstatus = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Pandbouwjaar = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Lon = table.Column<double>(type: "decimal(18,6)", nullable: false),
                    Lat = table.Column<double>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeographicalData", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GeographicalData",
                columns: new[] { "Id", "Gemeente", "Huisletter", "Huisnummer", "Huisnummertoevoeging", "Lat", "Lon", "Nevenadres", "Nummeraanduiding", "ObjectId", "ObjectType", "Openbareruimte", "Oppervlakteverblijfsobject", "Pandbouwjaar", "Pandid", "Pandstatus", "Postcode", "Provincie", "Verblijfsobjectgebruiksdoel", "Verblijfsobjectstatus", "Woonplaats", "X", "Y" },
                values: new object[,]
                {
                    { 1, "Amsterdam", null, 1, null, 52.370215999999999, 4.895168, null, "1", "OBJ1", "Pand", "Hoofdstraat", 100, 1990, "PAND1", "Bestaand", "1234AB", "Noord-Holland", "Wonen", "Actief", "Amsterdam", 12345, 67890 },
                    { 2, "Rotterdam", "A", 2, null, 51.922499999999999, 4.4791699999999999, null, "2A", "OBJ2", "Pand", "Dorpsstraat", 80, 1980, "PAND2", "Bestaand", "5678CD", "Zuid-Holland", "Wonen", "Actief", "Rotterdam", 54321, 98765 },
                    { 3, "Utrecht", null, 3, 1, 52.090739999999997, 5.1214199999999996, null, "3-1", "OBJ3", "Pand", "Kerkstraat", 120, 2000, "PAND3", "Bestaand", "9012EF", "Utrecht", "Wonen", "Actief", "Utrecht", 11111, 22222 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalData_Address",
                table: "GeographicalData",
                columns: new[] { "Openbareruimte", "Huisnummer", "Postcode" });

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalData_Gemeente",
                table: "GeographicalData",
                column: "Gemeente");

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalData_Postcode",
                table: "GeographicalData",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_GeographicalData_Woonplaats",
                table: "GeographicalData",
                column: "Woonplaats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeographicalData");
        }
    }
}

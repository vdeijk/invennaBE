using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE.Migrations
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
                    { 1, "Amsterdam", "A", 1, null, 52.367600000000003, 4.9040999999999997, null, "0363010000000001", "NL.IMBAG.Verblijfsobject.0363010000000001", "Verblijfsobject", "Kerkstraat", 75, 1950, "NL.IMBAG.Pand.0363100000000001", "Pand in gebruik", "1234AB", "Noord-Holland", "woonfunctie", "Verblijfsobject in gebruik", "Amsterdam", 121000, 487000 },
                    { 2, "Utrecht", null, 25, null, 52.090699999999998, 5.1214000000000004, null, "0344010000000002", "NL.IMBAG.Verblijfsobject.0344010000000002", "Verblijfsobject", "Hoofdstraat", 120, 1975, "NL.IMBAG.Pand.0344100000000002", "Pand in gebruik", "3511AB", "Utrecht", "winkelfunctie", "Verblijfsobject in gebruik", "Utrecht", 155000, 463000 },
                    { 3, "Nijmegen", "B", 8, 2, 51.812600000000003, 5.8520000000000003, null, "0268010000000003", "NL.IMBAG.Verblijfsobject.0268010000000003", "Verblijfsobject", "Dorpsplein", 200, 2000, "NL.IMBAG.Pand.0268100000000003", "Pand in gebruik", "6543CD", "Gelderland", "kantoorfunctie", "Verblijfsobject in gebruik", "Nijmegen", 190000, 426000 }
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

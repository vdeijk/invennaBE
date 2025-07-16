using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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

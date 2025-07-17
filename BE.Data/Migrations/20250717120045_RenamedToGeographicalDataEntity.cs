using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamedToGeographicalDataEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Y",
                table: "GeographicalData",
                newName: "y");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "GeographicalData",
                newName: "x");

            migrationBuilder.RenameColumn(
                name: "Woonplaats",
                table: "GeographicalData",
                newName: "woonplaats");

            migrationBuilder.RenameColumn(
                name: "Verblijfsobjectstatus",
                table: "GeographicalData",
                newName: "verblijfsobjectstatus");

            migrationBuilder.RenameColumn(
                name: "Verblijfsobjectgebruiksdoel",
                table: "GeographicalData",
                newName: "verblijfsobjectgebruiksdoel");

            migrationBuilder.RenameColumn(
                name: "Provincie",
                table: "GeographicalData",
                newName: "provincie");

            migrationBuilder.RenameColumn(
                name: "Postcode",
                table: "GeographicalData",
                newName: "postcode");

            migrationBuilder.RenameColumn(
                name: "Pandstatus",
                table: "GeographicalData",
                newName: "pandstatus");

            migrationBuilder.RenameColumn(
                name: "Pandid",
                table: "GeographicalData",
                newName: "pandid");

            migrationBuilder.RenameColumn(
                name: "Pandbouwjaar",
                table: "GeographicalData",
                newName: "pandbouwjaar");

            migrationBuilder.RenameColumn(
                name: "Oppervlakteverblijfsobject",
                table: "GeographicalData",
                newName: "oppervlakteverblijfsobject");

            migrationBuilder.RenameColumn(
                name: "Openbareruimte",
                table: "GeographicalData",
                newName: "openbareruimte");

            migrationBuilder.RenameColumn(
                name: "ObjectType",
                table: "GeographicalData",
                newName: "objecttype");

            migrationBuilder.RenameColumn(
                name: "ObjectId",
                table: "GeographicalData",
                newName: "objectid");

            migrationBuilder.RenameColumn(
                name: "Nummeraanduiding",
                table: "GeographicalData",
                newName: "nummeraanduiding");

            migrationBuilder.RenameColumn(
                name: "Nevenadres",
                table: "GeographicalData",
                newName: "nevenadres");

            migrationBuilder.RenameColumn(
                name: "Lon",
                table: "GeographicalData",
                newName: "lon");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "GeographicalData",
                newName: "lat");

            migrationBuilder.RenameColumn(
                name: "Huisnummertoevoeging",
                table: "GeographicalData",
                newName: "huisnummertoevoeging");

            migrationBuilder.RenameColumn(
                name: "Huisnummer",
                table: "GeographicalData",
                newName: "huisnummer");

            migrationBuilder.RenameColumn(
                name: "Huisletter",
                table: "GeographicalData",
                newName: "huisletter");

            migrationBuilder.RenameColumn(
                name: "Gemeente",
                table: "GeographicalData",
                newName: "gemeente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "y",
                table: "GeographicalData",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "x",
                table: "GeographicalData",
                newName: "X");

            migrationBuilder.RenameColumn(
                name: "woonplaats",
                table: "GeographicalData",
                newName: "Woonplaats");

            migrationBuilder.RenameColumn(
                name: "verblijfsobjectstatus",
                table: "GeographicalData",
                newName: "Verblijfsobjectstatus");

            migrationBuilder.RenameColumn(
                name: "verblijfsobjectgebruiksdoel",
                table: "GeographicalData",
                newName: "Verblijfsobjectgebruiksdoel");

            migrationBuilder.RenameColumn(
                name: "provincie",
                table: "GeographicalData",
                newName: "Provincie");

            migrationBuilder.RenameColumn(
                name: "postcode",
                table: "GeographicalData",
                newName: "Postcode");

            migrationBuilder.RenameColumn(
                name: "pandstatus",
                table: "GeographicalData",
                newName: "Pandstatus");

            migrationBuilder.RenameColumn(
                name: "pandid",
                table: "GeographicalData",
                newName: "Pandid");

            migrationBuilder.RenameColumn(
                name: "pandbouwjaar",
                table: "GeographicalData",
                newName: "Pandbouwjaar");

            migrationBuilder.RenameColumn(
                name: "oppervlakteverblijfsobject",
                table: "GeographicalData",
                newName: "Oppervlakteverblijfsobject");

            migrationBuilder.RenameColumn(
                name: "openbareruimte",
                table: "GeographicalData",
                newName: "Openbareruimte");

            migrationBuilder.RenameColumn(
                name: "objecttype",
                table: "GeographicalData",
                newName: "ObjectType");

            migrationBuilder.RenameColumn(
                name: "objectid",
                table: "GeographicalData",
                newName: "ObjectId");

            migrationBuilder.RenameColumn(
                name: "nummeraanduiding",
                table: "GeographicalData",
                newName: "Nummeraanduiding");

            migrationBuilder.RenameColumn(
                name: "nevenadres",
                table: "GeographicalData",
                newName: "Nevenadres");

            migrationBuilder.RenameColumn(
                name: "lon",
                table: "GeographicalData",
                newName: "Lon");

            migrationBuilder.RenameColumn(
                name: "lat",
                table: "GeographicalData",
                newName: "Lat");

            migrationBuilder.RenameColumn(
                name: "huisnummertoevoeging",
                table: "GeographicalData",
                newName: "Huisnummertoevoeging");

            migrationBuilder.RenameColumn(
                name: "huisnummer",
                table: "GeographicalData",
                newName: "Huisnummer");

            migrationBuilder.RenameColumn(
                name: "huisletter",
                table: "GeographicalData",
                newName: "Huisletter");

            migrationBuilder.RenameColumn(
                name: "gemeente",
                table: "GeographicalData",
                newName: "Gemeente");
        }
    }
}

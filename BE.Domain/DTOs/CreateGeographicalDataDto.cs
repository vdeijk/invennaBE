using System.ComponentModel.DataAnnotations;

namespace BE.Domain.DTOs
{
    public class CreateGeographicalDataDto
    {
        [Required(ErrorMessage = "Openbare ruimte is verplicht")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Openbare ruimte moet tussen 1 en 200 karakters zijn")]
        public string Openbareruimte { get; set; } = string.Empty;

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Range(1, 99999, ErrorMessage = "Huisnummer moet tussen 1 en 99999 zijn")]
        public int Huisnummer { get; set; }

        [StringLength(1, ErrorMessage = "Huisletter mag maximaal 1 karakter zijn")]
        [RegularExpression(@"^[A-Za-z]?$", ErrorMessage = "Huisletter moet een letter zijn")]
        public string? Huisletter { get; set; }

        [Range(1, 9999, ErrorMessage = "Huisnummertoevoeging moet tussen 1 en 9999 zijn")]
        public int? Huisnummertoevoeging { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Postcode moet tussen 6 en 10 karakters zijn")]
        [RegularExpression(@"^[1-9][0-9]{3}\s?[A-Za-z]{2}$", ErrorMessage = "Postcode moet een geldige Nederlandse postcode zijn (bijv. 1234AB of 1234 AB)")]
        public string Postcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Woonplaats is verplicht")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Woonplaats moet tussen 1 en 100 karakters zijn")]
        public string Woonplaats { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Gemeente mag maximaal 100 karakters zijn")]
        public string Gemeente { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Provincie mag maximaal 100 karakters zijn")]
        public string Provincie { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Nummeraanduiding mag maximaal 100 karakters zijn")]
        public string Nummeraanduiding { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Verblijfsobject gebruiksdoel mag maximaal 100 karakters zijn")]
        public string Verblijfsobjectgebruiksdoel { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Oppervlakte verblijfsobject moet groter dan 0 zijn")]
        public int Oppervlakteverblijfsobject { get; set; }

        [StringLength(100, ErrorMessage = "Verblijfsobject status mag maximaal 100 karakters zijn")]
        public string Verblijfsobjectstatus { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Object ID mag maximaal 100 karakters zijn")]
        public string ObjectId { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Object type mag maximaal 100 karakters zijn")]
        public string ObjectType { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Nevenadres mag maximaal 200 karakters zijn")]
        public string? Nevenadres { get; set; }

        [StringLength(100, ErrorMessage = "Pand ID mag maximaal 100 karakters zijn")]
        public string Pandid { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Pand status mag maximaal 100 karakters zijn")]
        public string Pandstatus { get; set; } = string.Empty;

        [Range(1800, 2100, ErrorMessage = "Pand bouwjaar moet tussen 1800 en 2100 zijn")]
        public int Pandbouwjaar { get; set; }

        [Range(0, 300000, ErrorMessage = "X-coördinaat moet tussen 0 en 300000 zijn (RD-coördinaten)")]
        public int X { get; set; }

        [Range(300000, 650000, ErrorMessage = "Y-coördinaat moet tussen 300000 en 650000 zijn (RD-coördinaten)")]
        public int Y { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude moet tussen -180 en 180 graden zijn")]
        public double Lon { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude moet tussen -90 en 90 graden zijn")]
        public double Lat { get; set; }
    }
}

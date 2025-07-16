namespace BE.Domain.DTOs
{
    public class GeographicalDataDto
    {
        public int Id { get; set; }
        public string Openbareruimte { get; set; } = string.Empty;
        public int Huisnummer { get; set; }
        public string? Huisletter { get; set; }
        public int? Huisnummertoevoeging { get; set; }
        public string Postcode { get; set; } = string.Empty;
        public string Woonplaats { get; set; } = string.Empty;
        public string Gemeente { get; set; } = string.Empty;
        public string Provincie { get; set; } = string.Empty;
        public string Nummeraanduiding { get; set; } = string.Empty;
        public string Verblijfsobjectgebruiksdoel { get; set; } = string.Empty;
        public int Oppervlakteverblijfsobject { get; set; }
        public string Verblijfsobjectstatus { get; set; } = string.Empty;
        public string ObjectId { get; set; } = string.Empty;
        public string ObjectType { get; set; } = string.Empty;
        public string? Nevenadres { get; set; }
        public string Pandid { get; set; } = string.Empty;
        public string Pandstatus { get; set; } = string.Empty;
        public int Pandbouwjaar { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
}

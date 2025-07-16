using System.ComponentModel.DataAnnotations;

namespace BE.Domain.Models
{
    /// <summary>
    /// Entity model for geographical data (database table).
    /// </summary>
    public class GeographicalData
    {
        /// <summary>Unique identifier.</summary>
        public int Id { get; set; }

        /// <summary>Street name or public space.</summary>
        [Required, StringLength(200)]
        public string Openbareruimte { get; set; } = string.Empty;

        /// <summary>House number.</summary>
        [Required]
        public int Huisnummer { get; set; }

        /// <summary>House letter (optional).</summary>
        [StringLength(1)]
        public string? Huisletter { get; set; }

        /// <summary>House number addition (optional).</summary>
        public int? Huisnummertoevoeging { get; set; }

        /// <summary>Postal code.</summary>
        [Required, StringLength(10)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>City or town.</summary>
        [Required, StringLength(100)]
        public string Woonplaats { get; set; } = string.Empty;

        /// <summary>Municipality.</summary>
        [StringLength(100)]
        public string Gemeente { get; set; } = string.Empty;

        /// <summary>Province.</summary>
        [StringLength(100)]
        public string Provincie { get; set; } = string.Empty;

        /// <summary>Address indication number.</summary>
        [StringLength(100)]
        public string Nummeraanduiding { get; set; } = string.Empty;

        /// <summary>Usage purpose of the residence.</summary>
        [StringLength(100)]
        public string Verblijfsobjectgebruiksdoel { get; set; } = string.Empty;

        /// <summary>Surface area of the residence.</summary>
        public int Oppervlakteverblijfsobject { get; set; }

        /// <summary>Status of the residence.</summary>
        [StringLength(100)]
        public string Verblijfsobjectstatus { get; set; } = string.Empty;

        /// <summary>Object ID.</summary>
        [StringLength(100)]
        public string ObjectId { get; set; } = string.Empty;

        /// <summary>Object type.</summary>
        [StringLength(100)]
        public string ObjectType { get; set; } = string.Empty;

        /// <summary>Secondary address (optional).</summary>
        [StringLength(100)]
        public string? Nevenadres { get; set; }

        /// <summary>Building ID.</summary>
        [StringLength(100)]
        public string Pandid { get; set; } = string.Empty;

        /// <summary>Building status.</summary>
        [StringLength(100)]
        public string Pandstatus { get; set; } = string.Empty;

        /// <summary>Year of construction.</summary>
        public int Pandbouwjaar { get; set; }

        /// <summary>X coordinate.</summary>
        public int X { get; set; }

        /// <summary>Y coordinate.</summary>
        public int Y { get; set; }

        /// <summary>Longitude.</summary>
        public double Lon { get; set; }

        /// <summary>Latitude.</summary>
        public double Lat { get; set; }
        // ...rest of the model code remains unchanged...
    }
}

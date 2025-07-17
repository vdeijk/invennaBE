using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Domain.Entities
{
    /// <summary>
    /// Entity model for geographical data (database table).
    /// </summary>
    [Table("GeographicalData")]
    public class GeographicalDataEntity
    {
        /// <summary>Unique identifier.</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Street name or public space.</summary>
        [Required, StringLength(200)]
        [Column("openbareruimte")]
        public string Openbareruimte { get; set; } = string.Empty;

        /// <summary>House number.</summary>
        [Required]
        [Column("huisnummer")]
        public int Huisnummer { get; set; }

        /// <summary>House letter (optional).</summary>
        [StringLength(1)]
        [Column("huisletter")]
        public string? Huisletter { get; set; }

        /// <summary>House number addition (optional).</summary>
        [Column("huisnummertoevoeging")]
        public int? Huisnummertoevoeging { get; set; }

        /// <summary>Postal code.</summary>
        [Required, StringLength(10)]
        [Column("postcode")]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>City or town.</summary>
        [Required, StringLength(100)]
        [Column("woonplaats")]
        public string Woonplaats { get; set; } = string.Empty;

        /// <summary>Municipality.</summary>
        [StringLength(100)]
        [Column("gemeente")]
        public string Gemeente { get; set; } = string.Empty;

        /// <summary>Province.</summary>
        [StringLength(100)]
        [Column("provincie")]
        public string Provincie { get; set; } = string.Empty;

        /// <summary>Address indication number.</summary>
        [StringLength(100)]
        [Column("nummeraanduiding")]
        public string Nummeraanduiding { get; set; } = string.Empty;

        /// <summary>Usage purpose of the residence.</summary>
        [StringLength(100)]
        [Column("verblijfsobjectgebruiksdoel")]
        public string Verblijfsobjectgebruiksdoel { get; set; } = string.Empty;

        /// <summary>Surface area of the residence.</summary>
        [Column("oppervlakteverblijfsobject")]
        public int Oppervlakteverblijfsobject { get; set; }

        /// <summary>Status of the residence.</summary>
        [StringLength(100)]
        [Column("verblijfsobjectstatus")]
        public string Verblijfsobjectstatus { get; set; } = string.Empty;

        /// <summary>Object ID.</summary>
        [StringLength(100)]
        [Column("objectid")]
        public string ObjectId { get; set; } = string.Empty;

        /// <summary>Object type.</summary>
        [StringLength(100)]
        [Column("objecttype")]
        public string ObjectType { get; set; } = string.Empty;

        /// <summary>Secondary address (optional).</summary>
        [StringLength(100)]
        [Column("nevenadres")]
        public string? Nevenadres { get; set; }

        /// <summary>Building ID.</summary>
        [StringLength(100)]
        [Column("pandid")]
        public string Pandid { get; set; } = string.Empty;

        /// <summary>Building status.</summary>
        [StringLength(100)]
        [Column("pandstatus")]
        public string Pandstatus { get; set; } = string.Empty;

        /// <summary>Year of construction.</summary>
        [Column("pandbouwjaar")]
        public int Pandbouwjaar { get; set; }

        /// <summary>X coordinate.</summary>
        [Column("x")]
        public int X { get; set; }

        /// <summary>Y coordinate.</summary>
        [Column("y")]
        public int Y { get; set; }

        /// <summary>Longitude.</summary>
        [Column("lon")]
        public double Lon { get; set; }

        /// <summary>Latitude.</summary>
        [Column("lat")]
        public double Lat { get; set; }
    }
}

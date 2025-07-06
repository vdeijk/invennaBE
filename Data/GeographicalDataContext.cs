using Microsoft.EntityFrameworkCore;
using BE.Models;

namespace BE.Data
{
    public class GeographicalDataContext : DbContext
    {
        public GeographicalDataContext(DbContextOptions<GeographicalDataContext> options)
            : base(options)
        {
        }

        public DbSet<GeographicalData> GeographicalData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the GeographicalData entity
            modelBuilder.Entity<GeographicalData>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Openbareruimte)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Huisnummer)
                    .IsRequired();

                entity.Property(e => e.Huisletter)
                    .HasMaxLength(1);

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Woonplaats)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gemeente)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Provincie)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nummeraanduiding)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Verblijfsobjectgebruiksdoel)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Oppervlakteverblijfsobject)
                    .IsRequired();

                entity.Property(e => e.Verblijfsobjectstatus)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ObjectId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ObjectType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nevenadres)
                    .HasMaxLength(200);

                entity.Property(e => e.Pandid)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pandstatus)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Pandbouwjaar)
                    .IsRequired();

                entity.Property(e => e.X)
                    .IsRequired();

                entity.Property(e => e.Y)
                    .IsRequired();

                entity.Property(e => e.Lon)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

                entity.Property(e => e.Lat)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

                // Add indexes for common queries
                entity.HasIndex(e => e.Postcode)
                    .HasDatabaseName("IX_GeographicalData_Postcode");

                entity.HasIndex(e => e.Woonplaats)
                    .HasDatabaseName("IX_GeographicalData_Woonplaats");

                entity.HasIndex(e => e.Gemeente)
                    .HasDatabaseName("IX_GeographicalData_Gemeente");

                entity.HasIndex(e => new { e.Openbareruimte, e.Huisnummer, e.Postcode })
                    .HasDatabaseName("IX_GeographicalData_Address");
            });

            // Seed data
            modelBuilder.Entity<GeographicalData>().HasData(
                new GeographicalData
                {
                    Id = 1,
                    Openbareruimte = "Kerkstraat",
                    Huisnummer = 1,
                    Huisletter = "A",
                    Postcode = "1234AB",
                    Woonplaats = "Amsterdam",
                    Gemeente = "Amsterdam",
                    Provincie = "Noord-Holland",
                    Nummeraanduiding = "0363010000000001",
                    Verblijfsobjectgebruiksdoel = "woonfunctie",
                    Oppervlakteverblijfsobject = 75,
                    Verblijfsobjectstatus = "Verblijfsobject in gebruik",
                    ObjectId = "NL.IMBAG.Verblijfsobject.0363010000000001",
                    ObjectType = "Verblijfsobject",
                    Pandid = "NL.IMBAG.Pand.0363100000000001",
                    Pandstatus = "Pand in gebruik",
                    Pandbouwjaar = 1950,
                    X = 121000,
                    Y = 487000,
                    Lon = 4.9041,
                    Lat = 52.3676
                },
                new GeographicalData
                {
                    Id = 2,
                    Openbareruimte = "Hoofdstraat",
                    Huisnummer = 25,
                    Postcode = "3511AB",
                    Woonplaats = "Utrecht",
                    Gemeente = "Utrecht",
                    Provincie = "Utrecht",
                    Nummeraanduiding = "0344010000000002",
                    Verblijfsobjectgebruiksdoel = "winkelfunctie",
                    Oppervlakteverblijfsobject = 120,
                    Verblijfsobjectstatus = "Verblijfsobject in gebruik",
                    ObjectId = "NL.IMBAG.Verblijfsobject.0344010000000002",
                    ObjectType = "Verblijfsobject",
                    Pandid = "NL.IMBAG.Pand.0344100000000002",
                    Pandstatus = "Pand in gebruik",
                    Pandbouwjaar = 1975,
                    X = 155000,
                    Y = 463000,
                    Lon = 5.1214,
                    Lat = 52.0907
                },
                new GeographicalData
                {
                    Id = 3,
                    Openbareruimte = "Dorpsplein",
                    Huisnummer = 8,
                    Huisletter = "B",
                    Huisnummertoevoeging = 2,
                    Postcode = "6543CD",
                    Woonplaats = "Nijmegen",
                    Gemeente = "Nijmegen",
                    Provincie = "Gelderland",
                    Nummeraanduiding = "0268010000000003",
                    Verblijfsobjectgebruiksdoel = "kantoorfunctie",
                    Oppervlakteverblijfsobject = 200,
                    Verblijfsobjectstatus = "Verblijfsobject in gebruik",
                    ObjectId = "NL.IMBAG.Verblijfsobject.0268010000000003",
                    ObjectType = "Verblijfsobject",
                    Pandid = "NL.IMBAG.Pand.0268100000000003",
                    Pandstatus = "Pand in gebruik",
                    Pandbouwjaar = 2000,
                    X = 190000,
                    Y = 426000,
                    Lon = 5.8520,
                    Lat = 51.8126
                }
            );
        }
    }
}
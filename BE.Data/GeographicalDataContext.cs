using Microsoft.EntityFrameworkCore;
using BE.Domain.Entities;

namespace BE.Data
{
    public class GeographicalDataContext : DbContext
    {
        public GeographicalDataContext() { }

        public GeographicalDataContext(DbContextOptions<GeographicalDataContext> options)
            : base(options)
        {
        }

        public DbSet<GeographicalDataEntity> GeographicalData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GeographicalDataEntity>(entity =>
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

            modelBuilder.Entity<GeographicalDataEntity>(entity =>
{
    entity.HasData(
        new GeographicalDataEntity
        {
            Id = 1,
            Openbareruimte = "Hoofdstraat",
            Huisnummer = 1,
            Huisletter = null,
            Huisnummertoevoeging = null,
            Postcode = "1234AB",
            Woonplaats = "Amsterdam",
            Gemeente = "Amsterdam",
            Provincie = "Noord-Holland",
            Nummeraanduiding = "1",
            Verblijfsobjectgebruiksdoel = "Wonen",
            Oppervlakteverblijfsobject = 100,
            Verblijfsobjectstatus = "Actief",
            ObjectId = "OBJ1",
            ObjectType = "Pand",
            Nevenadres = null,
            Pandid = "PAND1",
            Pandstatus = "Bestaand",
            Pandbouwjaar = 1990,
            X = 12345,
            Y = 67890,
            Lon = 4.895168,
            Lat = 52.370216
        },
        new GeographicalDataEntity
        {
            Id = 2,
            Openbareruimte = "Dorpsstraat",
            Huisnummer = 2,
            Huisletter = "A",
            Huisnummertoevoeging = null,
            Postcode = "5678CD",
            Woonplaats = "Rotterdam",
            Gemeente = "Rotterdam",
            Provincie = "Zuid-Holland",
            Nummeraanduiding = "2A",
            Verblijfsobjectgebruiksdoel = "Wonen",
            Oppervlakteverblijfsobject = 80,
            Verblijfsobjectstatus = "Actief",
            ObjectId = "OBJ2",
            ObjectType = "Pand",
            Nevenadres = null,
            Pandid = "PAND2",
            Pandstatus = "Bestaand",
            Pandbouwjaar = 1980,
            X = 54321,
            Y = 98765,
            Lon = 4.47917,
            Lat = 51.9225
        },
        new GeographicalDataEntity
        {
            Id = 3,
            Openbareruimte = "Kerkstraat",
            Huisnummer = 3,
            Huisletter = null,
            Huisnummertoevoeging = 1,
            Postcode = "9012EF",
            Woonplaats = "Utrecht",
            Gemeente = "Utrecht",
            Provincie = "Utrecht",
            Nummeraanduiding = "3-1",
            Verblijfsobjectgebruiksdoel = "Wonen",
            Oppervlakteverblijfsobject = 120,
            Verblijfsobjectstatus = "Actief",
            ObjectId = "OBJ3",
            ObjectType = "Pand",
            Nevenadres = null,
            Pandid = "PAND3",
            Pandstatus = "Bestaand",
            Pandbouwjaar = 2000,
            X = 11111,
            Y = 22222,
            Lon = 5.12142,
            Lat = 52.09074
        }
    );
});
        }
    }
}
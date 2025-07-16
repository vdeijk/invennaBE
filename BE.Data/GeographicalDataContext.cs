using Microsoft.EntityFrameworkCore;
using BE.Domain.Models;

namespace BE.Data
{
    public class GeographicalDataContext : DbContext
    {
        public GeographicalDataContext(DbContextOptions<GeographicalDataContext> options)
            : base(options)
        {
        }

        public DbSet<BE.Domain.Models.GeographicalData> GeographicalData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // ...existing seed data and other configuration...
        }
    }
}

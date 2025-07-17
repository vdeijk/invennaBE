using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Domain.Entities;
using BE.Domain.Models;

namespace DataImporter;

class Program
{
    static async Task Main(string[] args)
    {
        var csvPath = @"C:\Users\Gebruiker\repos\Invenna\BE\BE\BE.Data\Data\geographicaldata-fixed.csv";

        var options = new DbContextOptionsBuilder<GeographicalDataContext>()
            .UseSqlite("Data Source=C:\\Users\\Gebruiker\\repos\\Invenna\\BE\\BE\\BE.Data\\geodata.db")
            .Options;

        using var context = new GeographicalDataContext(options);

        Console.WriteLine("Starting full data import...");
        
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migrations applied.");

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });

        context.GeographicalData.RemoveRange(context.GeographicalData);
        await context.SaveChangesAsync();

        var records = csv.GetRecords<dynamic>().ToList();
        Console.WriteLine($"Found {records.Count} records to import...");

        var batch = new List<GeographicalDataEntity>();
        int processed = 0;

        foreach (var record in records)
        {
            try
            {
                var dict = record as IDictionary<string, object>;
                if (dict == null) continue;
                
                var geoData = new GeographicalDataEntity
                {
                    Openbareruimte = dict["openbareruimte"]?.ToString() ?? "",
                    Huisnummer = int.TryParse(dict["huisnummer"]?.ToString(), out var hn) ? hn : 0,
                    Huisletter = dict["huisletter"]?.ToString(),
                    Huisnummertoevoeging = int.TryParse(dict["huisnummertoevoeging"]?.ToString(), out var hnt) ? hnt : null,
                    Postcode = dict["postcode"]?.ToString() ?? "",
                    Woonplaats = dict["woonplaats"]?.ToString() ?? "",
                    Gemeente = dict["gemeente"]?.ToString() ?? "",
                    Provincie = dict["provincie"]?.ToString() ?? "",
                    Nummeraanduiding = dict["nummeraanduiding"]?.ToString() ?? "",
                    Verblijfsobjectgebruiksdoel = dict["verblijfsobjectgebruiksdoel"]?.ToString() ?? "",
                    Oppervlakteverblijfsobject = int.TryParse(dict["oppervlakteverblijfsobject"]?.ToString(), out var ovs) ? ovs : 0,
                    Verblijfsobjectstatus = dict["verblijfsobjectstatus"]?.ToString() ?? "",
                    ObjectId = dict["object_id"]?.ToString() ?? "",
                    ObjectType = dict["object_type"]?.ToString() ?? "",
                    Nevenadres = dict["nevenadres"]?.ToString(),
                    Pandid = dict["pandid"]?.ToString() ?? "",
                    Pandstatus = dict["pandstatus"]?.ToString() ?? "",
                    Pandbouwjaar = int.TryParse(dict["pandbouwjaar"]?.ToString(), out var pbj) ? pbj : 0,
                    X = int.TryParse(dict["x"]?.ToString(), out var x) ? x : 0,
                    Y = int.TryParse(dict["y"]?.ToString(), out var y) ? y : 0,
                    Lon = double.TryParse(dict["lon"]?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var lon) ? lon : 0,
                    Lat = double.TryParse(dict["lat"]?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) ? lat : 0
                };

                batch.Add(geoData);
                processed++;

                if (batch.Count >= 100)
                {
                    context.GeographicalData.AddRange(batch);
                    await context.SaveChangesAsync();
                    batch.Clear();
                    Console.WriteLine($"Processed {processed} records...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing record {processed + 1}: {ex.Message}");
            }
        }

        if (batch.Any())
        {
            context.GeographicalData.AddRange(batch);
            await context.SaveChangesAsync();
        }

        Console.WriteLine($"Import completed! Total records imported: {processed}");

        var count = await context.GeographicalData.CountAsync();
        Console.WriteLine($"Database now contains {count} records.");
    }
}

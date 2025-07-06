using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BE.Data;

namespace BE.Import
{
    public class ImportTool
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<GeographicalDataContext>(options =>
                options.UseSqlite("Data Source=BE/Data/GeographicalData.db"));
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
            var count = ImportGeographicalData.ImportFromCsv(db, "BE/Data/geographicaldata (1).csv");
            Console.WriteLine($"Imported {count} records from CSV.");
        }
    }
}

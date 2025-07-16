using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BE.Data;

namespace BE.Import
{
    public class ImportTool
    {
        public static int ImportFromCsv(string dbPath, string csvPath)
        {
            var services = new ServiceCollection();
            services.AddDbContext<GeographicalDataContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
            return ImportGeographicalData.ImportFromCsv(db, csvPath);
        }
    }
}

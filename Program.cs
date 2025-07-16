using Microsoft.EntityFrameworkCore;
using BE.Import;

var builder = WebApplication.CreateBuilder(args);

var dataPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BE.Data", "GeographicalData.db"));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Geographical Data API",
        Version = "v1.0",
        Description = "API for managing geographical data"
    });
});

builder.Services.AddDbContext<BE.Data.GeographicalDataContext>(options =>
{
    options.UseSqlite($"Data Source={dataPath}");
});
builder.Services.AddScoped<BE.Domain.Interfaces.IGeographicalDataService, BE.Services.GeographicalDataService>();
builder.Services.AddScoped<BE.Domain.Interfaces.IGeographicalDataRepository, BE.Repositories.GeographicalDataRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Geographical Data API v1");
        options.RoutePrefix = string.Empty; // Swagger UI at app root
    });
}

app.UseHttpsRedirection();
app.MapControllers();

// Import CSV data if database is empty
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BE.Data.GeographicalDataContext>();
    if (!dbContext.GeographicalData.Any())
    {
        var csvPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "BE.Data", "Data", "geographicaldata (1).csv"));
        if (File.Exists(csvPath))
        {
            Console.WriteLine("Database is empty. Importing CSV data...");
            var importedCount = ImportGeographicalData.ImportFromCsv(dbContext, csvPath);
            Console.WriteLine($"Successfully imported {importedCount} records.");
        }
        else
        {
            Console.WriteLine($"CSV file not found at: {csvPath}");
        }
    }
    else
    {
        Console.WriteLine($"Database already contains {dbContext.GeographicalData.Count()} records.");
    }
}

app.Run();

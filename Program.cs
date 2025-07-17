using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dataPath = Path.Combine("BE.Data", "geodata.db");
builder.Services.AddDbContext<BE.Data.GeographicalDataContext>(options =>
{
    options.UseSqlite($"Data Source={dataPath}");
});

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

// Apply migrations or create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BE.Data.GeographicalDataContext>();
    dbContext.Database.Migrate();
}

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

app.Run();

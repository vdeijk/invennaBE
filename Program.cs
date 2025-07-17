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

// Configure HTTPS redirection
builder.Services.AddHttpsRedirection(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.HttpsPort = 7129; // Match the HTTPS port in launchSettings.json
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Geographical Data API",
        Version = "v1.0",
        Description = "API for managing geographical data"
    });
});

builder.Services.AddScoped<BE.Domain.Interfaces.IGeographicalDataService, BE.Services.GeographicalDataService>();
builder.Services.AddScoped<BE.Domain.Interfaces.IGeographicalDataRepository, BE.Repositories.GeographicalDataRepository>();
builder.Services.AddScoped<BE.Domain.Interfaces.IUnitOfWork, BE.Data.UnitOfWork>();

var app = builder.Build();

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
        options.RoutePrefix = string.Empty;
    });
}

// Configure HTTPS redirection with explicit port for development
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAngularApp");

app.MapControllers();

app.Run();

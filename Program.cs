using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Interfaces;
using BE.Repositories;
using BE.Import;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Geographical Data API",
        Version = "v1",
        Description = "API for managing geographical data"
    });
});

// Add Entity Framework
builder.Services.AddDbContext<GeographicalDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repository
builder.Services.AddScoped<IGeographicalDataRepository, GeographicalDataRepository>();

// Add CORS support for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Angular default port
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// CSV Import logic
if (args.Contains("--import"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
    var csvPath = Path.Combine("Data", "geographicaldata (1).csv");
    ImportGeographicalData.ImportFromCsv(db, csvPath);
    return; // Exit after import
}

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geographical Data API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthorization();
app.MapControllers();

app.Run();

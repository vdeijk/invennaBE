using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BE.Data.GeographicalDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<BE.Interfaces.IGeographicalDataRepository, BE.Repositories.GeographicalDataRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Ensure database and tables are created, then seed data if needed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BE.Data.GeographicalDataContext>();
    db.Database.EnsureCreated(); // <-- Use EnsureCreated for local dev
    if (!db.GeographicalData.Any())
    {
        db.GeographicalData.AddRange(
            new BE.Models.GeographicalData {
                Openbareruimte = "Kerkstraat",
                Huisnummer = 1,
                Huisletter = "A",
                Huisnummertoevoeging = null,
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
                Nevenadres = null,
                Pandid = "NL.IMBAG.Pand.0363100000000001",
                Pandstatus = "Pand in gebruik",
                Pandbouwjaar = 1950,
                X = 121000,
                Y = 487000,
                Lon = 4.9041,
                Lat = 52.3676
            },
            new BE.Models.GeographicalData {
                Openbareruimte = "Hoofdstraat",
                Huisnummer = 25,
                Huisletter = null,
                Huisnummertoevoeging = null,
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
                Nevenadres = null,
                Pandid = "NL.IMBAG.Pand.0344100000000002",
                Pandstatus = "Pand in gebruik",
                Pandbouwjaar = 1975,
                X = 155000,
                Y = 463000,
                Lon = 5.1214,
                Lat = 52.0907
            }
        );
        db.SaveChanges();
    }
}

app.Run();

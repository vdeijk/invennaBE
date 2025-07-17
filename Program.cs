using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BE.Domain.Interfaces;
using BE.Services;

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

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Model validation failed: {Errors}", 
                string.Join(", ", context.ModelState.SelectMany(ms => ms.Value?.Errors.Select(e => $"{ms.Key}: {e.ErrorMessage}") ?? Enumerable.Empty<string>())));
            
            return new BadRequestObjectResult(context.ModelState);
        };
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpsRedirection(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.HttpsPort = 7129; 
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
builder.Services.AddScoped<BE.Domain.Interfaces.IBusinessValidator, BE.Services.BusinessValidator>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request: {Method} {Path} {QueryString}", 
        context.Request.Method, 
        context.Request.Path, 
        context.Request.QueryString);
    await next();
});

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

if (!builder.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("EnableHttpsRedirection", false))
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngularApp");

app.MapControllers();

app.Run();

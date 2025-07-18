using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BE.Domain.Interfaces;
using BE.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider", "SQLite");

builder.Services.AddDbContext<BE.Data.GeographicalDataContext>(options =>
{
    switch (databaseProvider?.ToUpper())
    {
        case "SQLSERVER":
        case "LOCALDB":
            options.UseSqlServer(connectionString);
            break;
        case "INMEMORY":
            options.UseInMemoryDatabase("GeographicalDataTestDb");
            break;
        case "SQLITE":
        default:
            var sqliteConnection = connectionString ?? Path.Combine("BE.Data", "Data", "geodata.db");
            options.UseSqlite($"Data Source={sqliteConnection}");
            break;
    }
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
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
    if (!context.Response.HasStarted)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-API-Version"] = "1.0";
        context.Response.Headers["X-API-Name"] = "Geographical-Data-API";
    }
    
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await next();
        stopwatch.Stop();
        
        if (!context.Response.HasStarted)
        {
            context.Response.Headers["X-Response-Time-Ms"] = stopwatch.ElapsedMilliseconds.ToString();
        }
    });
}

app.UseMiddleware<BE.API.Middleware.GlobalExceptionMiddleware>();

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
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        if (databaseProvider?.ToUpper() == "SQLITE")
        {
            var connectionStringForLog = connectionString ?? Path.Combine("BE.Data", "Data", "geodata.db");
            logger.LogInformation("Attempting to connect to SQLite database at: {DatabasePath}", connectionStringForLog);
            
            // Extract the actual file path from the connection string
            var dbFilePath = connectionStringForLog;
            if (connectionStringForLog.StartsWith("Data Source="))
            {
                dbFilePath = connectionStringForLog.Substring("Data Source=".Length);
            }
            
            var dbPath = Path.GetDirectoryName(dbFilePath);
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
                logger.LogInformation("Created database directory: {DatabaseDirectory}", dbPath);
            }
        }
        
        if (!string.IsNullOrEmpty(databaseProvider) && databaseProvider.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
        {
            dbContext.Database.EnsureCreated();
            logger.LogInformation("In-memory database created successfully");
        }
        else
        {
            dbContext.Database.Migrate();
            logger.LogInformation("Database migration completed successfully using provider: {DatabaseProvider}", databaseProvider);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during database migration with provider {DatabaseProvider}. Connection: {ConnectionString}", 
            databaseProvider, connectionString ?? "default");
        throw;
    }
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

public partial class Program { }

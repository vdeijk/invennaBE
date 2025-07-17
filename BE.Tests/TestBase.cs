using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BE.Data;
using BE.Domain.Interfaces;
using BE.Services;
using BE.Repositories;
using BE.Domain.Entities;

namespace BE.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly GeographicalDataContext Context;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ILogger<TestBase> Logger;

    protected TestBase()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<GeographicalDataContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors());

        services.AddLogging(builder => builder.AddConsole());

        services.AddScoped<IGeographicalDataService, GeographicalDataService>();
        services.AddScoped<IGeographicalDataRepository, GeographicalDataRepository>();
        services.AddScoped<IUnitOfWork, TestUnitOfWork>();
        services.AddScoped<IBusinessValidator, BusinessValidator>();

        ServiceProvider = services.BuildServiceProvider();
        Context = ServiceProvider.GetRequiredService<GeographicalDataContext>();
        Logger = ServiceProvider.GetRequiredService<ILogger<TestBase>>();

        Context.Database.EnsureCreated();
        SeedTestData();
    }

    protected virtual void SeedTestData()
    {
    }

    public void Dispose()
    {
        Context?.Dispose();
        ServiceProvider?.GetService<IServiceScope>()?.Dispose();
    }
}

using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BE.Data;

namespace BE.Tests.Configuration;

public class DatabaseProviderTests
{
    [Fact]
    public void SQLite_Configuration_ShouldCreateSQLiteContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("DatabaseProvider", "SQLite"),
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", "Data Source=test.db")
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDbContext<GeographicalDataContext>(options =>
        {
            var provider = configuration.GetValue<string>("DatabaseProvider", "SQLite");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            switch (provider?.ToUpper())
            {
                case "SQLITE":
                default:
                    options.UseSqlite(connectionString ?? "Data Source=test.db");
                    break;
            }
        });

        var serviceProvider = services.BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<GeographicalDataContext>();

        context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.Sqlite");
    }

    [Fact]
    public void InMemory_Configuration_ShouldCreateInMemoryContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("DatabaseProvider", "InMemory")
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDbContext<GeographicalDataContext>(options =>
        {
            var provider = configuration.GetValue<string>("DatabaseProvider", "SQLite");

            switch (provider?.ToUpper())
            {
                case "INMEMORY":
                    options.UseInMemoryDatabase("TestDb");
                    break;
                case "SQLITE":
                default:
                    options.UseSqlite("Data Source=test.db");
                    break;
            }
        });

        var serviceProvider = services.BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<GeographicalDataContext>();

        context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.InMemory");
    }

    [Fact]
    public void SqlServer_Configuration_ShouldCreateSqlServerContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("DatabaseProvider", "SqlServer"),
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", "Server=test;Database=test;")
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDbContext<GeographicalDataContext>(options =>
        {
            var provider = configuration.GetValue<string>("DatabaseProvider", "SQLite");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            switch (provider?.ToUpper())
            {
                case "SQLSERVER":
                case "LOCALDB":
                    options.UseSqlServer(connectionString);
                    break;
                case "INMEMORY":
                    options.UseInMemoryDatabase("TestDb");
                    break;
                case "SQLITE":
                default:
                    options.UseSqlite("Data Source=test.db");
                    break;
            }
        });

        var serviceProvider = services.BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<GeographicalDataContext>();

        context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.SqlServer");
    }

    [Fact]
    public void DefaultProvider_ShouldBeSQLite()
    {
        var configuration = new ConfigurationBuilder().Build(); // Empty configuration

        var services = new ServiceCollection();
        services.AddDbContext<GeographicalDataContext>(options =>
        {
            var provider = configuration.GetValue<string>("DatabaseProvider", "SQLite");

            switch (provider?.ToUpper())
            {
                case "SQLSERVER":
                case "LOCALDB":
                    options.UseSqlServer("Server=test;Database=test;");
                    break;
                case "INMEMORY":
                    options.UseInMemoryDatabase("TestDb");
                    break;
                case "SQLITE":
                default:
                    options.UseSqlite("Data Source=test.db");
                    break;
            }
        });

        var serviceProvider = services.BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<GeographicalDataContext>();

        context.Database.ProviderName.Should().Be("Microsoft.EntityFrameworkCore.Sqlite");
    }

    [Fact]
    public void InMemoryDatabase_ShouldCreateAndSeedData()
    {
        var services = new ServiceCollection();
        services.AddDbContext<GeographicalDataContext>(options =>
            options.UseInMemoryDatabase("SeedTestDb"));

        var serviceProvider = services.BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<GeographicalDataContext>();
        context.Database.EnsureCreated();

        context.GeographicalData.Should().HaveCountGreaterThan(0, "because seed data should be loaded");
        context.GeographicalData.Should().Contain(g => g.Openbareruimte == "Hoofdstraat");
        context.GeographicalData.Should().Contain(g => g.Gemeente == "Amsterdam");
    }
}

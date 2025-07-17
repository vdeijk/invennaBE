using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Json;
using System.Net;
using BE.Data;
using BE.Domain.DTOs;
using BE.Domain.Entities;
using BE.Domain.Interfaces;

namespace BE.Tests.Integration;

public class GeographicalDataControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GeographicalDataControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<GeographicalDataContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(GeographicalDataContext));
                if (contextDescriptor != null)
                    services.Remove(contextDescriptor);

                var unitOfWorkDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IUnitOfWork));
                if (unitOfWorkDescriptor != null)
                    services.Remove(unitOfWorkDescriptor);

                services.AddDbContext<GeographicalDataContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid());
                });

                services.AddScoped<IUnitOfWork, TestUnitOfWork>();
            });

            builder.UseEnvironment("Testing");
        });

        _client = _factory.CreateClient();
        
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
        context.Database.EnsureCreated();
        SeedTestData(context);
    }

    private static void SeedTestData(GeographicalDataContext context)
    {
        if (!context.GeographicalData.Any())
        {
            context.GeographicalData.AddRange(
                new GeographicalDataEntity
                {
                    Openbareruimte = "Integration Test Street",
                    Huisnummer = 1,
                    Postcode = "1000AB",
                    Woonplaats = "TestCity",
                    Gemeente = "TestMunicipality",
                    Provincie = "TestProvince",
                    Nummeraanduiding = "INT001",
                    Verblijfsobjectgebruiksdoel = "Residential",
                    Oppervlakteverblijfsobject = 100,
                    Verblijfsobjectstatus = "Active",
                    ObjectId = "OBJ001",
                    ObjectType = "Building",
                    Pandid = "PAND001",
                    Pandstatus = "Existing",
                    Pandbouwjaar = 2020,
                    X = 100000,
                    Y = 400000,
                    Lon = 4.9,
                    Lat = 52.3
                }
            );
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllGeographicalData()
    {
        var response = await _client.GetAsync("/api/GeographicalData");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var data = await response.Content.ReadFromJsonAsync<List<GeographicalDataDto>>();
        data.Should().NotBeNull();
        data.Should().HaveCountGreaterThan(0);
        data.Should().Contain(g => g.Openbareruimte == "Integration Test Street");
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnGeographicalData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
        var testData = context.GeographicalData.First();

        var response = await _client.GetAsync($"/api/GeographicalData/{testData.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var data = await response.Content.ReadFromJsonAsync<GeographicalDataDto>();
        data.Should().NotBeNull();
        data!.Id.Should().Be(testData.Id);
        data.Openbareruimte.Should().Be(testData.Openbareruimte);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/GeographicalData/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldCreateGeographicalData()
    {
        var createDto = new CreateGeographicalDataDto
        {
            Openbareruimte = "API Test Street",
            Huisnummer = 42,
            Postcode = "5000IJ",
            Woonplaats = "APICity",
            Gemeente = "APIMunicipality",
            Provincie = "APIProvince",
            Nummeraanduiding = "API001",
            Verblijfsobjectgebruiksdoel = "Commercial",
            Oppervlakteverblijfsobject = 250,
            Verblijfsobjectstatus = "Active",
            ObjectId = "OBJ042",
            ObjectType = "Store",
            Pandid = "PAND042",
            Pandstatus = "Modern",
            Pandbouwjaar = 2023,
            X = 140000,
            Y = 440000,
            Lon = 5.3,
            Lat = 52.7
        };

        var response = await _client.PostAsJsonAsync("/api/GeographicalData", createDto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdData = await response.Content.ReadFromJsonAsync<GeographicalDataDto>();
        createdData.Should().NotBeNull();
        createdData!.Id.Should().BeGreaterThan(0);
        createdData.Openbareruimte.Should().Be("API Test Street");
        createdData.Huisnummer.Should().Be(42);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain($"/api/GeographicalData/{createdData.Id}");
    }

    [Fact]
    public async Task Create_WithInvalidData_ShouldReturnBadRequest()
    {
        var invalidDto = new CreateGeographicalDataDto
        {
            Huisnummer = 1,
        };

        var response = await _client.PostAsJsonAsync("/api/GeographicalData", invalidDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldUpdateGeographicalData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
        var testData = context.GeographicalData.First();

        var updateDto = new UpdateGeographicalDataDto
        {
            Id = testData.Id,
            Openbareruimte = "Updated Integration Street",
            Huisnummer = testData.Huisnummer,
            Postcode = testData.Postcode,
            Woonplaats = testData.Woonplaats,
            Gemeente = testData.Gemeente,
            Provincie = testData.Provincie,
            Nummeraanduiding = testData.Nummeraanduiding,
            Verblijfsobjectgebruiksdoel = testData.Verblijfsobjectgebruiksdoel,
            Oppervlakteverblijfsobject = testData.Oppervlakteverblijfsobject,
            Verblijfsobjectstatus = testData.Verblijfsobjectstatus,
            ObjectId = testData.ObjectId,
            ObjectType = testData.ObjectType,
            Pandid = testData.Pandid,
            Pandstatus = testData.Pandstatus,
            Pandbouwjaar = testData.Pandbouwjaar,
            X = testData.X,
            Y = testData.Y,
            Lon = testData.Lon,
            Lat = testData.Lat
        };

        var response = await _client.PutAsJsonAsync($"/api/GeographicalData/{testData.Id}", updateDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedData = await response.Content.ReadFromJsonAsync<GeographicalDataDto>();
        updatedData.Should().NotBeNull();
        updatedData!.Openbareruimte.Should().Be("Updated Integration Street");
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldDeleteGeographicalData()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GeographicalDataContext>();
        var testData = context.GeographicalData.First();

        var response = await _client.DeleteAsync($"/api/GeographicalData/{testData.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var getResponse = await _client.GetAsync($"/api/GeographicalData/{testData.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.DeleteAsync("/api/GeographicalData/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BE.Domain.Interfaces;
using BE.Domain.DTOs;

namespace BE.Tests.Services;

public class GeographicalDataServiceTests : TestBase
{
    private readonly IGeographicalDataService _service;

    public GeographicalDataServiceTests()
    {
        _service = ServiceProvider.GetRequiredService<IGeographicalDataService>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGeographicalDataDtos()
    {
        var result = await _service.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(3, "because the database has 3 seeded entities");
        result.Should().Contain(g => g.Openbareruimte == "Hoofdstraat");
        result.Should().Contain(g => g.Openbareruimte == "Dorpsstraat");
        result.Should().Contain(g => g.Openbareruimte == "Kerkstraat");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnGeographicalDataDto()
    {
        var testData = Context.GeographicalData.First();

        var result = await _service.GetByIdAsync(testData.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(testData.Id);
        result.Openbareruimte.Should().Be(testData.Openbareruimte);
        result.Huisnummer.Should().Be(testData.Huisnummer);
        result.Postcode.Should().Be(testData.Postcode);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        await _service.Invoking(s => s.GetByIdAsync(999))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldCreateAndReturnGeographicalDataDto()
    {
        var createDto = new CreateGeographicalDataDto
        {
            Openbareruimte = "Service Test Street",
            Huisnummer = 25,
            Postcode = "4000GH",
            Woonplaats = "ServiceCity",
            Gemeente = "ServiceMunicipality",
            Provincie = "ServiceProvince",
            Nummeraanduiding = "SVC001",
            Verblijfsobjectgebruiksdoel = "Office",
            Oppervlakteverblijfsobject = 300,
            Verblijfsobjectstatus = "Active",
            ObjectId = "OBJ004",
            ObjectType = "Office Building",
            Pandid = "PAND004",
            Pandstatus = "Modern",
            Pandbouwjaar = 2024,
            X = 130000,
            Y = 430000,
            Lon = 5.2,
            Lat = 52.6
        };
        var originalCount = Context.GeographicalData.Count();

        var result = await _service.CreateAsync(createDto);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Openbareruimte.Should().Be("Service Test Street");
        result.Huisnummer.Should().Be(25);
        result.Postcode.Should().Be("4000GH");
        
        Context.GeographicalData.Should().HaveCount(originalCount + 1);
        var fromDb = Context.GeographicalData.FirstOrDefault(g => g.Openbareruimte == "Service Test Street");
        fromDb.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        var updateDto = new UpdateGeographicalDataDto
        {
            Id = 999,
            Openbareruimte = "Valid Street",
            Huisnummer = 1,
            Postcode = "1234AB",
            Woonplaats = "Valid City",
            Gemeente = "Valid Municipality",
            Provincie = "Valid Province",
            Nummeraanduiding = "VALID001",
            Verblijfsobjectgebruiksdoel = "Residential",
            Oppervlakteverblijfsobject = 100,
            Verblijfsobjectstatus = "Active",
            ObjectId = "VALID001",
            ObjectType = "Building",
            Pandid = "VALIDPAND001",
            Pandstatus = "Existing",
            Pandbouwjaar = 2020,
            X = 155000, 
            Y = 450000, 
            Lon = 4.9, 
            Lat = 52.3 
        };

        await _service.Invoking(s => s.UpdateAsync(999, updateDto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Update validatie gefaald: Entiteit met ID 999 bestaat niet*");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteGeographicalData()
    {
        var testData = Context.GeographicalData.AsNoTracking().First();
        var originalCount = Context.GeographicalData.Count();

        await _service.DeleteAsync(testData.Id);

        Context.GeographicalData.Should().HaveCount(originalCount - 1);
        Context.GeographicalData.Should().NotContain(g => g.Id == testData.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        await _service.Invoking(s => s.DeleteAsync(999))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}

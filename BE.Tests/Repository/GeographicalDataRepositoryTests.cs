using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BE.Domain.Interfaces;
using BE.Domain.Entities;

namespace BE.Tests.Repository;

public class GeographicalDataRepositoryTests : TestBase
{
    private readonly IGeographicalDataRepository _repository;

    public GeographicalDataRepositoryTests()
    {
        _repository = ServiceProvider.GetRequiredService<IGeographicalDataRepository>();
    }

    protected IGeographicalDataRepository Repository => _repository;

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGeographicalData()
    {
        var result = await Repository.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(3, "because the database has 3 seeded entities");
        result.Should().Contain(g => g.Openbareruimte == "Hoofdstraat");
        result.Should().Contain(g => g.Openbareruimte == "Dorpsstraat");
        result.Should().Contain(g => g.Openbareruimte == "Kerkstraat");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnGeographicalData()
    {
        var testData = Context.GeographicalData.First();

        var result = await _repository.GetByIdAsync(testData.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(testData.Id);
        result.Openbareruimte.Should().Be(testData.Openbareruimte);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateGeographicalData()
    {
        var newData = new GeographicalDataEntity
        {
            Openbareruimte = "New Street",
            Huisnummer = 10,
            Postcode = "3000EF",
            Woonplaats = "NewCity",
            Gemeente = "NewMunicipality",
            Provincie = "NewProvince",
            Nummeraanduiding = "NEW001",
            Verblijfsobjectgebruiksdoel = "Mixed",
            Oppervlakteverblijfsobject = 150,
            Verblijfsobjectstatus = "Active",
            ObjectId = "OBJ003",
            ObjectType = "Complex",
            Pandid = "PAND003",
            Pandstatus = "New",
            Pandbouwjaar = 2023,
            X = 120000,
            Y = 420000,
            Lon = 5.1,
            Lat = 52.5
        };

        var result = await _repository.CreateAsync(newData);
        await Context.SaveChangesAsync();

        result.Should().NotBeNull();
        result.Openbareruimte.Should().Be("New Street");
        
        var fromDb = await Context.GeographicalData.FindAsync(result.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Openbareruimte.Should().Be("New Street");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteGeographicalData()
    {
        var testData = Context.GeographicalData.First();
        var originalCount = Context.GeographicalData.Count();

        var result = await _repository.DeleteAsync(testData.Id);
        await Context.SaveChangesAsync();

        result.Should().BeTrue();
        Context.GeographicalData.Should().HaveCount(originalCount - 1);
        Context.GeographicalData.Should().NotContain(g => g.Id == testData.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        var originalCount = Context.GeographicalData.Count();

        var result = await _repository.DeleteAsync(999);

        result.Should().BeFalse();
        Context.GeographicalData.Should().HaveCount(originalCount);
    }
}

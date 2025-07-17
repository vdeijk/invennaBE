using BE.Domain.Interfaces;
using BE.Data;
using Microsoft.Extensions.Logging;

namespace BE.Tests;

/// <summary>
/// Test-specific UnitOfWork that doesn't use transactions to avoid issues with in-memory database
/// </summary>
public class TestUnitOfWork : IUnitOfWork
{
    private readonly GeographicalDataContext _context;
    private readonly ILogger<TestUnitOfWork> _logger;
    private readonly IGeographicalDataRepository _geographicalData;

    public TestUnitOfWork(
        GeographicalDataContext context, 
        ILogger<TestUnitOfWork> logger,
        IGeographicalDataRepository geographicalData)
    {
        _context = context;
        _logger = logger;
        _geographicalData = geographicalData;
    }

    public IGeographicalDataRepository GeographicalData => _geographicalData;

    public async Task<int> SaveChangesAsync()
    {
        _logger.LogDebug("Saving changes to test database");
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _logger.LogDebug("BeginTransaction called - skipping for in-memory database");
        await Task.CompletedTask;
    }

    public async Task CommitTransactionAsync()
    {
        _logger.LogDebug("CommitTransaction called - skipping for in-memory database");
        await Task.CompletedTask;
    }

    public async Task RollbackTransactionAsync()
    {
        _logger.LogDebug("RollbackTransaction called - skipping for in-memory database");
        await Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}

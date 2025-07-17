using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using BE.Data;
using BE.Domain.Interfaces;
using BE.Repositories;

namespace BE.Data
{
    /// <summary>
    /// Unit of Work implementation for managing transactions across multiple repositories.
    /// Provides atomic operations and ensures data consistency.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeographicalDataContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Lazy-loaded repositories
        private IGeographicalDataRepository? _geographicalDataRepository;

        public UnitOfWork(
            GeographicalDataContext context, 
            ILogger<UnitOfWork> logger,
            IGeographicalDataRepository geographicalDataRepository)
        {
            _context = context;
            _logger = logger;
            _geographicalDataRepository = geographicalDataRepository;
        }

        /// <summary>
        /// Repository for geographical data operations.
        /// </summary>
        public IGeographicalDataRepository GeographicalData => _geographicalDataRepository!;

        /// <summary>
        /// Saves all pending changes to the database in a single transaction.
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _logger.LogDebug("Saving changes to database");
                var result = await _context.SaveChangesAsync();
                _logger.LogDebug("Successfully saved {Count} changes to database", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to database");
                throw;
            }
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Transaction already started");
            }

            _logger.LogDebug("Beginning database transaction");
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction to commit");
            }

            try
            {
                _logger.LogDebug("Committing database transaction");
                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while committing transaction");
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                return;
            }

            try
            {
                _logger.LogDebug("Rolling back database transaction");
                await _transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while rolling back transaction");
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Disposes the unit of work and its resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                // Note: Don't dispose _context here as it's managed by DI container
                _disposed = true;
            }
        }
    }
}

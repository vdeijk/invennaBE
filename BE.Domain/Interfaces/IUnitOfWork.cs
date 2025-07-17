using System;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing transactions across multiple repositories.
    /// Ensures data consistency and provides atomic operations.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository for geographical data operations.
        /// </summary>
        IGeographicalDataRepository GeographicalData { get; }

        /// <summary>
        /// Saves all pending changes to the database in a single transaction.
        /// </summary>
        /// <returns>The number of entities written to the database.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}

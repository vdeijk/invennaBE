using BE.Domain.Models;
using BE.Domain.Entities;

namespace BE.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for data access operations on geographical data entities.
    /// </summary>
    public interface IGeographicalDataRepository
    {
        /// <summary>
        /// Gets all geographical data entities.
        /// </summary>
        /// <returns>List of all geographical data entities.</returns>
        Task<IEnumerable<GeographicalDataEntity>> GetAllAsync();

        /// <summary>
        /// Gets a paged list of geographical data entities with optional filtering and sorting.
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <returns>Paged result of geographical data entities.</returns>
        Task<PagedResult<GeographicalDataEntity>> GetPagedAsync(PaginationParameters parameters);

        /// <summary>
        /// Gets a geographical data entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data entity.</param>
        /// <returns>The geographical data entity, or null if not found.</returns>
        Task<GeographicalDataEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new geographical data entity.
        /// </summary>
        /// <param name="geographicalData">The entity to create.</param>
        /// <returns>The created geographical data entity.</returns>
        Task<GeographicalDataEntity> CreateAsync(GeographicalDataEntity geographicalData);

        /// <summary>
        /// Updates an existing geographical data entity.
        /// </summary>
        /// <param name="geographicalData">The updated entity.</param>
        /// <returns>The updated geographical data entity.</returns>
        Task<GeographicalDataEntity> UpdateAsync(GeographicalDataEntity geographicalData);

        /// <summary>
        /// Deletes a geographical data entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Checks if a geographical data entity exists by its ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if exists, false otherwise.</returns>
        Task<bool> ExistsAsync(int id);
    }
}

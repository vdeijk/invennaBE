using BE.Models;

namespace BE.Interfaces
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
        Task<IEnumerable<GeographicalData>> GetAllAsync();

        /// <summary>
        /// Gets a geographical data entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data entity.</param>
        /// <returns>The geographical data entity, or null if not found.</returns>
        Task<GeographicalData?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new geographical data entity.
        /// </summary>
        /// <param name="geographicalData">The entity to create.</param>
        /// <returns>The created geographical data entity.</returns>
        Task<GeographicalData> CreateAsync(GeographicalData geographicalData);

        /// <summary>
        /// Updates an existing geographical data entity.
        /// </summary>
        /// <param name="geographicalData">The updated entity.</param>
        /// <returns>The updated geographical data entity.</returns>
        Task<GeographicalData> UpdateAsync(GeographicalData geographicalData);

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
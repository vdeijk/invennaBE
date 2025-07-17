using BE.Domain.DTOs;
using BE.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    /// <summary>
    /// Service interface for managing geographical data.
    /// </summary>
    public interface IGeographicalDataService
    {
        /// <summary>
        /// Gets all geographical data items.
        /// </summary>
        /// <returns>List of all geographical data items.</returns>
        Task<IEnumerable<GeographicalDataDto>> GetAllAsync();

        /// <summary>
        /// Gets a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item.</param>
        /// <returns>The geographical data item.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the item is not found.</exception>
        Task<GeographicalDataDto> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new geographical data item.
        /// </summary>
        /// <param name="dto">The data for the new geographical data item.</param>
        /// <returns>The created geographical data item.</returns>
        Task<GeographicalDataDto> CreateAsync(CreateGeographicalDataDto dto);

        /// <summary>
        /// Updates an existing geographical data item.
        /// </summary>
        /// <param name="id">The ID of the geographical data item to update.</param>
        /// <param name="dto">The updated data.</param>
        /// <returns>The updated geographical data item.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the item is not found.</exception>
        /// <exception cref="ArgumentException">Thrown when validation fails or ID mismatch.</exception>
        Task<GeographicalDataDto> UpdateAsync(int id, UpdateGeographicalDataDto dto);

        /// <summary>
        /// Deletes a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the item is not found.</exception>
        Task DeleteAsync(int id);

        /// <summary>
        /// Gets a paginated list of geographical data items with optional search and sorting.
        /// </summary>
        /// <param name="parameters">Pagination, search, and sorting parameters.</param>
        /// <returns>A paginated result containing geographical data items.</returns>
        Task<PagedResult<GeographicalDataDto>> GetPagedAsync(PaginationParameters parameters);
    }
}

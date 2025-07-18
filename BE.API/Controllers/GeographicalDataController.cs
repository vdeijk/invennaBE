using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BE.Domain.Interfaces;
using BE.Domain.DTOs;
using BE.Domain.Models;
using Microsoft.Extensions.Logging;

namespace BE.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GeographicalDataController : BaseApiController
    {
        private readonly ILogger<GeographicalDataController> _logger;
        private readonly IGeographicalDataService _service;

        public GeographicalDataController(
            ILogger<GeographicalDataController> logger,
            IGeographicalDataService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Retrieves all geographical data items.
        /// </summary>
        /// <returns>List of all geographical data items.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GeographicalDataDto>>> GetGeographicalData()
        {
            _logger.LogInformation("Retrieving all geographical data");
            var dtoList = await _service.GetAllAsync();
            return Ok(dtoList);
        }

        /// <summary>
        /// Retrieves a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item.</param>
        /// <returns>The geographical data item, or 404 if not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeographicalDataDto>> GetGeographicalData(int id)
        {
            _logger.LogInformation("Retrieving geographical data with ID: {Id}", id);
            var dto = await _service.GetByIdAsync(id);
            return Ok(dto);
        }
        /// <summary>
        /// Creates a new geographical data item.
        /// </summary>
        /// <param name="dto">The data for the new geographical data item.</param>
        /// <returns>The created geographical data item.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeographicalDataDto>> CreateGeographicalData([FromBody] CreateGeographicalDataDto dto)
        {
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Created new geographical data with ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetGeographicalData), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing geographical data item.
        /// </summary>
        /// <param name="id">The ID of the geographical data item to update.</param>
        /// <param name="dto">The updated data.</param>
        /// <returns>The updated geographical data item, or 404 if not found.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GeographicalDataDto>> UpdateGeographicalData(int id, [FromBody] UpdateGeographicalDataDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(Problem(
                    detail: "ID mismatch",
                    statusCode: 400));
            }

            var updated = await _service.UpdateAsync(id, dto);
            _logger.LogInformation("Updated geographical data with ID: {Id}", id);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item to delete.</param>
        /// <returns>No content if deleted, 404 if not found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteGeographicalData(int id)
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Deleted geographical data with ID: {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Retrieves geographical data items with pagination, search, and sorting.
        /// </summary>
        /// <param name="parameters">Pagination parameters including page, size, search, and sort options</param>
        /// <returns>Paginated list of geographical data items with metadata.</returns>
        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<GeographicalDataDto>>> GetGeographicalDataPaged([FromQuery] PaginationParameters parameters)
        {
            _logger.LogInformation("Retrieving geographical data - Page: {Page}, Size: {Size}, Search: {Search}, Sort: {SortBy} {SortDirection}", 
                parameters.Page, parameters.PageSize, parameters.Search, parameters.SortBy, parameters.SortDirection);

            var result = await _service.GetPagedAsync(parameters);

            return Ok(result);
        }
    }
}
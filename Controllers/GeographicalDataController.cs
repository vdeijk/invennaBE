using Microsoft.AspNetCore.Mvc;
using BE.Interfaces;
using BE.DTOs;
using Microsoft.AspNetCore.Http;

namespace BE.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GeographicalDataController : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GeographicalDataDto>>> GetGeographicalData()
        {
            try
            {
                _logger.LogInformation("Retrieving all geographical data");
                var dtoList = await _service.GetAllAsync();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving geographical data");
                return StatusCode(500, Problem(
                    detail: "Internal server error while retrieving geographical data",
                    statusCode: 500));
            }
        }

        /// <summary>
        /// Retrieves a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item.</param>
        /// <returns>The geographical data item, or 404 if not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GeographicalDataDto>> GetGeographicalData(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving geographical data with ID: {Id}", id);
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found", id);
                    return NotFound(Problem(
                        detail: $"Geographical data with ID {id} not found",
                        statusCode: 404));
                }
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving geographical data with ID: {Id}", id);
                return StatusCode(500, Problem(
                    detail: "Internal server error while retrieving geographical data",
                    statusCode: 500));
            }
        }

        /// <summary>
        /// Creates a new geographical data item.
        /// </summary>
        /// <param name="dto">The data for the new geographical data item.</param>
        /// <returns>The created geographical data item.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GeographicalDataDto>> CreateGeographicalData([FromBody] CreateGeographicalDataDto dto)
        {
            // [ApiController] handles null and model validation automatically
            try
            {
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Created new geographical data with ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetGeographicalData), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating geographical data");
                return StatusCode(500, Problem(
                    detail: "Internal server error while creating geographical data",
                    statusCode: 500));
            }
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GeographicalDataDto>> UpdateGeographicalData(int id, [FromBody] UpdateGeographicalDataDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(Problem(
                    detail: "ID mismatch",
                    statusCode: 400));
            }
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found for update", id);
                    return NotFound(Problem(
                        detail: $"Geographical data with ID {id} not found",
                        statusCode: 404));
                }
                _logger.LogInformation("Updated geographical data with ID: {Id}", id);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating geographical data with ID: {Id}", id);
                return StatusCode(500, Problem(
                    detail: "Internal server error while updating geographical data",
                    statusCode: 500));
            }
        }

        /// <summary>
        /// Deletes a geographical data item by its ID.
        /// </summary>
        /// <param name="id">The ID of the geographical data item to delete.</param>
        /// <returns>No content if deleted, 404 if not found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteGeographicalData(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found for deletion", id);
                    return NotFound(Problem(
                        detail: $"Geographical data with ID {id} not found",
                        statusCode: 404));
                }
                _logger.LogInformation("Deleted geographical data with ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting geographical data with ID: {Id}", id);
                return StatusCode(500, Problem(
                    detail: "Internal server error while deleting geographical data",
                    statusCode: 500));
            }
        }
    }
}
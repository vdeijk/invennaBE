using Microsoft.AspNetCore.Mvc;
using BE.Models;
using BE.Interfaces;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeographicalDataController : ControllerBase
    {
        private readonly ILogger<GeographicalDataController> _logger;
        private readonly IGeographicalDataRepository _repository;

        public GeographicalDataController(
            ILogger<GeographicalDataController> logger,
            IGeographicalDataRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Gets all geographical data
        /// </summary>
        /// <returns>List of all geographical data items</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeographicalData>>> GetGeographicalData()
        {
            try
            {
                _logger.LogInformation("Retrieving all geographical data");
                var data = await _repository.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving geographical data");
                return StatusCode(500, "Internal server error while retrieving geographical data");
            }
        }

        /// <summary>
        /// Gets geographical data by ID
        /// </summary>
        /// <param name="id">The ID of the geographical data item</param>
        /// <returns>The geographical data item with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GeographicalData>> GetGeographicalData(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving geographical data with ID: {Id}", id);
                
                var item = await _repository.GetByIdAsync(id);
                
                if (item == null)
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found", id);
                    return NotFound($"Geographical data with ID {id} not found");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving geographical data with ID: {Id}", id);
                return StatusCode(500, "Internal server error while retrieving geographical data");
            }
        }

        /// <summary>
        /// Creates a new geographical data item
        /// </summary>
        /// <param name="geographicalData">The geographical data to create</param>
        /// <returns>The created geographical data item</returns>
        [HttpPost]
        public async Task<ActionResult<GeographicalData>> CreateGeographicalData(GeographicalData geographicalData)
        {
            try
            {
                if (geographicalData == null)
                {
                    return BadRequest("Geographical data cannot be null");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(geographicalData.Openbareruimte) ||
                    string.IsNullOrEmpty(geographicalData.Postcode) ||
                    string.IsNullOrEmpty(geographicalData.Woonplaats))
                {
                    return BadRequest("Required fields (Openbareruimte, Postcode, Woonplaats) cannot be empty");
                }

                // Reset ID for creation
                geographicalData.Id = 0;
                
                var createdItem = await _repository.CreateAsync(geographicalData);

                _logger.LogInformation("Created new geographical data with ID: {Id}", createdItem.Id);
                
                return CreatedAtAction(
                    nameof(GetGeographicalData),
                    new { id = createdItem.Id },
                    createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating geographical data");
                return StatusCode(500, "Internal server error while creating geographical data");
            }
        }

        /// <summary>
        /// Updates an existing geographical data item
        /// </summary>
        /// <param name="id">The ID of the geographical data item to update</param>
        /// <param name="geographicalData">The updated geographical data</param>
        /// <returns>The updated geographical data item</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GeographicalData>> UpdateGeographicalData(int id, GeographicalData geographicalData)
        {
            try
            {
                if (geographicalData == null)
                {
                    return BadRequest("Geographical data cannot be null");
                }

                if (id != geographicalData.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!await _repository.ExistsAsync(id))
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found for update", id);
                    return NotFound($"Geographical data with ID {id} not found");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(geographicalData.Openbareruimte) ||
                    string.IsNullOrEmpty(geographicalData.Postcode) ||
                    string.IsNullOrEmpty(geographicalData.Woonplaats))
                {
                    return BadRequest("Required fields (Openbareruimte, Postcode, Woonplaats) cannot be empty");
                }

                var updatedItem = await _repository.UpdateAsync(geographicalData);

                _logger.LogInformation("Updated geographical data with ID: {Id}", id);
                
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating geographical data with ID: {Id}", id);
                return StatusCode(500, "Internal server error while updating geographical data");
            }
        }

        /// <summary>
        /// Deletes a geographical data item
        /// </summary>
        /// <param name="id">The ID of the geographical data item to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeographicalData(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Geographical data with ID {Id} not found for deletion", id);
                    return NotFound($"Geographical data with ID {id} not found");
                }

                _logger.LogInformation("Deleted geographical data with ID: {Id}", id);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting geographical data with ID: {Id}", id);
                return StatusCode(500, "Internal server error while deleting geographical data");
            }
        }
    }
}
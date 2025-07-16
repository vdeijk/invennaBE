using Microsoft.AspNetCore.Mvc;
using BE.Domain.Interfaces;
using BE.API.DTOs;
using Microsoft.AspNetCore.Http;

namespace BE.API.Controllers
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
        // ...rest of the controller code remains unchanged...
    }
}

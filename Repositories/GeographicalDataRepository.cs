using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Interfaces;
using BE.Models;
using Microsoft.Extensions.Logging;

namespace BE.Repositories
{
    public class GeographicalDataRepository : IGeographicalDataRepository
    {
        private readonly GeographicalDataContext _context;
        private readonly ILogger<GeographicalDataRepository> _logger;

        public GeographicalDataRepository(GeographicalDataContext context, ILogger<GeographicalDataRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<GeographicalData>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all geographical data from the database");
            var result = await _context.GeographicalData
                .AsNoTracking()
                .ToListAsync();
            _logger.LogInformation("Retrieved {Count} geographical data items", result.Count);
            return result;
        }

        public async Task<GeographicalData?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving geographical data with ID: {Id}", id);
            var item = await _context.GeographicalData
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Geographical data with ID {Id} not found", id);
            }
            return item;
        }

        public async Task<GeographicalData> CreateAsync(GeographicalData geographicalData)
        {
            _logger.LogInformation("Creating new geographical data entry");
            _context.GeographicalData.Add(geographicalData);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created geographical data with ID: {Id}", geographicalData.Id);
            return geographicalData;
        }

        public async Task<GeographicalData> UpdateAsync(GeographicalData geographicalData)
        {
            _logger.LogInformation("Updating geographical data with ID: {Id}", geographicalData.Id);
            _context.Entry(geographicalData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated geographical data with ID: {Id}", geographicalData.Id);
            return geographicalData;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting geographical data with ID: {Id}", id);
            var geographicalData = await _context.GeographicalData.FindAsync(id);
            if (geographicalData == null)
            {
                _logger.LogWarning("Attempted to delete non-existent geographical data with ID: {Id}", id);
                return false;
            }

            _context.GeographicalData.Remove(geographicalData);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted geographical data with ID: {Id}", id);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var exists = await _context.GeographicalData.AnyAsync(g => g.Id == id);
            _logger.LogInformation("Checked existence for geographical data with ID: {Id} - Exists: {Exists}", id, exists);
            return exists;
        }
    }
}
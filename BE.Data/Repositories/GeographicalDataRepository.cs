using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Domain.Interfaces;
using BE.Domain.Models;
using BE.Domain.DTOs;
using BE.Domain.Entities;
using GeographicalDataModel = BE.Domain.Entities.GeographicalDataEntity;

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

        public async Task<IEnumerable<GeographicalDataEntity>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all geographical data from the database");
            var result = await _context.GeographicalData
                .ToListAsync();
            _logger.LogInformation("Retrieved {Count} geographical data items", result.Count);
            return result;
        }

        public async Task<PagedResult<GeographicalDataEntity>> GetPagedAsync(PaginationParameters parameters)
        {
            _logger.LogInformation("Retrieving paged geographical data: Page {Page}, Size {PageSize}, Search: {Search}", 
                parameters.Page, parameters.PageSize, parameters.Search);

            var query = _context.GeographicalData.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var searchTerm = parameters.Search.ToLower();
                query = query.Where(g => 
                    g.Openbareruimte.ToLower().Contains(searchTerm) ||
                    g.Postcode.ToLower().Contains(searchTerm) ||
                    g.Woonplaats.ToLower().Contains(searchTerm) ||
                    g.Gemeente.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "openbareruimte" => parameters.SortDirection == SortDirection.Descending 
                        ? query.OrderByDescending(g => g.Openbareruimte)
                        : query.OrderBy(g => g.Openbareruimte),
                    "huisnummer" => parameters.SortDirection == SortDirection.Descending 
                        ? query.OrderByDescending(g => g.Huisnummer)
                        : query.OrderBy(g => g.Huisnummer),
                    "postcode" => parameters.SortDirection == SortDirection.Descending 
                        ? query.OrderByDescending(g => g.Postcode)
                        : query.OrderBy(g => g.Postcode),
                    "woonplaats" => parameters.SortDirection == SortDirection.Descending 
                        ? query.OrderByDescending(g => g.Woonplaats)
                        : query.OrderBy(g => g.Woonplaats),
                    "gemeente" => parameters.SortDirection == SortDirection.Descending 
                        ? query.OrderByDescending(g => g.Gemeente)
                        : query.OrderBy(g => g.Gemeente),
                    _ => query.OrderBy(g => g.Id)
                };
            }
            else
            {
                query = query.OrderBy(g => g.Id);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} items out of {Total} total for page {Page}", 
                items.Count, totalCount, parameters.Page);

            return new PagedResult<GeographicalDataEntity>
            {
                Items = items,
                TotalCount = totalCount,
                Page = parameters.Page,
                PageSize = parameters.PageSize
            };
        }

        public async Task<GeographicalDataEntity?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving geographical data with ID: {Id}", id);
            var item = await _context.GeographicalData
                .FirstOrDefaultAsync(g => g.Id == id);
            if (item == null)
            {
                _logger.LogWarning("Geographical data with ID {Id} not found", id);
            }
            return item;
        }

        public Task<GeographicalDataEntity> CreateAsync(GeographicalDataEntity geographicalData)
        {
            _logger.LogInformation("Creating new geographical data entry");
            _context.GeographicalData.Add(geographicalData);
            _logger.LogInformation("Prepared geographical data for creation (pending save)");
            return Task.FromResult(geographicalData);
        }

        public Task<GeographicalDataEntity> UpdateAsync(GeographicalDataEntity geographicalData)
        {
            _logger.LogInformation("Updating geographical data with ID: {Id}", geographicalData.Id);
            _context.Entry(geographicalData).State = EntityState.Modified;
            _logger.LogInformation("Prepared geographical data for update (pending save)");
            return Task.FromResult(geographicalData);
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
            _logger.LogInformation("Prepared geographical data for deletion (pending save)");
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
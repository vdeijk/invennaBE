using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public class GeographicalDataRepository : IGeographicalDataRepository
    {
        private readonly GeographicalDataContext _context;

        public GeographicalDataRepository(GeographicalDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GeographicalData>> GetAllAsync()
        {
            return await _context.GeographicalData
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GeographicalData?> GetByIdAsync(int id)
        {
            return await _context.GeographicalData
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<GeographicalData> CreateAsync(GeographicalData geographicalData)
        {
            _context.GeographicalData.Add(geographicalData);
            await _context.SaveChangesAsync();
            return geographicalData;
        }

        public async Task<GeographicalData> UpdateAsync(GeographicalData geographicalData)
        {
            _context.Entry(geographicalData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return geographicalData;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var geographicalData = await _context.GeographicalData.FindAsync(id);
            if (geographicalData == null)
            {
                return false;
            }

            _context.GeographicalData.Remove(geographicalData);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GeographicalData
                .AnyAsync(g => g.Id == id);
        }
    }
}

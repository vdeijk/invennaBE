using BE.Models;

namespace BE.Interfaces
{
    public interface IGeographicalDataRepository
    {
        Task<IEnumerable<GeographicalData>> GetAllAsync();
        Task<GeographicalData?> GetByIdAsync(int id);
        Task<GeographicalData> CreateAsync(GeographicalData geographicalData);
        Task<GeographicalData> UpdateAsync(GeographicalData geographicalData);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

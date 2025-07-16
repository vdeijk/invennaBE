using BE.API.DTOs;
using BE.Domain.Interfaces;
using BE.Domain.Models;
using Microsoft.Extensions.Logging;

namespace BE.Services
{
    public class GeographicalDataService : IGeographicalDataService
    {
        private readonly IGeographicalDataRepository _repository;
        private readonly ILogger<GeographicalDataService> _logger;

        public GeographicalDataService(IGeographicalDataRepository repository, ILogger<GeographicalDataService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<GeographicalDataDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(MapToDto).ToList();
        }

        public async Task<GeographicalDataDto?> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<GeographicalDataDto> CreateAsync(CreateGeographicalDataDto dto)
        {
            var entity = MapToEntity(dto);
            // Optionally add business logic/validation here
            var created = await _repository.CreateAsync(entity);
            return MapToDto(created);
        }

        public async Task<GeographicalDataDto?> UpdateAsync(int id, UpdateGeographicalDataDto dto)
        {
            if (id != dto.Id) return null;
            var entity = MapToEntity(dto);
            // Optionally add business logic/validation here
            var updated = await _repository.UpdateAsync(entity);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private GeographicalDataDto MapToDto(GeographicalData entity)
        {
            return new GeographicalDataDto
            {
                Id = entity.Id,
                Openbareruimte = entity.Openbareruimte,
                Huisnummer = entity.Huisnummer,
                Huisletter = entity.Huisletter,
                Huisnummertoevoeging = entity.Huisnummertoevoeging,
                Postcode = entity.Postcode,
                Woonplaats = entity.Woonplaats,
                Gemeente = entity.Gemeente,
                Provincie = entity.Provincie,
                Nummeraanduiding = entity.Nummeraanduiding,
                Verblijfsobjectgebruiksdoel = entity.Verblijfsobjectgebruiksdoel,
                Oppervlakteverblijfsobject = entity.Oppervlakteverblijfsobject,
                Verblijfsobjectstatus = entity.Verblijfsobjectstatus,
                ObjectId = entity.ObjectId,
                ObjectType = entity.ObjectType,
                Nevenadres = entity.Nevenadres,
                Pandid = entity.Pandid,
                Pandstatus = entity.Pandstatus,
                Pandbouwjaar = entity.Pandbouwjaar,
                X = entity.X,
                Y = entity.Y,
                Lon = entity.Lon,
                Lat = entity.Lat
            };
        }

        private GeographicalData MapToEntity(CreateGeographicalDataDto dto)
        {
            return new GeographicalData
            {
                Openbareruimte = dto.Openbareruimte,
                Huisnummer = dto.Huisnummer,
                Huisletter = dto.Huisletter,
                Huisnummertoevoeging = dto.Huisnummertoevoeging,
                Postcode = dto.Postcode,
                Woonplaats = dto.Woonplaats,
                Gemeente = dto.Gemeente,
                Provincie = dto.Provincie,
                Nummeraanduiding = dto.Nummeraanduiding,
                Verblijfsobjectgebruiksdoel = dto.Verblijfsobjectgebruiksdoel,
                Oppervlakteverblijfsobject = dto.Oppervlakteverblijfsobject,
                Verblijfsobjectstatus = dto.Verblijfsobjectstatus,
                ObjectId = dto.ObjectId,
                ObjectType = dto.ObjectType,
                Nevenadres = dto.Nevenadres,
                Pandid = dto.Pandid,
                Pandstatus = dto.Pandstatus,
                Pandbouwjaar = dto.Pandbouwjaar,
                X = dto.X,
                Y = dto.Y,
                Lon = dto.Lon,
                Lat = dto.Lat
            };
        }

        private GeographicalData MapToEntity(UpdateGeographicalDataDto dto)
        {
            var entity = MapToEntity((CreateGeographicalDataDto)dto);
            entity.Id = dto.Id;
            return entity;
        }
    }
}

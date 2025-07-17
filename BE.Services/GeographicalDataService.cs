using BE.Domain.DTOs;
using BE.Domain.Interfaces;
using BE.Domain.Models;
using BE.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BE.Services
{
    public class GeographicalDataService : IGeographicalDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GeographicalDataService> _logger;

        public GeographicalDataService(IUnitOfWork unitOfWork, ILogger<GeographicalDataService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<GeographicalDataDto>> GetAllAsync()
        {
            var data = await _unitOfWork.GeographicalData.GetAllAsync();
            return data.Select(MapToDto).ToList();
        }

        public async Task<GeographicalDataDto?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.GeographicalData.GetByIdAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<GeographicalDataDto> CreateAsync(CreateGeographicalDataDto dto)
        {
            var entity = MapToEntity(dto);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var created = await _unitOfWork.GeographicalData.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return MapToDto(created);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<GeographicalDataDto?> UpdateAsync(int id, UpdateGeographicalDataDto dto)
        {
            if (id != dto.Id) return null;
            var entity = MapToEntity(dto);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updated = await _unitOfWork.GeographicalData.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return MapToDto(updated);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.GeographicalData.DeleteAsync(id);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
                return result;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PagedResult<GeographicalDataDto>> GetPagedAsync(PaginationParameters parameters)
        {
            var pagedResult = await _unitOfWork.GeographicalData.GetPagedAsync(parameters);
            
            return new PagedResult<GeographicalDataDto>
            {
                Items = pagedResult.Items.Select(MapToDto).ToList(),
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize
            };
        }

        private GeographicalDataDto MapToDto(GeographicalDataEntity entity)
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

        private GeographicalDataEntity MapToEntity(CreateGeographicalDataDto dto)
        {
            return new GeographicalDataEntity
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

        private GeographicalDataEntity MapToEntity(UpdateGeographicalDataDto dto)
        {
            var entity = MapToEntity((CreateGeographicalDataDto)dto);
            entity.Id = dto.Id;
            return entity;
        }
    }
}

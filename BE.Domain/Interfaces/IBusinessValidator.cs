using BE.Domain.DTOs;
using BE.Domain.Models;

namespace BE.Domain.Interfaces
{
    public interface IBusinessValidator
    {
        Task<ValidationResult> ValidateCreateAsync(CreateGeographicalDataDto dto);
        Task<ValidationResult> ValidateUpdateAsync(UpdateGeographicalDataDto dto);
        ValidationResult ValidateCoordinates(double lat, double lon, int x, int y);
        ValidationResult ValidatePostcode(string postcode);
        ValidationResult ValidateAddress(string openbareruimte, int huisnummer, string? huisletter, int? huisnummertoevoeging);
        ValidationResult ValidateBuildingYear(int year);
    }
}

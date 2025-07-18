using BE.Domain.DTOs;
using BE.Domain.Interfaces;
using BE.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BE.Services
{
    public class BusinessValidator : IBusinessValidator
    {
        private readonly ILogger<BusinessValidator> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private const double NL_MIN_LAT = 50.7503;
        private const double NL_MAX_LAT = 53.5542;
        private const double NL_MIN_LON = 3.3316;
        private const double NL_MAX_LON = 7.2275;

        private const int RD_MIN_X = 0;
        private const int RD_MAX_X = 300000;
        private const int RD_MIN_Y = 300000;
        private const int RD_MAX_Y = 650000;

        public BusinessValidator(ILogger<BusinessValidator> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult> ValidateCreateAsync(CreateGeographicalDataDto dto)
        {
            var result = new ValidationResult { IsValid = true };

            var addressValidation = ValidateAddress(dto.Openbareruimte, dto.Huisnummer, dto.Huisletter, dto.Huisnummertoevoeging);
            result.Combine(addressValidation);

            var postcodeValidation = ValidatePostcode(dto.Postcode);
            result.Combine(postcodeValidation);

            var coordinatesValidation = ValidateCoordinates(dto.Lat, dto.Lon, dto.X, dto.Y);
            result.Combine(coordinatesValidation);

            var yearValidation = ValidateBuildingYear(dto.Pandbouwjaar);
            result.Combine(yearValidation);

            await ValidateDuplicateAddress(dto, result);

            ValidateSurfaceArea(dto, result);

            ValidateLocationConsistency(dto, result);

            return result;
        }

        public async Task<ValidationResult> ValidateUpdateAsync(UpdateGeographicalDataDto dto)
        {
            var result = await ValidateCreateAsync(dto);

            if (dto.Id <= 0)
            {
                result.AddFieldError(nameof(dto.Id), "ID moet groter dan 0 zijn voor updates");
            }

            var exists = await _unitOfWork.GeographicalData.GetByIdAsync(dto.Id);
            if (exists == null)
            {
                result.AddError($"Entiteit met ID {dto.Id} bestaat niet");
            }

            return result;
        }

        public ValidationResult ValidateCoordinates(double lat, double lon, int x, int y)
        {
            var result = new ValidationResult { IsValid = true };

            if (lat < NL_MIN_LAT || lat > NL_MAX_LAT)
            {
                result.AddFieldError("Lat", $"Latitude moet binnen Nederland liggen ({NL_MIN_LAT} - {NL_MAX_LAT})");
            }

            if (lon < NL_MIN_LON || lon > NL_MAX_LON)
            {
                result.AddFieldError("Lon", $"Longitude moet binnen Nederland liggen ({NL_MIN_LON} - {NL_MAX_LON})");
            }

            if (x < RD_MIN_X || x > RD_MAX_X)
            {
                result.AddFieldError("X", $"X-coördinaat moet binnen RD-bereik liggen ({RD_MIN_X} - {RD_MAX_X})");
            }

            if (y < RD_MIN_Y || y > RD_MAX_Y)
            {
                result.AddFieldError("Y", $"Y-coördinaat moet binnen RD-bereik liggen ({RD_MIN_Y} - {RD_MAX_Y})");
            }

            return result;
        }

        public ValidationResult ValidatePostcode(string postcode)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrWhiteSpace(postcode))
            {
                result.AddFieldError("Postcode", "Postcode is verplicht");
                return result;
            }

            var cleanPostcode = postcode.Replace(" ", "").ToUpper();

            var postcodePattern = @"^[1-9][0-9]{3}[A-Z]{2}$";
            if (!Regex.IsMatch(cleanPostcode, postcodePattern))
            {
                result.AddFieldError("Postcode", "Postcode moet een geldige Nederlandse postcode zijn (bijv. 1234AB)");
            }

            if (cleanPostcode.StartsWith("0000") || cleanPostcode.StartsWith("9999"))
            {
                result.AddFieldError("Postcode", "Postcode mag niet beginnen met 0000 of 9999");
            }

            return result;
        }

        public ValidationResult ValidateAddress(string openbareruimte, int huisnummer, string? huisletter, int? huisnummertoevoeging)
        {
            var result = new ValidationResult { IsValid = true };

            if (Regex.IsMatch(openbareruimte.Trim(), @"^\d+$"))
            {
                result.AddFieldError("Openbareruimte", "Openbare ruimte mag niet alleen uit cijfers bestaan");
            }

            if (!string.IsNullOrEmpty(huisletter) && huisnummertoevoeging.HasValue)
            {
                _logger.LogInformation("Address heeft zowel huisletter als toevoeging: {Street} {Number}{Letter}-{Addition}", 
                    openbareruimte, huisnummer, huisletter, huisnummertoevoeging);
            }

            if (huisnummer > 10000)
            {
                result.AddError($"Huisnummer {huisnummer} is ongewoon hoog - controleer of dit correct is");
            }

            return result;
        }

        public ValidationResult ValidateBuildingYear(int year)
        {
            var result = new ValidationResult { IsValid = true };
            var currentYear = DateTime.Now.Year;

            if (year < 1000)
            {
                result.AddFieldError("Pandbouwjaar", "Bouwjaar kan niet voor het jaar 1000 zijn");
            }
            else if (year > currentYear + 10)
            {
                result.AddFieldError("Pandbouwjaar", $"Bouwjaar kan niet meer dan 10 jaar in de toekomst zijn (max {currentYear + 10})");
            }

            if (year < 1200 && year >= 1000)
            {
                _logger.LogWarning("Zeer oud gebouw geregistreerd: bouwjaar {Year}", year);
            }

            return result;
        }

        private async Task ValidateDuplicateAddress(CreateGeographicalDataDto dto, ValidationResult result)
        {
            try
            {
                var allData = await _unitOfWork.GeographicalData.GetAllAsync();
                var duplicate = allData.FirstOrDefault(x => 
                    x.Openbareruimte.Equals(dto.Openbareruimte, StringComparison.OrdinalIgnoreCase) &&
                    x.Huisnummer == dto.Huisnummer &&
                    x.Huisletter == dto.Huisletter &&
                    x.Huisnummertoevoeging == dto.Huisnummertoevoeging &&
                    x.Postcode.Replace(" ", "").Equals(dto.Postcode.Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

                if (duplicate != null)
                {
                    result.AddError($"Adres bestaat al in de database (ID: {duplicate.Id})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Kon niet controleren op duplicate adressen");
            }
        }

        private void ValidateSurfaceArea(CreateGeographicalDataDto dto, ValidationResult result)
        {
            if (dto.Oppervlakteverblijfsobject <= 0)
            {
                result.AddFieldError("Oppervlakteverblijfsobject", "Oppervlakte moet groter dan 0 zijn");
            }
            else if (dto.Oppervlakteverblijfsobject > 100000) 
            {
                result.AddError($"Oppervlakte van {dto.Oppervlakteverblijfsobject} m² is ongewoon groot - controleer of dit correct is");
            }
            else if (dto.Oppervlakteverblijfsobject < 10)
            {
                result.AddError($"Oppervlakte van {dto.Oppervlakteverblijfsobject} m² is ongewoon klein - controleer of dit correct is");
            }
        }

        private void ValidateLocationConsistency(CreateGeographicalDataDto dto, ValidationResult result)
        {
            var postcodeDigits = dto.Postcode.Substring(0, 4);
            
            if (postcodeDigits.StartsWith("10") || postcodeDigits.StartsWith("11") || postcodeDigits.StartsWith("12"))
            {
                if (!dto.Woonplaats.Contains("Amsterdam", StringComparison.OrdinalIgnoreCase) && 
                    !dto.Gemeente.Contains("Amsterdam", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Postcode {Postcode} suggereert Amsterdam regio, maar plaats is {Place}", 
                        dto.Postcode, dto.Woonplaats);
                }
            }
        }
    }
}

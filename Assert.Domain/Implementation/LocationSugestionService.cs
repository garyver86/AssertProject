using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;

namespace Assert.Domain.Implementation
{
    public class LocationSugestionService : ILocationSugestionService
    {
        private readonly ICityRepository _cityRepository; // Podrías renombrarlo a ILocationRepository si buscas más allá de ciudades

        public LocationSugestionService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<ReturnModel<List<LocationSuggestion>>> GetLocationSuggestions(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter) || filter.Length < 2) // O algún umbral mínimo para el filtro
            {
                return new ReturnModel<List<LocationSuggestion>>(); // No devolver sugerencias si el filtro es muy corto
            }

            var suggestions = new List<LocationSuggestion>();

            // 1. Buscar Países
            var countries = await _cityRepository.SearchCountries(filter);
            suggestions.AddRange(countries.Select(c => new LocationSuggestion
            {
                Id = $"country_{c.CountryId}",
                Name = c.Name,
                Description = c.Description,
                Type = LocationType.Country,
                EntityId = c.CountryId,
                TypeDesc = "country"
            }));

            var states = await _cityRepository.SearchStates(filter);
            suggestions.AddRange(states.Select(s => new LocationSuggestion
            {
                Id = $"state_{s.StateId}",
                Name = s.Name,
                Description = $"Departamento de {s.Name}, {s.Country?.Name}",
                Type = LocationType.State,
                EntityId = s.StateId,
                TypeDesc = "state"
            }));

            var counties = await _cityRepository.SearchCounties(filter);
            suggestions.AddRange(counties.Select(co => new LocationSuggestion
            {
                Id = $"county_{co.CountyId}",
                Name = co.Name,
                Description = $"Provincia de {co.Name}, {co.State?.Name}, {co.State?.Country?.Name}",
                Type = LocationType.County,
                EntityId = co.CountyId,
                TypeDesc = "county"
            }));

            var cities = await _cityRepository.SearchCities(filter);
            suggestions.AddRange(cities.Select(c => new LocationSuggestion
            {
                Id = $"city_{c.CityId}",
                Name = c.Name,
                Description = $"{c.Name},{c.County?.State?.Name}, {c.County?.State?.Country?.Name}", 
                Type = LocationType.City,
                EntityId = c.CityId,
                TypeDesc = "city"
            }));
                        
            suggestions = suggestions.GroupBy(s => s.Id)
                                     .Select(g => g.First())
                                     .ToList();

            var result = suggestions.OrderBy(s => GetLocationTypeOrder(s.Type))
                              .ThenBy(s => s.Name)
                              .ToList();
            return new ReturnModel<List<LocationSuggestion>>
            {
                StatusCode = ResultStatusCode.OK,
                Data = result
            };
        }

        private int GetLocationTypeOrder(LocationType type)
        {
            return type switch
            {
                LocationType.City => 1,
                LocationType.County => 2,
                LocationType.State => 3,
                LocationType.Country => 4,
                _ => 99,
            };
        }
    }
}

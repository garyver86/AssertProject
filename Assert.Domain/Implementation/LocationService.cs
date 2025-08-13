using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Assert.Domain.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly ICityRepository _cityRepository;

        private readonly IFuzzyMatcher _fuzzyMatcher;

        public LocationService(ICityRepository cityRepository, IFuzzyMatcher fuzzyMatcher)
        {
            _cityRepository = cityRepository;
            _fuzzyMatcher = fuzzyMatcher;
        }

        public async Task<LocationModel?> ResolveLocation(string? country, string? state, string? county, string? city, string? street)
        {
            // Paso 1: Normalización de inputs
            var normalizedInput = new NormalizedLocationInput(
               Utils.Tools.NormalizeText(country),
                Utils.Tools.NormalizeText(state),
                Utils.Tools.NormalizeText(county),
                Utils.Tools.NormalizeText(city)
            );

            // Paso 2: Búsqueda jerárquica con tolerancia a variaciones
            LocationModel result = new LocationModel();

            // Buscar país primero
            if (!normalizedInput.Country.IsNullOrEmpty())
            {
                result.CountryId = await _cityRepository.FindBestCountryMatch(normalizedInput.Country);
            }

            // Luego estado/provincia
            if (result.CountryId > 0 && !normalizedInput.State.IsNullOrEmpty())
            {
                result.StateId = await _cityRepository.FindBestStateMatch(normalizedInput.State, result.CountryId);
            }
            else if (!normalizedInput.State.IsNullOrEmpty())
            {
                result.StateId = -1; // Indica que se proporcionó estado pero no se encontró
            }

            // Luego condado
            if (result.StateId > 0 && !normalizedInput.County.IsNullOrEmpty())
            {
                result.CountyId = await _cityRepository.FindBestCountyMatch(normalizedInput.County, result.StateId);
            }
            else if (!normalizedInput.County.IsNullOrEmpty())
            {
                result.CountyId = -1; // Indica que se proporcionó condado pero no se encontró
            }

            // Finalmente ciudad
            if (result.CountyId > 0 && !normalizedInput.City.IsNullOrEmpty())
            {
                result.CityId = await _cityRepository.FindBestCityMatch(normalizedInput.City, result.CountyId);
            }
            else if (!normalizedInput.City.IsNullOrEmpty())
            {
                result.CityId = -1; // Indica que se proporcionó ciudad pero no se encontró
            }
            return result;
        }



        /// <summary>
        /// Busca ubicaciones (ciudades, condados, estados, países) según un filtro
        /// y las agrupa en una estructura jerárquica de GroupedResponse.
        /// </summary>
        /// <param name="filter">El texto a filtrar.</param>
        /// <param name="filterType">
        /// 0: City, 1: Country, 2: State, 3: County, 4/-1: Búsqueda general (City name, County name, State name, Country name).
        /// </param>
        /// <returns>Una estructura GroupedResponse con los datos geográficos agrupados.</returns>
        public async Task<GroupedLocationResponse> SearchAndGroupLocations(string filter, int filterType)
        {
            // Obtener la lista plana de TCity con sus jerarquías cargadas desde el repositorio
            var foundCities = await _cityRepository.FindByFilter(filter, filterType);

            if (!foundCities.Any())
            {
                return new GroupedLocationResponse { data = new List<CountryGrouped>() };
            }

            // --- Lógica de Agrupación Específica (ya no genérica, adaptada a TCity) ---
            var groupedCountries = new Dictionary<int, CountryGrouped>();
            var groupedStates = new Dictionary<int, StateGrouped>();
            var groupedCounties = new Dictionary<int, CountyGrouped>();

            foreach (var city in foundCities)
            {
                // Verificaciones de nulidad (es buena práctica, aunque EF Core debería cargar esto con Include)
                if (city.County == null || city.County.State == null || city.County.State.Country == null)
                {
                    continue;
                }

                var countryEntity = city.County.State.Country;
                var stateEntity = city.County.State;
                var countyEntity = city.County;

                // 1. Agrupar por País
                if (!groupedCountries.TryGetValue(countryEntity.CountryId, out var countryGrouped))
                {
                    countryGrouped = new CountryGrouped
                    {
                        countryId = countryEntity.CountryId,
                        name = countryEntity.Name
                    };
                    groupedCountries.Add(countryEntity.CountryId, countryGrouped);
                }

                // 2. Agrupar por Estado
                if (!groupedStates.TryGetValue(stateEntity.StateId, out var stateGrouped))
                {
                    stateGrouped = new StateGrouped
                    {
                        stateId = stateEntity.StateId,
                        name = stateEntity.Name
                    };
                    countryGrouped.states.Add(stateGrouped);
                    groupedStates.Add(stateEntity.StateId, stateGrouped);
                }

                // 3. Agrupar por Condado
                if (!groupedCounties.TryGetValue(countyEntity.CountyId, out var countyGrouped))
                {
                    countyGrouped = new CountyGrouped
                    {
                        countyId = countyEntity.CountyId,
                        name = countyEntity.Name
                    };
                    stateGrouped.counties.Add(countyGrouped);
                    groupedCounties.Add(countyEntity.CountyId, countyGrouped);
                }

                // 4. Añadir la Ciudad
                // Esto asegura que una ciudad solo se agregue una vez a un condado si aparece múltiples veces
                // en la lista 'foundCities' por alguna razón.
                if (!countyGrouped.cities.Any(c => c.cityId == city.CityId))
                {
                    countyGrouped.cities.Add(new CityGrouped
                    {
                        cityId = city.CityId, // Usamos CityId de TCity
                        name = city.Name // Usamos Name de TCity
                    });
                }
            }

            return new GroupedLocationResponse
            {
                data = groupedCountries.Values.ToList()
            };
        }
    }
}

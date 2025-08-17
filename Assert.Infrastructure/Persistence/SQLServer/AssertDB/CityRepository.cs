using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CityRepository : ICityRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IFuzzyMatcher _fuzzyMatcher;
        public CityRepository(InfraAssertDbContext infraAssertDbContext, IFuzzyMatcher fuzzyMatcher)
        {
            _context = infraAssertDbContext;
            _fuzzyMatcher = fuzzyMatcher;
        }

        // Método general para aplicar filtros de habilitación y cargar relaciones
        private IQueryable<TCity> ApplyBaseFiltersAndIncludes(IQueryable<TCity> query)
        {
            return query
                .Where(c => !(c.IsDisabled ?? false))
                .Include(c => c.County)
                    .Where(c => !(c.County.IsDisabled ?? false)) // Filtra condados deshabilitados
                .Include(c => c.County.State)
                    .Where(c => !(c.County.State.IsDisabled ?? false)) // Filtra estados deshabilitados
                .Include(c => c.County.State.Country)
                    .Where(c => !(c.County.State.Country.IsDisabled ?? false)); // Filtra países deshabilitados
        }

        private IQueryable<TCounty> ApplyBaseFiltersAndIncludes(IQueryable<TCounty> query)
        {
            return query
                .Where(c => !(c.IsDisabled ?? false))
                .Include(c => c.State)
                    .Where(c => !(c.State.IsDisabled ?? false)) // Filtra estados deshabilitados
                .Include(c => c.State.Country)
                    .Where(c => !(c.State.Country.IsDisabled ?? false)); // Filtra países deshabilitados
        }
        private IQueryable<TState> ApplyBaseFiltersAndIncludes(IQueryable<TState> query)
        {
            return query
                .Where(c => !(c.IsDisabled ?? false))
                .Include(c => c.Country)
                    .Where(c => !(c.Country.IsDisabled ?? false)); // Filtra países deshabilitados
        }
        private IQueryable<TCountry> ApplyBaseFiltersAndIncludes(IQueryable<TCountry> query)
        {
            return query
                .Where(c => !(c.IsDisabled ?? false));
        }

        public async Task<List<TCity>> Find(string filter)
        {
            return await _context.TCities
           .Where(c => !(c.IsDisabled ?? false) && c.Name.StartsWith(filter))
           .AsNoTracking()
           .ToListAsync();
        }

        public async Task<List<TCity>> FindByFilter(string filter, int filterType)
        {
            IQueryable<TCity> query = _context.TCities;

            // Aplica filtros base y carga las relaciones ANTES de añadir el filtro de búsqueda
            query = ApplyBaseFiltersAndIncludes(query);

            // Aplica el filtro de búsqueda específico según el filterType
            if (filterType == 4 || filterType == -1) // 'null' en tu ejemplo, usaremos -1 para evitar posibles conflictos de tipo
            {
                // Búsqueda general por ciudad (o predeterminado)
                query = query.Where(c => c.Name.Contains(filter));
            }
            else if (filterType == 1) // Country
            {
                query = query.Where(c => c.County.State.Country.Name.Contains(filter));
            }
            else if (filterType == 2) // State
            {
                query = query.Where(c => c.County.State.Name.Contains(filter));
            }
            else if (filterType == 3) // County
            {
                query = query.Where(c => c.County.Name.Contains(filter));
            }
            else if (filterType == 0)
            {
                query = query.Where(c => c.County.State.Country.Name.Contains(filter) || c.County.State.Name.Contains(filter) ||
                c.County.Name.Contains(filter) || c.Name.Contains(filter));
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TCity> GetById(int cityId)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCities)
               .Where(c => c.CityId == cityId)
               .Include(c => c.County)
                   .ThenInclude(co => co.State)
                       .ThenInclude(s => s.Country)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            return result;
        }
        public async Task<TCounty> GetCountyById(int countyId)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCounties)
               .Where(c => c.CountyId == countyId)
                   .Include(co => co.State)
                       .ThenInclude(s => s.Country)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            return result;
        }
        public async Task<TState> GetStateById(int stateId)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TStates)
               .Where(c => c.StateId == stateId)
                       .Include(s => s.Country)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            return result;
        }
        public async Task<TCountry> GetCountryById(int countryId)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCountries)
               .Where(c => c.CountryId == countryId)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            return result;
        }

        public async Task<TCity> GetByListingRentId(long listingRentId)
        {
            var result = (await _context.TlListingRents.Where(x => x.ListingRentId == listingRentId).FirstOrDefaultAsync())?.TpProperties.FirstOrDefault().City;
            return result;
        }


        public async Task<List<TCountry>> SearchCountries(string filter)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCountries)
                .Where(c => c.Name.Contains(filter))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<List<TState>> SearchStates(string filter)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TStates)
                .Where(s => s.Name.Contains(filter))
                .Include(s => s.Country) // Incluye el país para la descripción
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<List<TCounty>> SearchCounties(string filter)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCounties)
                .Where(co => co.Name.Contains(filter))
                .Include(co => co.State) // Incluye el estado para la descripción
                    .ThenInclude(s => s.Country) // Y el país
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        // Tu método FindAllCitiesRelatedToFilter podría renombrarse/adaptarse para buscar ciudades específicas
        // Para las sugerencias, un método simple de búsqueda de ciudades es suficiente.
        public async Task<List<TCity>> SearchCities(string filter)
        {
            var result = await ApplyBaseFiltersAndIncludes(_context.TCities)
                .Where(c => c.Name.Contains(filter))
                .Include(c => c.County)
                    .ThenInclude(co => co.State)
                        .ThenInclude(s => s.Country)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        public async Task<int?> FindBestCountryMatch(string normalizedCountryName)
        {
            // Primero intentar búsqueda exacta
            var exactMatch = await _context.TCountries
                .FirstOrDefaultAsync(c => c.NormalizedName == normalizedCountryName);

            if (exactMatch != null) return exactMatch.CountryId;

            // Fallback a fuzzy matching
            var allCountries = await _context.TCountries.ToListAsync();
            var bestMatch = allCountries
                .Select(c => new
                {
                    CountryId = c.CountryId,
                    Score = _fuzzyMatcher.Compare(c.NormalizedName, normalizedCountryName)
                })
                .Where(x => x.Score >= 0.7) // Umbral de similitud
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestMatch?.CountryId;
        }
        public async Task<int?> FindBestStateMatch(string normalizedStateName, int? countryId)
        {
            // Primero intentar búsqueda exacta
            var exactMatch = await _context.TStates
                .FirstOrDefaultAsync(c => c.CountryId == countryId && c.NormalizedName == normalizedStateName);

            if (exactMatch != null) return exactMatch.StateId;

            // Fallback a fuzzy matching
            var allStates = await _context.TStates.Where(x => x.CountryId == countryId).ToListAsync();
            var bestMatch = allStates
                .Select(c => new
                {
                    StateId = c.StateId,
                    Score = _fuzzyMatcher.Compare(c.NormalizedName, normalizedStateName)
                })
                .Where(x => x.Score >= 0.7) // Umbral de similitud
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestMatch?.StateId;
        }

        public async Task<int?> FindBestCountyMatch(string normalizedCountyName, int? stateId)
        {
            // Primero intentar búsqueda exacta
            var exactMatch = await _context.TCounties
                .FirstOrDefaultAsync(c => c.StateId == stateId && c.NormalizedName == normalizedCountyName);

            if (exactMatch != null) return exactMatch.CountyId;

            // Fallback a fuzzy matching
            var allStates = await _context.TCounties.Where(x => x.StateId == stateId).ToListAsync();
            var bestMatch = allStates
                .Select(c => new
                {
                    CountyId = c.CountyId,
                    Score = _fuzzyMatcher.Compare(c.NormalizedName, normalizedCountyName)
                })
                .Where(x => x.Score >= 0.7) // Umbral de similitud
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestMatch?.CountyId;
        }

        public async Task<int?> FindBestCityMatch(string normalizedCityName, int? countyId)
        {
            // Primero intentar búsqueda exacta
            var exactMatch = await _context.TCities
                .FirstOrDefaultAsync(c => c.CountyId == countyId && c.NormalizedName == normalizedCityName);

            if (exactMatch != null) return exactMatch.CityId;

            // Fallback a fuzzy matching
            var allStates = await _context.TCities.Where(x => x.CountyId == countyId).ToListAsync();
            var bestMatch = allStates
                .Select(c => new
                {
                    CityId = c.CityId,
                    Score = _fuzzyMatcher.Compare(c.NormalizedName, normalizedCityName)
                })
                .Where(x => x.Score >= 0.7) // Umbral de similitud
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            return bestMatch?.CityId;
        }

        public TCity RegisterLocation(string country, string state, string county, string city, string? street, LocationModel result)
        {
            if (result.CityId > 0)
            {
                return _context.TCities.Where(x => x.CityId == result.CityId).Include(x => x.County).ThenInclude(x => x.State).FirstOrDefault();
            }
            else
            {
                TCity newCity = new TCity
                {
                    Name = city,
                    NormalizedName = NormalizeText(city),
                    CountyId = result.CountyId > 0 ? result.CountyId ?? -1 : -1,
                    //Street = street,
                    IsDisabled = false
                };
                // Si el condado no existe, lo creamos
                if (result.CountyId <= 0)
                {
                    TCounty newCounty = new TCounty
                    {
                        Name = county,
                        NormalizedName = NormalizeText(county),
                        StateId = result.StateId > 0 ? result.StateId : null,
                        IsDisabled = false
                    };
                    // Si el estado no existe, lo creamos
                    if (result.StateId <= 0)
                    {
                        TState newState = new TState
                        {
                            Name = state,
                            NormalizedName = NormalizeText(state),
                            CountryId = result.CountryId > 0 ? result.CountryId ?? -1 : -1,
                            IsDisabled = false
                        };
                        // Si el país no existe, lo creamos
                        if (result.CountryId <= 0)
                        {
                            TCountry newCountryEntity = new TCountry
                            {
                                Name = country,
                                NormalizedName = NormalizeText(country),
                                IsDisabled = false
                            };
                            _context.TCountries.Add(newCountryEntity);
                            _context.SaveChanges();
                            newState.CountryId = newCountryEntity.CountryId;
                        }
                        _context.TStates.Add(newState);
                        _context.SaveChanges();
                        newCounty.StateId = newState.StateId;
                    }
                    _context.TCounties.Add(newCounty);
                    _context.SaveChanges();
                    newCity.CountyId = newCounty.CountyId;
                }
                _context.TCities.Add(newCity);
                _context.SaveChanges();

                var fullCity = _context.TCities
                    .Include(c => c.County)
                        .ThenInclude(cty => cty.State)
                            .ThenInclude(st => st.Country)
                    .FirstOrDefault(c => c.CityId == newCity.CityId);

                return fullCity!;
            }
        }
        public static string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Trim()
                .ToLowerInvariant()
                .RemoveDiacritics() // Eliminar acentos
                .RemovePunctuation() // Eliminar puntuación
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u")
                .Replace("-", " ")
                .Replace("  ", " ");
        }
    }
}

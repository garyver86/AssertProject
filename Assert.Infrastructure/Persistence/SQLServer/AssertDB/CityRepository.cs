using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CityRepository : ICityRepository
    {
        private readonly InfraAssertDbContext _context;
        public CityRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
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
    }
}

using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CityRepository : ICityRepository
    {
        private readonly InfraAssertDbContext _context;
        public CityRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TCity>> Find(string filter)
        {
            return await _context.TCities
           .Where(c => !(c.IsDisabled ?? false) && c.Name.StartsWith(filter))
           .AsNoTracking()
           .ToListAsync();
        }
        public async Task<List<TCity>> FindByFilter(string filter)
        {
            //var cities = await _context.TCities
            //.Where(c => c.Name.StartsWith(filter) &&
            //            !(c.IsDisabled ?? false))
            //.Include(c => c.County)
            //    .Where(co => !(co.IsDisabled ?? false)) // Filtrar condados deshabilitados
            //.Include(c => c.State)
            //    .Where(s => !(s.IsDisabled ?? false)) // Filtrar Estados deshabilitados
            //.Include(c => c.County)
            //    .Where(c => !(c.IsDisabled ?? false)) // Filtrar estados y países deshabilitados
            //.AsNoTracking()
            //.ToListAsync();

            var cities = await _context.TCities
            .Where(c => c.Name.Contains(filter) &&
                        !(c.IsDisabled ?? false))
            .Include(c => c.County)
                .Where(co => !(co.County.IsDisabled ?? false))
                .Include(co => co.County.State)
                    .Where(s => !(s.County.State.IsDisabled ?? false))
                    .Include(s => s.County.State.Country)
                        .Where(country => !(country.County.State.Country.IsDisabled ?? false))
            .AsNoTracking()
            .ToListAsync();

            return cities;
        }

        public TCity GetById(int cityId)
        {
            throw new NotImplementedException();
        }

        public async Task<TCity> GetByListingRentId(long listingRentId)
        {
            var result = (await _context.TpPropertyAddresses.Where(x => x.Property.ListingRentId == listingRentId).FirstOrDefaultAsync())?.City;
            return result;
        }
    }
}

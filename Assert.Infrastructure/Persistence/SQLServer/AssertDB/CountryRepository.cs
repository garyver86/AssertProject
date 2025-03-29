using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CountryRepository : ICountryRepository
    {
        private readonly InfraAssertDbContext _context;
        public CountryRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<IEnumerable<TCountry>> Find(string filter)
        {
            return await _context.TCountries
           .Where(co => !(co.IsDisabled ?? false) && co.Name.StartsWith(filter))
           .AsNoTracking()
           .ToListAsync();
        }

        public async Task<TCountry> GetByStateId(long stateId)
        {
            var result = (await _context.TStates.Where(x => x.StateId == stateId).FirstOrDefaultAsync())?.Country;
            return result;
        }
    }
}

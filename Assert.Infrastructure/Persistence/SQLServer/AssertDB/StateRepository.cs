﻿using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class StateRepository : IStateRepository
    {
        private readonly InfraAssertDbContext _context;
        public StateRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<IEnumerable<TState>> Find(string filter)
        {
            return await _context.TStates
           .Where(co => !(co.IsDisabled ?? false) && co.Name.StartsWith(filter))
           .AsNoTracking()
           .ToListAsync();
        }

        public async Task<TState> GetByCityId(long cityId)
        {
            var result = (await _context.TCities.Where(x => x.CityId == cityId).FirstOrDefaultAsync())?.County.State;
            return result;
        }
    }
}

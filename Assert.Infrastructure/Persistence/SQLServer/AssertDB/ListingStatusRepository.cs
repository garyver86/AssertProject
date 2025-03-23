using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingStatusRepository : IListingStatusRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingStatusRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TlListingStatus> Get(int id)
        {
            var result = _context.TlListingStatuses.Where(x => x.ListingStatusId == id).FirstOrDefault();
            return await Task.FromResult(result);
        }
        public async Task<TlListingStatus> Get(string code)
        {
            var result = _context.TlListingStatuses.Where(x => x.Code == code).FirstOrDefault();
            return await Task.FromResult(result);
        }
    }
}

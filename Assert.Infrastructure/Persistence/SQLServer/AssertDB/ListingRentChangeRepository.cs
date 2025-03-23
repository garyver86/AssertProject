using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingRentChangeRepository : IListingrentChangeRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingRentChangeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task Register(long listingRentId, string action, string browseInfo, bool? isMobile, string ipAddress, string additionalData, string applicationCode)
        {
            TlListingRentChange change = new TlListingRentChange
            {
                AdditionalData = additionalData,
                ApplicationCode = applicationCode,
                DateTimeChange = DateTime.Now,
                IpAddress = ipAddress ?? "",
                IsMobile = isMobile,
                ListingRentId = listingRentId,
                BrowserInfoInfo = browseInfo,
                ActionChange = action ?? "Undefined"
            };
            _context.TlListingRentChanges.Add(change);
            await _context.SaveChangesAsync();
        }
    }
}

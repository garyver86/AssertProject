using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingLogRepository : IListingLogRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingLogRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task RegisterLog(long ListingRentId, string Action, string BrowserInfoInfo, bool? IsMobile, string _IPAddress, string AdditionalData, string ApplicationCode)
        {
            TlListingRentChange change = new TlListingRentChange
            {
                AdditionalData = AdditionalData,
                ApplicationCode = ApplicationCode,
                DateTimeChange = DateTime.Now,
                IpAddress = _IPAddress ?? "",
                IsMobile = IsMobile,
                ListingRentId = ListingRentId,
                BrowserInfoInfo = BrowserInfoInfo,
                ActionChange = Action ?? "Undefined"
            };
            _context.TlListingRentChanges.Add(change);
            await _context.SaveChangesAsync();
        }

        public async Task RegisterBulkLog(List<long> listingRentIds, 
            string action, string browserInfo, bool? isMobile, string ipAddress, 
            string additionalData, string applicationCode)
        {
            if (listingRentIds == null || !listingRentIds.Any())
                return;

            var now = DateTime.UtcNow;

            var changes = listingRentIds.Select(id => new TlListingRentChange
            {
                ListingRentId = id,
                ActionChange = action ?? "Undefined",
                BrowserInfoInfo = browserInfo,
                IsMobile = isMobile,
                IpAddress = ipAddress ?? string.Empty,
                AdditionalData = additionalData,
                ApplicationCode = applicationCode,
                DateTimeChange = now
            }).ToList();

            _context.TlListingRentChanges.AddRange(changes);
            await _context.SaveChangesAsync();
        }


    }
}

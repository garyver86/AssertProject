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
    }
}
